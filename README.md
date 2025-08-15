# Hi Ramy!

In this Lab, I implemented logging, health checks, localization, caching and background jobs.

## Serilog
- Replaced the default logger with Serilog at the host level, so every `ILogger<>` call becomes a structured event.
- Inserted a custom RequestLoggingMiddleware early in the pipeline. It safely buffers the body, logs method/path/query and all headers, stamps a UTC timestamp, times the request, and records the final status (and any unhandled exceptions). Because the host uses Serilog, these entries flow straight into the structured sinks without changing the middleware’s ILogger<> code.
- Registered the middleware in `Program.cs` so every request/response gets captured.

## Health Checks 
- In `Program.cs`, health checks are registered for PostgreSQL connectivity and a domain-level probe (StudentCheck). Results are exposed as JSON at /health and via HealthChecks UI.
- It implements IHealthCheck and performs a simple query against Students to verify real data access. It returns Healthy when the query succeeds (students reachable) and Unhealthy otherwise, keeping diagnostics focused and independent of controller logic.

## Localization
- Localization is enabled with a resources folder and request-based culture switching. In Program.cs, the app registers localization, supports en and fr, sets fr as default, and inserts a `QueryStringRequestCultureProvider` so ?culture=en|fr switches UI strings.
- A markre class `SharedResource` anchors the .resx files for each culture.
- StudentsController injects IStringLocalizer<SharedResource> and returns a localized string in the Tesssst endpoint (_localizer["students found"]). Changing ?culture= switches the response language. 

## Caching
**MediatR**: 
- Registered repositories for Student and Teacher. 
- Enabled StackExchange.Redis as IDistributedCache, using the Redis connection string from appsettings.json. 

**Queries & Handlers**
- Query: `GetAllStudentsQuery : IRequest<List<StudentDto>>`
- Hadler: checks Redis key `all_students`, if present, returns cached JSON: List<StudentDto). On miss, loads from IRepository<Student>, maps with AutoMapper, caches for 10 minutes, then returns. 

- Query: `GetAllTeachersQuery : IRequest<List<TeacherDto>>`
- Handler: identical pattern with Redis key teachers_all.

**Mapping**: 
- AutoMapper profile maps Entities and DTO/ViewModels. 
- Repositories prvide GetAllAsync() (and basic add/delete), keeping EF Core behind an abstraction.

This way, the caching logic lives in handlers, so controllers remain slim and the read path is fast and consistent.

## Background Job
- Hangfire is configured with in-memory storage, the server is started, and the dashboard is exposed at `/hangfire`. The DI container registers `EmailService: IEmailService` and `BackgroundJobService: IBackgroundJobService`.
- When a student is added, the controller immediately enqueues a background job.
- `BackgroundJobService` composes a welcome subject/body, logs intent, and delegates the actual send to `IEmailService.SendEmailAsync(...)`, then logs completion.
- `EmailService` implements the send contract and simulates delivery by writing the email to the console (the service does not actually send an email, it was done for the purpose of showing how background jobs would work).

## Conclusion
Logging gives clear visibility, haelth checks surface issues early, localization adapts responses per culture, caching speeds up repeat reads, and background jobs offload slow work. The result is a more reliable, faster, and cleaner system.
