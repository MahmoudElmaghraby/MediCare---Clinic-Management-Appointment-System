# MediCare.Services

Business logic. This project references `MediCare.Data` and is referenced by `MediCare.Web`.

- `Interfaces/`: service contracts. `ISpecializationService` is the sample.
- `Implementations/`: service classes. They use `IUnitOfWork`, not `DbContext`.
- `Factories/`: classes that build valid entities. `SpecializationFactory` is the sample.
- `DependencyInjection.cs`: `AddMediCareServices`, called from `Program.cs`.

`Program.cs` registers this project. It does not reference `MediCare.Data`.
