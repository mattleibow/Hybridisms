# Hybridisms

## Overview
Hybridisms is a demonstration solution that showcases how to build modern, distributed, and resilient hybrid applications using .NET 9, Blazor, .NET MAUI, WebAssembly, ONNX, and cloud-native patterns. The solution is designed to run seamlessly across web, native, and cloud environments, leveraging shared code, hybrid service fallback, and advanced orchestration.

---

## Project Structure and Responsibilities

### 1. Hybridisms.AppHost
Orchestrates the entire distributed application using Aspire. Provisions resources (Azure OpenAI, SQLite), manages service discovery, and launches all hybrid app components (web, native, wasm).

### 2. Hybridisms.Server.WebApp
ASP.NET Core Web API and Blazor Server app. Hosts REST APIs and interactive Blazor components for notes, topics, notebooks, and AI features. Integrates with a local SQLite database and Azure OpenAI for backend intelligence.

### 3. Hybridisms.Client.WebAssembly
Blazor WebAssembly (WASM) client. Runs in the browser, connects to backend APIs for data and AI, and uses shared contracts for seamless hybrid integration.

### 4. Hybridisms.Client.NativeApp
.NET MAUI Blazor Hybrid app for mobile and desktop. Combines native device features with Blazor UI, supports both local (ONNX) and remote (cloud) AI/data services, and synchronizes with backend APIs.

### 5. Hybridisms.Server
Backend library for data access and advanced AI. Implements EF Core models, DbContext, and server-side intelligence services for hybrid scenarios.

### 6. Hybridisms.Shared
Defines shared models, interfaces, and remote service implementations. Enables code reuse and consistent contracts across all hybrid clients and servers.

### 7. Hybridisms.Hosting
Shared Blazor component library for layouts, navigation, and UI elements. Ensures a consistent look and feel across all hybrid app frontends.

### 8. Hybridisms.Server.Worker
.NET Worker Service that migrates and seeds the database on startup, ensuring the backend is ready for all hybrid clients.

### 9. Hybridisms.Client.Native
Provides native data and AI service implementations for on-device storage and intelligence, including ONNX-powered AI and hybrid fallback logic.

### 10. ServiceDefaults Projects
These projects provide service discovery, HTTP resilience, and telemetry defaults for their respective environments, enabling robust hybrid communication and monitoring.

- **Hybridisms.Server.ServiceDefaults**
- **Hybridisms.Client.Native.ServiceDefaults**
- **Hybridisms.Client.WebAssembly.ServiceDefaults**


### 11. Stub Projects
Used for Aspire orchestration and service discovery, allowing the native and WASM clients to be managed as part of the distributed hybrid app.

- **stubs/Hybridisms.Client.NativeApp.ClientStub**
- **stubs/Hybridisms.Client.WebAssembly.ClientStub**


---

## How Hybridisms Enables Hybrid Scenarios

### 1. Shared Contracts and Code Reuse
All clients and servers reference `Hybridisms.Shared`, ensuring a single source of truth for models and interfaces. This enables seamless data flow and business logic reuse across web, native, and cloud.

```csharp
public interface INotesService
{
    Task<ICollection<Notebook>> GetNotebooksAsync(CancellationToken cancellationToken = default);
    // ...
}
```

### 2. Hybrid Service Fallback (Local/Remote)
Native clients (MAUI) use hybrid services that automatically switch between remote (cloud) and local (on-device) implementations for resilience and offline support.

```csharp
public class HybridIntelligenceService : IIntelligenceService
{
    public Task<string> GenerateNoteContentsAsync(string prompt, CancellationToken cancellationToken = default)
    {
        // Try remote, fallback to local
        return WithLocalFallback(
            ct => remote.GenerateNoteContentsAsync(prompt, ct),
            ct => local.GenerateNoteContentsAsync(prompt, ct),
            cancellationToken);
    }
}
```

### 3. Hybrid Rendering (Blazor Server + WASM)
The server registers both server and WebAssembly render modes for Blazor components, allowing the same UI to run in the browser or on the server.

```csharp
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode();
```

### 4. On-Device AI with ONNX
Native clients bundle ONNX models for local AI (embeddings, chat, recommendations), enabling hybrid intelligence even when offline.

```csharp
builder.Services.AddOptions<OnnxEmbeddingClient.EmbeddingClientOptions>()
    .Configure(options => {
        options.BundledPath = "Models/miniml_model.zip";
        options.ExtractedPath = Path.Combine(FileSystem.AppDataDirectory, "Models", "embedding_model");
    });
```

### 5. Service Discovery and Resilience
ServiceDefaults projects configure dynamic endpoint discovery, HTTP resilience, and OpenTelemetry for robust hybrid communication and monitoring.

```xml
<PackageReference Include="Microsoft.Extensions.ServiceDiscovery" />
```

### 6. Distributed Orchestration with Aspire
Aspire manages all resources, dependencies, and project lifecycles, ensuring all hybrid app components are discoverable and connected.

```csharp
builder.AddMauiProject("mobile", "Hybridisms.Client.NativeApp")
    .InGroup(appsGroup)
    .WithReference(web)
    .ExcludeFromManifest();
```

### 7. Automated Backend Setup
A .NET Worker Service seeds and migrates the database on startup, ensuring the backend is always ready for hybrid clients.

```csharp
protected override async Task ExecuteAsync(CancellationToken stoppingToken)
{
    await dbContext.Database.EnsureCreatedAsync(stoppingToken);
    // ...
}
```

---

## Summary
Hybridisms demonstrates a comprehensive approach to hybrid app development, combining shared code, hybrid service fallback, on-device AI, dynamic service discovery, and distributed orchestration. The result is a resilient, flexible, and modern app architecture that runs seamlessly across web, native, and cloud environments.
