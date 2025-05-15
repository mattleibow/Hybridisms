# Hybridisms.Server.WebApp

This project demonstrates a unified backend for hybrid applications, serving both web and native clients through a combination of HTTP APIs and multiple Blazor rendering modes. It showcases how a single server can support different client types with varied connectivity patterns.

## Hybrid Techniques Demonstrated
- **Multi-Client Backend**: Serves both Blazor WebAssembly and MAUI native clients
- **Multiple Render Modes**: Supports Server, WebAssembly and Auto render modes
- **Cloud AI Services**: Provides AI capabilities to both connected and occasionally-connected clients
- **Shared Component Hosting**: Serves UI components that work across platforms

## Key Hybrid Features
- **Multi-Mode Blazor Configuration**: Configures the server to support multiple rendering scenarios:
  ```csharp
  builder.Services.AddRazorComponents()
      .AddInteractiveServerComponents()  // For server-side rendering
      .AddInteractiveWebAssemblyComponents();  // For WebAssembly clients
  ```

- **API Controllers**: Serve both web and native clients through the same HTTP endpoints:
  ```csharp
  // NotesController serves both web and native clients
  [ApiController]
  [Route("api/[controller]")]
  public class NotesController : ControllerBase
  {
      // Common API endpoints for all clients
  }
  ```

- **Adaptive Routing**: Maps components with multiple render modes:
  ```csharp
  app.MapRazorComponents<App>()
      .AddInteractiveServerRenderMode()
      .AddInteractiveWebAssemblyRenderMode()
      .AddAdditionalAssemblies(/* shared component assemblies */);
  ```

- **Cloud-Based AI**: Provides Azure OpenAI services to clients regardless of platform

## Structure
- **Controllers/**: API endpoints for notes, topics, and intelligence.
- **Components/**: Blazor server-side UI components.
- **wwwroot/**: Static web assets.

The project references and uses:
- **Hybridisms.Server/Data/**: Entity Framework Core context and data models for persistent storage.
- **Hybridisms.Server/Services/**: Core business logic for notes, topics, and AI features.

## How the Hybrid Backend Pattern Works
- **Unified API Surface**: The same API endpoints serve both web and native clients
- **Component Sharing**: Server-side components are shared with Blazor WebAssembly and MAUI clients
- **Render Mode Detection**: The server detects client capabilities and adapts rendering accordingly
- **Service Registration**: Different service implementations are registered for different client scenarios
- **Data Synchronization**: Supports clients that may connect intermittently

## Implementing This Pattern in Your Hybrid Apps
1. Configure your server to support multiple Blazor render modes
2. Design API controllers to work with both web and native clients
3. Set up cloud-based services that can be accessed from any client platform
4. Use shared component libraries that work across different rendering scenarios
5. Implement synchronization patterns for occasionally-connected clients

---
*This README describes the hybrid backend techniques demonstrated by the Hybridisms.Server.WebApp project as of May 2025.*
