# Copilot Context for FrostByte Workbench

This file provides key architectural and build context for GitHub Copilot to generate code aligned with the FrostByte IDE project.

---

## Project Vision

- **FrostByte** is a desktop IDE for "Advent‑style" daily coding puzzles, integrating puzzle fetching, code editing, execution, and version control under one roof.
- It targets **Windows** first, then **Linux/macOS**, using a layered clean architecture and a plugin model for language support.

## Architecture Layers

1. **Presentation** (`FrostByte.Presentation`) – .NET MAUI pages & view‑models (CalendarPage, DayPage), using Markup‑Fluent syntax, with Monaco (editor) and xterm.js (terminal) in a WebView.
2. **Application** (`FrostByte.Application`) – Orchestrators and services:
   - `RunnerService` compiles and runs code via `ILanguagePlugin` and `IProcessSandbox`.
   - `AuthService` manages AoC session cookie.
   - `CalendarService` and `StatsService` expose puzzles and performance data.
   - `NotificationService` for snackbars/toasts.
3. **Domain** (`FrostByte.Domain`) – Entities and value objects (Puzzle, RunResult, DayOfAdvent), AST definitions for puzzle content.
4. **Infrastructure** (`FrostByte.Infrastructure`) – Implementations:
   - `AoCClient` downloads puzzle HTML and inputs; transforms HTML→JSON AST with AngleSharp.
   - Repositories: JSON file storage and SQLite (`SqliteStatsRepo`).
   - `GitWorkspaceService` wraps LibGit2Sharp and Octokit for in‑app Git.
   - `ProcessSandbox`: Local (JobObject on Windows) and optional Docker runner.
5. **Plugins** (`FrostByte.Plugins.Abstractions`) – `ILanguagePlugin` interface with `CompileAsync`, `ExecuteAsync`, `GetDiagnosticsAsync`.
   - First‑party plugin: `FrostByte.LanguagePlugins.TypeScript` using esbuild + Node.

## Core Interfaces

```csharp
public interface ILanguagePlugin {
  string Id { get; }
  Version Version { get; }
  Task<CompilationResult> CompileAsync(RunRequest req, CancellationToken ct);
  Task<ExecutionResult> ExecuteAsync(RunRequest req, CancellationToken ct, IStreamHandler streams);
  Task<IReadOnlyList<Diagnostic>> GetDiagnosticsAsync(SourceFile file, CancellationToken ct);
}

public interface IProcessSandbox {
  Task<ExecutionResult> ExecuteAsync(SandboxedProcessSpec spec, CancellationToken ct, IStreamHandler streamHandler);
}

public interface INotificationService {
  Task ShowToastAsync(string message, int durationSeconds = 2);
  Task ShowSnackbarAsync(string message, string actionText = null, Func<Task> action = null);
}
```

## Dependency Injection

- Extension methods in `FrostByte.App` bootstrap DI:
  ```csharp
  builder.Services
         .AddFrostByteCore(config)
         .AddFrostByteInfrastructure(config)
         .AddFrostBytePresentation()
         .AddFrostBytePlugins();
  ```
- Services are registered with lifetimes: Singleton for `SettingsService`, `PluginHost`, Transient for `RunnerService`, ViewModels, Scoped for page scopes.

## Build & Project Layout

```
FrostByte.sln
└─ src/
   ├─ FrostByte.App/             # MAUI host, Windows-only (net9.0-windows)
   ├─ FrostByte.Presentation/     # UI layer, net9.0 class library
   ├─ FrostByte.Application/      # app services, net9.0
   ├─ FrostByte.Domain/           # core models, net9.0
   └─ FrostByte.Infrastructure/   # HTTP, storage, git, sandbox, net9.0
```

### Prerequisites

- **.NET 9 Preview SDK** & **MAUI workload** installed:
  ```bash
  ```

dotnet workload install maui

````
- **JetBrains Rider** 2024.1+ or Visual Studio 2022 with MAUI support.

### Build & Run
1. **Restore & build**:
 ```bash
dotnet restore
dotnet build FrostByte.sln
````

2. **Run MAUI app** (Windows target):
   ```bash
   ```

dotnet run --project src/FrostByte.App/FrostByte.App.csproj -f net9.0-windows10.0.19041

```
3. **Hot Reload** in Rider/VS: enable .NET Hot Reload to see XAML and C# updates without restart.

---

*Keep this context file up-to-date as architecture and dependencies evolve.*

```
