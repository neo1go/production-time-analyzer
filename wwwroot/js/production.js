document.addEventListener("DOMContentLoaded", () => {
    loadTimeEntries();

    document.getElementById("filterForm")
        .addEventListener("submit", e => {
            e.preventDefault();
            loadTimeEntries();
        });
});

async function loadTimeEntries() {
    const response = await fetch("/api/timeentries");
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
}

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

    const params = new URLSearchParams();

    if (startDate) params.append("startDate", startDate);
    if (endDate) params.append("endDate", endDate);

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
}