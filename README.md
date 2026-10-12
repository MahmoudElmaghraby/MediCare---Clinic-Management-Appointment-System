# MediCare

Clinic management and appointment system. DEPI graduation project for a six-person team. Most of the team is new to .NET, so this repository starts as a small skeleton: a buildable ASP.NET Core MVC app, empty class libraries, and placeholder folders. Entities, repositories, and ASP.NET Identity are left for Jira tasks.

## Stack

- ASP.NET Core MVC on .NET 8 (LTS)
- Entity Framework Core with SQL Server
- ASP.NET Identity roles planned: Patient, Doctor, Admin
- Later: SignalR, Twilio, SendGrid, FullCalendar.js, Chart.js, Azure
- UI: Bootstrap 5 (included with the MVC template)

## Architecture

N-tier. The web project depends on services, and services depend on data. A working sample (specialization) shows the base entity, repository, unit of work, factory, service, and dependency injection. Copy that sample for the next feature. The Arabic team guide is [docs/project-guide-ar.md](docs/project-guide-ar.md).

```text
Browser (Razor, Bootstrap 5) calls MediCare.Web
MediCare.Web calls MediCare.Services
MediCare.Services calls MediCare.Data
MediCare.Data calls SQL Server
```

FullCalendar.js, Chart.js, SignalR, Twilio, SendGrid, and Azure are planned around the web app and are not implemented yet.

## Folder structure

```text
MediCare.sln
src/
  MediCare.Web/          MVC site (controllers, views, wwwroot)
    ViewModels/          view models for new screens
    Hubs/                SignalR placeholder
  MediCare.Services/     business logic
    Interfaces/
    Implementations/
    Factories/
  MediCare.Data/         EF Core, repositories, unit of work
    Entities/
    Repositories/
    Interfaces/
    UnitOfWork/
    Configurations/
    Migrations/
tests/
  MediCare.Tests/        xUnit
docs/                    architecture, ERD, user manual, setup guide
.github/
  workflows/ci.yml
  pull_request_template.md
```

Project references: `MediCare.Web` → `MediCare.Services` → `MediCare.Data`. The test project references all three. `MediCare.Data` references EF Core SQL Server, EF Core Tools, and ASP.NET Identity Entity Framework Core 8.0.31. `ApplicationDbContext` and the specialization sample are in place. Identity roles are not wired up yet.

## Run locally

Install the [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0). `global.json` asks for the 8.0 feature band.

```bash
dotnet restore MediCare.sln
dotnet build MediCare.sln
dotnet test MediCare.sln
dotnet run --project src/MediCare.Web --launch-profile http
```

Open `http://localhost:5067`. The HTTPS profile is in `src/MediCare.Web/Properties/launchSettings.json`. The home page does not query SQL Server. A local connection string without a password is in `appsettings.json`. Apply the migration when the database is installed:

```bash
dotnet ef database update --project src/MediCare.Data --startup-project src/MediCare.Data
```

Keep real passwords in [user secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets), not in Git.

## Team

Mahmoud, Ahmed, Ehab, Peter, Salem, Hady.

How we branch and open pull requests is in [CONTRIBUTING.md](CONTRIBUTING.md). Documents go in [docs/](docs/README.md). Beginners should start with [the Arabic project guide](docs/project-guide-ar.md).
