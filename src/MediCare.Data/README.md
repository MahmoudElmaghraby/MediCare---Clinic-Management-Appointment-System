# MediCare.Data

EF Core data access for SQL Server. `MediCare.Services` references this project. `MediCare.Web` does not.

`ApplicationDbContext`, `BaseEntity`, and the specialization sample are the pattern to copy. ASP.NET Identity is referenced and not implemented yet.

- `Entities/`: table classes. `BaseEntity` and `Specialization` are the sample.
- `Configurations/`: EF Core `IEntityTypeConfiguration` classes.
- `Repositories/`: `Repository<T>` and specific repositories.
- `Interfaces/`: `IRepository<T>`, specific repository contracts, and `IUnitOfWork`.
- `UnitOfWork/`: `UnitOfWork`, which saves changes once.
- `Migrations/`: EF Core migrations. The first one is `InitialCreate`.
- `DependencyInjection.cs`: `AddMediCareData`, called from `MediCare.Services`.
