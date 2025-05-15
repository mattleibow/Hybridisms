# Hybridisms.Shared

This project is the cornerstone of the Hybridisms demo's hybrid strategy. It provides shared client-side logic, models, and UI components that work across Blazor WebAssembly (browser) and .NET MAUI (native) platforms, demonstrating how to maximize code sharing in hybrid applications.

## Hybrid Techniques Demonstrated

- **Write Once, Run Anywhere UI**: Blazor components that work in both web and native environments
- **Adaptive Rendering**: `HybridRenderMode` utility that selects the appropriate render mode based on runtime capabilities
- **Common Service Abstractions**: Interface-based design that allows for platform-specific implementations while maintaining a consistent API
- **Shared Domain Model**: Single set of models used across all platforms

## Key Hybrid Features

- **HybridRenderMode**: Core utility that adapts rendering based on available runtime features:
  ```csharp
  // Use in Razor components like this:
  @renderMode="@HybridRenderMode.InteractiveAuto"
  ```
  
- **Service Interfaces**: Platform-agnostic interfaces that enable swappable implementations:
  - `INotesService`: For data operations across any platform
  - `IIntelligenceService`: For AI features that work both in the cloud and on-device

- **Remote Implementations**: HTTP-based service implementations for web scenarios
  
- **Cross-Platform UI Components**: Blazor components that adapt to where they're running

## Structure

### Models (`Services/Models`)
- **ModelBase**: Abstract base class with `Id`, `Created`, and `Modified` properties.
- **Note**: Represents a note, including title, content (Markdown), starred status, topics, and notebook association. Automatically tracks changes and generates HTML from Markdown.
- **Notebook**: Represents a notebook, with title, description, and a collection of notes.
- **Topic**: Represents a topic/tag, with name and color.
- **TopicRecommendation**: Used for AI-powered topic suggestions, pairing a `Topic` with a reason.

### Service Interfaces (`Services`)
- **INotesService**: Abstraction for CRUD operations on notebooks, notes, and topics. Used by both local and remote implementations.
- **IIntelligenceService**: Abstraction for AI-powered features, such as recommending topics for a note and generating note content from a prompt.

### Remote Service Implementations (`Services`)
- **RemoteNotesService**: Implements `INotesService` using HTTP APIs. Handles all CRUD operations by calling backend endpoints.
- **RemoteIntelligenceService**: Implements `IIntelligenceService` using HTTP APIs for AI features (topic recommendations, content generation).

### Extensions (`Services`)
- **NotesServiceExtensions**: Helper methods for working with `INotesService` (e.g., saving a single notebook).
- **ChatClientExtensions**: Helper methods for interacting with chat-based AI clients.

### Components (`Components`)
- **TagInput**: Blazor component for editing a list of tags/topics, with support for suggestions and recommendations.
- **Pages**: UI pages for editing and viewing notes and notebooks:
  - `DeviceInfo.razor`: Shows device and render mode info.
  - `Edit.razor`: Example interactive page.
  - `NotebookEdit.razor`: Edit notebook details.
  - `NotebookNew.razor`: Create a new notebook.
  - `NotebookNotes.razor`: List notes in a notebook.
  - `NoteEdit.razor`: Edit a note, including AI-powered topic suggestions and content generation.

### Utilities
- **HybridRenderMode**: Utility for selecting the appropriate Blazor render mode (server, WASM, or auto) based on runtime support.

## How the Hybrid Pattern Works

### Adaptive Rendering
The `HybridRenderMode` class demonstrates how to select the appropriate rendering strategy based on the runtime environment:

```csharp
public static class HybridRenderMode
{
    // Different render modes for different environments
    public static IComponentRenderMode? InteractiveServer { get; } = IfSupported(RenderMode.InteractiveServer);
    public static IComponentRenderMode? InteractiveAuto { get; } = IfSupported(RenderMode.InteractiveAuto);
    public static IComponentRenderMode? InteractiveWebAssembly { get; } = IfSupported(RenderMode.InteractiveWebAssembly);

    // Checks if the current environment supports the requested render mode
    private static IComponentRenderMode? IfSupported(IComponentRenderMode? mode) =>
        !AppContext.TryGetSwitch("Hybridisms.SupportsRenderMode", out var isEnabled) || isEnabled ? mode : null;
}
```

### Platform-Agnostic Service Design
Services are designed with interfaces that can be implemented differently for each platform:

- **Web Client**: Uses `RemoteNotesService` and `RemoteIntelligenceService` to call APIs
- **Native Client**: Can use `EmbeddedNotesService` for offline or `HybridNotesService` for mixed operation

### Implementing Your Own Hybrid Apps

1. **Reference this approach**: Study how the shared components and services are designed
2. **Use the interface pattern**: Define capabilities as interfaces with multiple implementations
3. **Leverage HybridRenderMode**: Adapt your UI based on the runtime environment
4. **Create hybrid services**: Implement services that can work both online and offline

---

*This README was generated automatically to describe the structure and purpose of the Hybridisms.Shared project as of May 2025.*
