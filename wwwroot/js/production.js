
//chart.js Resets
let chartInstance = null;
let analysisChart = null;

document.addEventListener("DOMContentLoaded", () => {
    loadMachines();
    loadTimeEntries();
    loadAnalysis();

    document.getElementById("filterForm")
        .addEventListener("submit", e => {
            e.preventDefault();
            loadTimeEntries();
            loadAnalysis();
        });

});

function statusToText(status) {

    switch (status) {
        case 0: return "Setup";
        case 1: return "Production";
        case 2: return "Downtime";
        case 3: return "Rework";
    }
}



async function loadTimeEntries() {

    const startDate = document.getElementById("startDate").value;
    const endDate = document.getElementById("endDate").value;
    const machineId = document.getElementById("machineSelect").value;
    const params = new URLSearchParams();

    if (startDate) params.append("startDate", startDate);
    if (endDate) params.append("endDate", endDate);
    if (machineId) params.append("machineId", machineId);

    const response = await fetch(`/api/timeentries?${params.toString()}`);
    const data = await response.json();

    const tbody = document.querySelector("#timeEntriesTable tbody");
    tbody.innerHTML = "";

    data.forEach(entry => {
        const row = document.createElement("tr");

        row.innerHTML = `
            <td>${entry.productName}</td>
            <td>${entry.machineName}</td>
            <td>${entry.status}</td>
            <td>${new Date(entry.startTime).toLocaleString()}</td>
            <td>${new Date(entry.endTime).toLocaleString()}</td>
        `;

        tbody.appendChild(row);
        
    });
    renderChart(data);
}



async function loadMachines() {

    const response = await fetch("/api/machines");
    const machines = await response.json();

    const select = document.getElementById("machineSelect");

    machines.forEach(m => {
        const option = document.createElement("option");
        option.value = m.id;
        option.textContent = m.name;
        select.appendChild(option);
    });
}

//chart.js Funktion


function renderChart(entries) {

    const totals = {
        setup: 0,
        production: 0,
        downtime: 0,
        rework: 0
    };

    entries.forEach(e => {
        const key = e.status.toString().trim().toLowerCase();

        if (!(key in totals)) return;

        const start = new Date(e.startTime);
        const end = new Date(e.endTime);
        const minutes = (end - start) / 60000;

        totals[key] += minutes;
    });

    const canvas = document.getElementById("timeChart");

    // ✅ Sicherheits-Guard
    if (!canvas) {
        console.warn("Canvas not found");
        return;
    }

    const ctx = canvas.getContext("2d");

    const labels = ["Setup", "Production", "Downtime", "Rework"];
    const data = [
        totals.setup,
        totals.production,
        totals.downtime,
        totals.rework
    ];

    
    if (chartInstance) {
        chartInstance.destroy();
        chartInstance = null;
    }

    chartInstance = new Chart(ctx, {
        type: "bar",
        data: {
            labels,
            datasets: [{
                label: "Minutes",
                data,
                backgroundColor: [
                    "#6c757d",
                    "#198754",
                    "#dc3545",
                    "#ffc107"
                ]
            }]
        },
        options: {
            responsive: true,
            plugins: {
                legend: { display: false }
            }
        }
    });
}

// Laden der Analyse Daten
async function loadAnalysis() {

    const startDate = document.getElementById("startDate").value;
    const endDate = document.getElementById("endDate").value;
    const machineId = document.getElementById("machineSelect").value;

    const params = new URLSearchParams();

    if (startDate) params.append("startDate", startDate);
    if (endDate) params.append("endDate", endDate);
    if (machineId) params.append("machineId", machineId);

    const response = await fetch(`/api/analysis?${params.toString()}`);
    const analysis = await response.json();

    renderAnalysisChart(analysis);
}



//Darstellen der Analyse Daten
function renderAnalysisChart(a) {

    document.getElementById("prodMinutes").innerText = a.productionMinutes;
    document.getElementById("downMinutes").innerText = a.downtimeMinutes;
    document.getElementById("downPercent").innerText =
        a.downtimePercentage.toFixed(1) + " %";

    const canvas = document.getElementById("analysisChart");
    const ctx = canvas.getContext("2d");

    const data = {
        labels: ["Time"],
        datasets: [
            {
                label: "Production",
                data: [a.productionMinutes],
                backgroundColor: "#198754"
            },
            {
                label: "Downtime",
                data: [a.downtimeMinutes],
                backgroundColor: "#dc3545"
            }
        ]
    };

    if (analysisChart) {
        analysisChart.destroy();
    }

    analysisChart = new Chart(ctx, {
        type: "bar",
        data,
        options: {
            responsive: true,
            scales: {
                x: { stacked: true },
                y: { stacked: true }
            }
        }
    });
}

