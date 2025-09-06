# Copilot instructions for AjpWiki

Short, focused guidelines so an AI agent can be productive in this repo.

## Big picture
- Multi-project .NET solution (AjpWiki.sln) with layers:
  - `AjpWiki.Domain` — domain entities (pure POCOs, e.g. `WikiArticle`).
  - `AjpWiki.Application` — DTOs, mappings and service contracts (see `Dto/` and `Mappings/`).
  - `AjpWiki.Infrastructure` — persistence (EF DbContext `Data/WikiDbContext.cs`) and EF-backed implementations.
  - `AjpWiki.Web` — Blazor WebAssembly host, DI registration and runtime (`Program.cs`).

## Where to look first (key files)
- `AjpWiki.Domain/Entities/WikiArticle.cs` — article entity (Id, Title, Slug).
- `AjpWiki.Infrastructure/Data/WikiDbContext.cs` — EF model configuration, indexes and constraints.
- `AjpWiki.Application/Dto/WikiArticleDto.cs` and `AjpWiki.Application/Mappings/WikiArticleMappings.cs` — DTO shape and mapping conventions.
- `AjpWiki.Web/Program.cs` — DI bootstrapping; currently configures an InMemory EF DB for development.

## Project-specific conventions and patterns
- Keep Domain free of framework dependencies. Add behavior in Application layer.
- Mapping: extension methods live under `AjpWiki.Application/Mappings` and produce DTOs.
- Service contracts/interfaces belong in `AjpWiki.Application` (e.g. `IWikiArticleService`).
- EF/implementation classes belong in `AjpWiki.Infrastructure` (e.g. `WikiDbContext`, repository/service implementations).
- Place helpers that are framework-agnostic (e.g. `SlugHelper`) in `AjpWiki.Application/Utils` so they are testable.

- Constructor style: when services have no constructor logic, prefer C# primary constructors for brevity (e.g. `public class NotificationService(WikiDbContext _db) : INotificationService`).
  - This requires a recent C# LangVersion (the repo targets .NET 8 which supports it). If build issues appear on older toolchains, use the explicit constructor pattern.

## Routing / URL strategy (how articles map to URLs)
- The codebase uses a `Slug` property on `WikiArticle`. The expected pattern:
  - Generate canonical slug on create (use a `SlugHelper` in `Application`).
  - Persist `Slug` and enforce uniqueness (see `WikiDbContext` unique index on `Slug`).
  - Resolve articles by `slug` (or prefer `/{id}/{slug?}` to avoid slug collisions).

## Common tasks / commands
- Build entire solution:
  ```bash
  dotnet build AjpWiki.sln
  ```
- Run the Blazor WebAssembly app (from repo root):
  ```bash
  dotnet run --project AjpWiki.Web/AjpWiki.Web.csproj
  ```
- Add a NuGet package to a project (example):
  ```bash
  dotnet add AjpWiki.Infrastructure package Microsoft.EntityFrameworkCore.InMemory
  ```
- List packages for a project:
  ```bash
  dotnet list AjpWiki.Infrastructure package
  ```

## EF / InMemory notes (important for agents)
- `AjpWiki.Web/Program.cs` currently registers `WikiDbContext` with `UseInMemoryDatabase("WikiInMemory")` — this is intended for development/tests.
- EF InMemory differs from relational providers: uniqueness constraints and query translation can differ. If implementing logic that relies on DB behavior (unique constraint races, SQL-specific features), prefer adding a small integration test against SQLite or a relational provider.

## Integration points & future adapters
- The codebase is structured to allow swapping persistence implementations behind interfaces. Typical locations:
  - `AjpWiki.Application` for interfaces (e.g. `IWikiArticleService`).
  - `AjpWiki.Infrastructure` for EF implementations.
- When adding Cloudflare D1 or KV adapters: implement new repository/service in `AjpWiki.Infrastructure` and register via DI in the host. Do not place network-bound implementations in `AjpWiki.Application`.

## Testing & scaffolding hints (discoverable patterns)
- Unit-test helpers (e.g. `SlugHelper`) belong in `AjpWiki.Application` and should be pure functions.
- For repository/service integration tests, use an in-memory provider but add at least one CI/integration job that runs against SQLite to detect provider differences.

## Quick examples for agents
- Add a slug helper (file: `AjpWiki.Application/Utils/SlugHelper.cs`) — pure static method.`
- Add service interface `AjpWiki.Application/Services/IWikiArticleService.cs` and implement `AjpWiki.Infrastructure/Services/WikiArticleService.cs` that uses `WikiDbContext` and `SlugHelper`.
- Wire DI in `AjpWiki.Web/Program.cs`:
  ```csharp
  builder.Services.AddScoped<IWikiArticleService, WikiArticleService>();
  ```

## When editing files — practical rules for AI
- Preserve public APIs and namespaces. New files should follow the existing folder layout and namespaces (project root namespace is the project folder name).
- When changing EF model conventions in `WikiDbContext`, update mappings and DTOs if shapes change.
- Avoid adding secrets or external credentials into the repo. For Cloudflare or production DB, require the developer to supply tokens/config via environment or user secrets.

If anything above is unclear or you want me to merge / scaffold the `SlugHelper` + service interface + EF implementation now, say which pieces to create and I'll add them.
