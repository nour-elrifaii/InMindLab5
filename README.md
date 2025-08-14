# Hi Ramy!

This task makes the necessary changes to switch to a clean Domain-Driven Design structure.

## Layer Breakdown

### API Layer
- StudentsController exposes endpoints.
- Program.cs wires DI, middleware pipeline, and Swagger.

### Application Layer
- Mappers convert between domain models and view models/DTOs.
- Validators enforce input rules before reaching domain logic.
- ViewModels/DTOs define the API’s input/output contracts decoupled from domain entities.

### Domain Layer
- Models represent the core business concepts. No framework dependencies.

### Infrastructure Layer
- Logging Middleware is handled and plugged into the API pipeline via Program.cs.

### Persistence Layer
- DbContext manages database access with EF Core.
- Changes flow from domain/persistence configuration into the database.

## Differences
- Controllers no longer contain business logic, they call application services/mappers/validators.
- Domain entities live independently in the Domain layer.
- Infrastructure and Persistence concerns (middleware, DbContext, migrations) are isolated from business logic.
- Mapping and validation formalize the boundary between external requests and the domain.


## Conclusion
The project now follows DDD principles with clear boundaries and easier testing.
