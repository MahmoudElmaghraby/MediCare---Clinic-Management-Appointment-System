# MediCare.Data

EF Core data access (SQL Server). `MediCare.Services` references this project. `MediCare.Web` does not.

NuGet packages are referenced so later tasks can use them. There is no `DbContext`, no entities, no repositories, and no ASP.NET Identity code yet. Add those in the Jira task that owns them.

| Folder | What goes here |
| --- | --- |
| `Entities/` | Domain entity classes |
| `Repositories/` | Repository implementations |
| `Interfaces/` | Repository and unit-of-work contracts |
| `UnitOfWork/` | Unit of Work |
| `Configurations/` | EF Core `IEntityTypeConfiguration` classes |
| `Migrations/` | EF Core migrations |
