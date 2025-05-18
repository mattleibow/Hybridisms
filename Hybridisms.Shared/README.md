# Hybridisms.Shared

## Overview
Hybridisms.Shared is a class library that defines the shared contracts, models, and service interfaces for the Hybridisms demonstration app. It enables code reuse and consistent data structures across all hybrid client and server projects.

## What the Project Does
- Defines core data models for notes, notebooks, and topics.
- Provides service interfaces for notes and AI-powered intelligence features.
- Implements remote service clients for HTTP-based communication.
- Supplies utility extensions for hybrid rendering and AI chat integration.

## Implementation Architecture
- **Models**: Includes `Note`, `Notebook`, `Topic`, and `TopicRecommendation` for consistent data representation.
- **Service Interfaces**: `INotesService` and `IIntelligenceService` abstract data and AI operations for all clients.
- **Remote Service Implementations**: `RemoteNotesService` and `RemoteIntelligenceService` enable HTTP-based access to backend APIs.
- **Hybrid Render Modes**: `HybridRenderMode` provides helpers for switching between server, auto, and WebAssembly rendering in Blazor.
- **Chat Client Extensions**: Utility methods for interacting with AI chat clients in a hybrid manner.

## Hybrid App Enablement
- **Code Sharing**: All client and server projects reference this library, ensuring a single source of truth for models and interfaces.
- **Remote/Local Service Swapping**: Clients can switch between local and remote implementations of services for hybrid flexibility.
- **Hybrid Rendering**: Blazor projects use shared render mode helpers to adapt to different hosting environments.

## Example: Shared Service Interface
```csharp
public interface INotesService
{
    Task<ICollection<Notebook>> GetNotebooksAsync(CancellationToken cancellationToken = default);
    Task<Notebook?> GetNotebookAsync(Guid notebookId, CancellationToken cancellationToken = default);
    // ...other methods...
}
```

## Example: Remote Service Implementation
```csharp
public class RemoteNotesService(HttpClient httpClient) : INotesService
{
    public async Task<ICollection<Notebook>> GetNotebooksAsync(CancellationToken cancellationToken = default)
    {
        var notebooks = await httpClient.GetFromJsonAsync<ICollection<Notebook>>("api/notebook");
        return notebooks ?? [];
    }
    // ...other methods...
}
```

## Example: Hybrid Render Mode Helper
```csharp
public static class HybridRenderMode
{
    public static IComponentRenderMode? InteractiveServer { get; } = IfSupported(RenderMode.InteractiveServer);
    public static IComponentRenderMode? InteractiveWebAssembly { get; } = IfSupported(RenderMode.InteractiveWebAssembly);
    // ...
}
```

## Summary
Hybridisms.Shared is the foundation for hybrid app development in the Hybridisms solution, providing shared contracts, models, and utilities for seamless interoperability across all platforms.
