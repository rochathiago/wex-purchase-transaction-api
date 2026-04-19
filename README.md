# PurchaseTransactionAPI

Summary
- PurchaseTransactionAPI is a small, layered .NET 10 web API that models purchase-related domain entities and exposes endpoints to work with transactions and exchange rates.
- Key technologies: .NET 10, ASP.NET Core Web API, Entity Framework Core (Sqlite / InMemory), FluentValidation, Swashbuckle (Swagger), xUnit/Moq for tests.

Business rules (domain highlights)
- `Currency` (domain entity)
  - Properties: `Code` (ISO code) and `Description`.
  - Validation rules enforced by the constructor:
    - `Code` is required and stored uppercased (`Code = code.ToUpper()`).
    - `Description` is required.
    - Invalid inputs throw `ArgumentException` with messages `"Code is required"` or `"Description is required"`.
- Transactions and exchange rates (high level)
  - Transactions reference domain entities (e.g., currency) and must respect domain validations.
  - Exchange rate retrieval is handled by an external client component and is used to convert amounts between currencies. (See `Tests\...ExchangeRateClientTests.cs` for client tests in the test project.)

Solution architecture
- Layered structure (typical separation of concerns):
  - `Domain` — business entities and domain logic (e.g., `Domain\Entities\Currency.cs`).
  - `API` — ASP.NET Core Web API controllers (e.g., `API\Controllers\TransactionsController.cs`).
  - `Infrastructure` — data access and external service clients (Entity Framework DbContext, external exchange rate client).
  - `Tests` — unit and integration tests (xUnit, Moq).
- Patterns and packages used:
  - Dependency Injection via ASP.NET Core DI container.
  - Persistence via `Microsoft.EntityFrameworkCore` (Sqlite for local, InMemory for tests).
  - Input validation via `FluentValidation`.
  - API documentation via `Swashbuckle.AspNetCore` (Swagger UI).
  - Tests: `xunit`, `Moq`, and `FluentAssertions` for expressive assertions.

Project layout (important files/folders)
- `Domain\Entities\Currency.cs` — currency entity and constructor validations.
- `API\Controllers\TransactionsController.cs` — controller entry points for transaction operations.
- `Tests\PurchaseTransaction.UnitTests\` — unit tests for domain and infrastructure components.
- `PurchaseTransactionAPI.csproj` — references target framework and packages (targets `net10.0`).

Prerequisites
- .NET 10 SDK installed.
- Visual Studio 2022/2026 or VS Code (recommended: Visual Studio Community 2026 for best integrated experience).
- (Optional) SQLite installed for local DB testing if using file-based persistence.

How to run locally (CLI)
- Restore, build, run API, and run tests from the workspace root:

How to run in Visual Studio
- Open the solution in Visual Studio.
- Start the API with __F5__ or use __Run__ from the toolbar.
- Use the Visual Studio __Test Explorer__ to run / debug unit and integration tests.
- API documentation (Swagger) is available at `http://localhost:{port}/swagger` when the app is running (Swashbuckle configured).

Testing notes
- Unit tests are implemented with `xUnit` and `Moq`. Look for test classes under `Tests\PurchaseTransaction.UnitTests`.
- Example unit test scope:
  - Domain constructors and validations (e.g., `Currency` constructor throws for invalid inputs).
  - Controller behavior using mocked dependencies.
  - Exchange rate client integration tests use either a test double or test server fixture.

Developer tips
- Follow the domain validations in `Domain\Entities` when creating or mapping DTOs.
- Preserve the uppercasing behavior of currency codes when reading/writing to stores or APIs.
- Use `Microsoft.EntityFrameworkCore.InMemory` for fast unit/integration tests; use `Sqlite` for lightweight local integration tests that exercise EF Core behavior.

Contributing
- Create a feature branch prefixed with your ticket/feature name.
- Add unit tests for new domain rules and controller behaviors.
- Run `dotnet test` before opening a pull request.

License and attribution
- Project license and contribution guidelines should be added to `LICENSE` and `CONTRIBUTING.md` at the repo root if required.

Contact / Further info
- For questions about specific domain rules or architecture choices, inspect the domain entity files in `Domain\Entities` and existing controller/tests under `API\Controllers` and `Tests\`.