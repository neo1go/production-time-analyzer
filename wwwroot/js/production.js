// ==============================
// Chart.js Instances
// ==============================
let timeChartInstance = null;
let analysisChartInstance = null;

// ==============================
// Initialisierung
// ==============================
document.addEventListener("DOMContentLoaded", () => {
    loadMachines();

    document.getElementById("filterForm")
        .addEventListener("submit", e => {
            e.preventDefault();
            refreshDashboard();
        });

    const insightBtn = document.getElementById("runInsightBtn");
    if (insightBtn) {
        insightBtn.addEventListener("click", runProductionInsight);
    }
});

// ==============================
// Zentrale Aktualisierung
// ==============================
async function refreshDashboard() {
    await loadTimeEntries();
    await loadAnalysis();
}

// ==============================
// TimeEntries laden (Tabelle + Chart 1)
// ==============================
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

    renderTimeDistributionChart(data);
}

// ==============================
// Maschinen laden
// ==============================
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

// ==============================
// Chart 1: Zeitverteilung
// ==============================
function renderTimeDistributionChart(entries) {
    const totals = {
        setup: 0,
        production: 0,
        downtime: 0,
        rework: 0
    };

    entries.forEach(e => {
        const key = e.status.toLowerCase();
        if (!(key in totals)) return;

        const start = new Date(e.startTime);
        const end = new Date(e.endTime);
        const minutes = (end - start) / 60000;

        totals[key] += minutes;
    });

    const canvas = document.getElementById("timeChart");
    if (!canvas) return;

    if (timeChartInstance) {
        timeChartInstance.destroy();
    }

    timeChartInstance = new Chart(canvas, {
        type: "bar",
        data: {
            labels: ["Setup", "Production", "Downtime", "Rework"],
            datasets: [{
                label: "Minutes",
                data: [
                    totals.setup,
                    totals.production,
                    totals.downtime,
                    totals.rework
                ],
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
            plugins: { legend: { display: false } }
        }
    });
}

// ==============================
// Analyse laden (Chart 2 + Zahlen)
// ==============================
async function loadAnalysis() {
    const from = document.getElementById("startDate").value;
    const to = document.getElementById("endDate").value;
    const machineId = document.getElementById("machineSelect").value;

    if (!from || !to) return;

    const params = new URLSearchParams();
    params.append("from", from);
    params.append("to", to);

    if (machineId) {
        params.append("machineId", machineId);
    }

    const response = await fetch(`/api/analysis?${params.toString()}`);
    const analysis = await response.json();

    renderAnalysisChart(analysis);
}


// ==============================
// Chart 2: Production vs Downtime
// ==============================
function renderAnalysisChart(a) {
    document.getElementById("prodMinutes").innerText = a.productionMinutes;
    document.getElementById("downMinutes").innerText = a.downtimeMinutes;
    document.getElementById("downPercent").innerText =
        a.downtimePercentage.toFixed(1) + " %";

    const canvas = document.getElementById("analysisChart");

    if (analysisChartInstance) {
        analysisChartInstance.destroy();
    }

    analysisChartInstance = new Chart(canvas, {
        type: "bar",
        data: {
            labels: ["Production", "Downtime"],
            datasets: [{
                label: "Minutes",
                data: [a.productionMinutes, a.downtimeMinutes],
                backgroundColor: ["#198754", "#dc3545"]
            }]
        },
        options: {
            responsive: true,
            scales: {
                x: { stacked: false },
                y: { beginAtZero: true }
            }
        }
    });
}

// ==============================
// KI-Analyse
// ==============================
async function runProductionInsight() {
    const status = document.getElementById("insight-status");
    const content = document.getElementById("insight-content");

    const from = document.getElementById("startDate").value;
    const to = document.getElementById("endDate").value;
    const machineId = document.getElementById("machineSelect").value;

    if (!from || !to) {
        status.innerText = "Bitte Start- und Enddatum wählen.";
        return;
    }

    status.innerText = "KI-Analyse wird erzeugt …";
    content.innerText = "";

    try {
        let url = `/api/insights?from=${from}&to=${to}`;
        if (machineId) {
            url += `&machineId=${machineId}`;
        }

        const response = await fetch(url);
        const data = await response.json();

        status.innerText = "Analyse abgeschlossen:";
        content.innerText = data.text;
    }
    catch (err) {
        status.innerText = "Fehler bei der KI-Analyse.";
        content.innerText = err.message;
    }
}