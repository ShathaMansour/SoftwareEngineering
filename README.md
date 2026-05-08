# StarterApp

A cross-platform peer-to-peer item rental application built with .NET MAUI. 

---

## Project Overview

StarterApp allows users to:
- Register and authenticate via JWT
- List items for rent with location, category, daily rate, and photos
- Browse and search available items nearby using device GPS
- Request, approve, reject, and manage rentals
- Leave reviews on completed rentals
- Track rental status through a full state machine workflow

---

## Architecture Overview

```
StarterApp/                   # .NET MAUI cross-platform app (Android/iOS)
├── ViewModels/               # MVVM ViewModels (ObservableObject, commands)
├── Views/                    # XAML pages bound to ViewModels
└── Services/                 # Location, authentication, API services

StarterApp.Database/          # Shared database library
├── Data/                     # Repository implementations (EF Core)
├── Models/                   # Entity models
├── States/                   # Rental state machine (State Pattern)
└── AppDbContext.cs

StarterApp.Migrations/        # EF Core migrations for PostgreSQL
StarterApp.Tests/             # xUnit test project
```

The app follows **MVVM** architecture with a **Repository pattern** for data access and a **Service layer** for business logic. A **State Pattern** enforces valid rental workflow transitions.

---



## Prerequisites

Before using this app, ensure you have:

1. **.NET 10 SDK** installed
2. **Docker** installed and running
3. **MAUI workload** installed:
   ```bash
   dotnet workload install maui-android
   ```
4. **PostgreSQL container** running (see [dev-environment tutorial](https://edinburgh-napier.github.io/SET09102/tutorials/csharp/dev-environment/))

You can use any of the following development environments:
- [Rider](https://www.jetbrains.com/rider/)
- [Visual Studio](https://visualstudio.microsoft.com/)
- [Visual Studio Code](https://code.visualstudio.com/)

---

## Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/ShathaMansour/SoftwareEngineering.git
cd SoftwareEngineering
```

### 2. Start the PostgreSQL database

```bash
docker-compose up -d
```

This starts a PostgreSQL container on port 5432 with the credentials defined in docker-compose.yml.

### 3. Configure the database connection

Copy the settings template and update with your PostgreSQL credentials:

```bash
cp StarterApp.Database/appsettings.json.template StarterApp.Database/appsettings.json
```

Then edit appsettings.json:

```json
{
  "ConnectionStrings": {
    "DevelopmentConnection": "Host=localhost;Username=student_user;Password=password123;Database=starterapp"
  }
}
```

### 4. Apply database migrations

```bash
cd StarterApp.Migrations
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 5. Restore dependencies

```bash
dotnet restore StarterApp.sln
```

### 6. Configure the API URL

Update the API base URL in StarterApp/Services/ApiService.cs if running against a local backend:

```csharp
private const string BaseUrl = "http://your-api-url";
```

---

## Running the Application

```bash
cd StarterApp
dotnet build
dotnet run
```

For Android specifically:

```bash
dotnet build StarterApp/StarterApp.csproj -f net10.0-android
dotnet run --project StarterApp/StarterApp.csproj -f net10.0-android
```

Or open the solution in Visual Studio / Rider and run directly on your target device or emulator.

---

## Running Tests

### Run all tests

```bash
dotnet test StarterApp.Tests/StarterApp.Tests.csproj --verbosity normal
```

### Run with coverage

```bash
dotnet test StarterApp.Tests/StarterApp.Tests.csproj --collect:"XPlat Code Coverage" --results-directory ./coverage
```

### Generate HTML coverage report

```bash
dotnet tool install -g dotnet-reportgenerator-globaltool
reportgenerator -reports:"./coverage/**/coverage.cobertura.xml" -targetdir:"./coverage-report" -reporttypes:"Html" -classfilters:"-StarterApp.Database.Migrations.*"
```

Open ./coverage-report/index.html in your browser.

### Test summary

| Test Class | Namespace | Tests |
|---|---|---|
| RentalStateMachineTests | Tests/States | 24 |
| ModelTests | Tests/Repositories | 13 |
| ItemsListViewModelTests | Tests/ViewModels | 11 |
| LocationServiceTests | Tests/Services | 9 |
| ReviewRepositoryTests | Tests/Repositories | 5 |
| UserRoleTests | Tests/Repositories | 5 |
| CategoryRepositoryTests | Tests/Repositories | 4 |
| ItemRepositoryTests | Tests/Repositories | 4 |
| RentalServiceTests | Tests/Services | 4 |
| RoleConstantsTests | Tests/Repositories | 4 |
| **Total** | | **83** |

Current line coverage: **66.6%** exceeds the 60% requirement.

---

## API Endpoint Documentation

The app communicates with a REST API. Documentation link here: https://moodle.napier.ac.uk/mod/page/view.php?id=2924259

---


## Docker

```bash
docker-compose up -d      # start database
docker-compose down       # stop database
docker-compose down -v    # stop and remove all data
```

---

