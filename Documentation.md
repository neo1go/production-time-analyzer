# Production Time Analyzer

## Technologies Used (Current State)

### Backend
- **ASP.NET Core MVC** – Web application framework
- **C#** – Primary programming language

### Frontend
- **Razor Views** – Server-side rendered UI
- **JavaScript** – Asynchronous data loading via Fetch API
- **Chart.js (v4.5.1)** – Data visualization (production time distribution and analysis charts)

### Data Access
- **Entity Framework Core** – Object–relational mapper (ORM)

### Database
- **SQL Server (LocalDB)** – Development database

### Authentication & Security
- **ASP.NET Core Identity** – User authentication and authorization
- **HTTPS** – Development certificate provided by ASP.NET Core

### Tooling & Development
- **Visual Studio**
- **.NET CLI**
- **Git**
- **GitHub**
- **Swagger (Swashbuckle)** – API inspection and testing

---

## AI Integration

- **Local LLM inference using LM Studio**
- **Model:** `qwen2.5-vl-3b-instruct` (~3.27 GB)
- **Communication:** HTTP API
- **Purpose of AI usage:**
  - Contextual explanation of aggregated production metrics
  - Highlighting of anomalies and unusual patterns
  - Qualitative interpretation of calculated KPIs
- **Important:**  
  The AI is **read-only** and **does not perform any calculations**.  
  All metrics are deterministically calculated in the backend before being passed to the AI.

---

## Development & Setup Notes

### Entity Framework Core

The following NuGet packages were added step by step:

- `Microsoft.EntityFrameworkCore`
- `Microsoft.EntityFrameworkCore.Design`
- `Microsoft.EntityFrameworkCore.Tools`
- `Microsoft.Data.SqlClient`

Entity Framework CLI installation:

bash
dotnet tool install --global dotnet-ef


### Initial migration and database setup:
dotnet ef migrations add InitialCreate
dotnet ef database update

### Identity Setup:
ASP.NET Core Identity scaffolding added via Visual Studio
(Rechtsklick → Neues Gerüstelement)
Generated Identity pages:

Account/Login
Account/Register
Account/Logout


Identity database integration:

# Initial migration in pm:
Add-Migration AddIdentityTables
# Database update in pm:
Update-Database

### Important
User registration is available at:
https://localhost:7294/Identity/Account/Register
or via click on 'Register account'
(The application must be running)

### DbContext Refactoring

AppDbContext was renamed to ProductionTimeAnalyzerContext
Multiple migrations and rebuilds were required to ensure a clean and consistent database state after Identity integration


### Chart.js Integration

chart.umd.js was downloaded from the official Chart.js GitHub release page:
https://github.com/chartjs/Chart.js/releases/tag/v4.5.1
The file was imported manually via the .tgz archive and placed into a local chart directory
Additional Chart.js resources are loaded via the browser where required


### Application Overview

Main dashboard route:
/Production/Overview

## Features:

Time-based filtering (date range)
Machine-specific filtering
Tabular display of time entries
Production time distribution chart
Production vs. downtime analysis chart
Optional AI-generated insights based on aggregated metrics




## Notes on AI Behavior

The AI is explicitly instructed:

Not to infer or guess time periods
Not to calculate metrics
Not to reinterpret downtime as an error condition


UnclassifiedMinutes are treated as a diagnostic indicator, not a primary KPI, and are only mentioned when relevant


## Version Control

Git is used for version control
GitHub hosts the repository
Local configuration files (e.g. appsettings.Development.json) are excluded via .gitignore


### Status
The project is under active development and focuses on:

Clean separation of concerns
Deterministic backend calculations
Transparent AI-assisted interpretation
Maintainable MVC architecture