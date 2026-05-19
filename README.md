# Production Time Analyzer

## Overview
**Production Time Insight** is a web-based ASP.NET Core application for capturing, visualizing, 
and analyzing production times in an industrial context.

The system focuses on manufacturing environments, where products are processed on machines and time segments such as setup time, production time, downtime, and rework are recorded and analyzed.

The goal is not employee time tracking, but transparent production time analysis.

---

## Features
- Capture and manage production time entries
- Filter by time  and machine
- Display data in tables and charts
- Analyze time segments (setup, production, downtime, rework)
- Interactive frontend using JavaScript (fetch API)
- Optional analysis service to explain anomalies
- AI Agent for analysis purpose only
---

## Technology Stack
- **ASP.NET Core MVC**
- **Entity Framework Core**
- **SQL Server (LocalDB)**
- **Razor Views (HTML)**
- **JavaScript (fetch, Chart.js)**

---

## Architecture

The application follows a layered architecture within a single ASP.NET Core project:


### Key Principles
- Clear separation of concerns
- Thin controllers
- Business logic inside services
- Monolithic application (no microservices)
- No unnecessary frontend frameworks (no Angular/React)

---

## Domain Model

### Product
- Name
- Order number

### Machine
- Name
- Type (e.g. CNC, lathe)

### TimeEntry
- Product
- Machine
- Start time
- End time
- Status (setup, production, downtime, rework)

---

## User Flow

1. The user logs in using standard ASP.NET authentication
2. The user opens the production overview page
3. The user selects:
   - Start date
   - End date
   - Optional: machine
4. The user clicks **"Show"**
5. The system displays:
   - A table of time entries
   - A graphical representation (charts)
6. The user can:
   - Select entries
   - Request detailed analysis

---

## Analysis Service

The application includes a simple analysis component:

- Implemented as a backend service
- Receives pre-processed data from the system
- Generates textual explanations of production data

Example use case:
> Explain unusual downtime in the selected period.

The analysis component is:
- simple and local
- replaceable
- not a full AI system

---

## Data Storage

- SQL Server (LocalDB)
- Managed using Entity Framework Core
- Real database (no mocks or simulations)

---

## Goals of this Project

This project is not intended to be a full production system.

Instead, it demonstrates:

- Understanding of real-world business domains
- Clean web architecture design
- Proper separation of concerns
- Practical use of modern technologies without overengineering

---

## Documentation

For a more detailed technical and architectural description,
see [Documentation.md](Documentation.md).

---

## Author

This project was developed as part of a learning and portfolio effort to demonstrate 
backend and web application architecture using ASP.NET Core.