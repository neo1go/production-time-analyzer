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

- dotnet tool install --global dotnet-ef
- dotnet ef migrations add InitialCreate


- 'chart.umd.js' wurde von der github Seite "https://github.com/chartjs/Chart.js/releases/tag/v4.5.1" mittels tgz importiert und dann in den chart-Ordner eingefügt.
- Der Rest von chart.js wird seperat vom Browser geladen. 

- Mittels Rechtsklick auf das Projekt und dann "Neues Gerüstelement" identity hinzugefügt 
  Account\Login\  hinzugefügt
  Account\Register\  hinzugefügt
  Account\Logout\  hinzugefügt
  im nuget Packetmanager Konsole 'Add-Migration AddIdentityTables'
  und anschließend 'Update-Database' durchgeführt

### DBContext wegen login erneuern
- AppDbContext wurde zu ProductionTimeAnalyzerContext
  viel neue Migrations und rebuilds, damit die DB wieder sauber läuft

###  Login-Seite erstellen (Terminal)
- dotnet tool install -g dotnet-aspnet-codegenerator
  dotnet aspnet-codegenerator identity -dc ProductionTimeAnalyzerContext
- ACHTUNG, erst hiermit registrieren:  https://localhost:7294/Identity/Account/Register
  , während die App läuft.


### Erstellen des KI-Agenten
- Swashbuckle sowie Swashbuckle Swagger installiert, um die json-Ausgabe des Agenten zu überprüfen
- KI Agent eingebunden: qwen 2.5 -vl-3b-instruct mit der Größe von 3.27 GB
- läuft in LMStudio