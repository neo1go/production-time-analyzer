## Technologies Used (Current State)

### Backend
- ASP.NET Core MVC (Web Application Framework)
- C# (Programming Language)

### Frontend
- Razor Views (Server-side rendering)
- JavaScript (fetch API for asynchronous requests)

### Data Access
- Entity Framework Core (ORM)

### Database
- SQL Server (LocalDB for development)

### Development & Version Control
- Visual Studio
- Git
- GitHub

### Security
- HTTPS (development certificate via ASP.NET)

### added (step by step)
- Microsoft.EntityFrameworkCore mittels nuget hinzugefügt
- Microsoft.EntityFrameworkCore.Design mittels nuget hinzugefügt
- Microsoft.EntityFrameworkCore.Tools mittels nuget hinzugefügt
- Microsoft.SqlServer hinzugefügt mittels nuget hinzugefügt

- im nuget Packetmanager Konsole mittels "Add-Migration InitialCreate" die Migration erstellen für die DB
- ebenfalls in dieser Konsole wird dann die DB von Entity Framework mittels "Update-Database" erstellt.

- 'chart.umd.js' wurde von der github Seite "https://github.com/chartjs/Chart.js/releases/tag/v4.5.1" mittels tgz importiert und dann in den chart-Ordner eingefügt.
- Der Rest von chart.js wird seperat vom Browser geladen. 


