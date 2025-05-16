# Hybridisms.Client.WebAssembly

This project demonstrates the web side of a hybrid application strategy using Blazor WebAssembly, showing how to share UI components, business logic, and service interfaces with native clients while still delivering a full browser-based experience.

## Hybrid Techniques Demonstrated
- **Component Sharing**: Uses the same Blazor components as the native MAUI app
- **Common Interfaces**: Implements the same service contracts as the native client
- **Aspire Integration**: Gets configuration from a distributed application model

## Key Hybrid Features
- **Shared Components**: Uses the exact same UI components from Hybridisms.Shared that the native app uses
- **Remote Service Pattern**: Implements the common service interfaces using HTTP APIs:
  ```csharp
  // Same interface as native, different implementation
  builder.Services.AddScoped<INotesService, RemoteNotesService>();
  builder.Services.AddScoped<IIntelligenceService, RemoteIntelligenceService>();
  ```
- **Platform Feature Detection**: Works with HybridRenderMode to optimize rendering

## Structure
- **wwwroot/**: Static web assets.
- **_Imports.razor**: Common Razor imports for the client.
- **Program.cs**: Client startup and service registration.

## How the Hybrid Sharing Works
- **Common Abstraction Layer**: Interfaces like `INotesService` allow for different implementations across platforms
- **Blazor UI Reuse**: Identical UI components work in both WebAssembly and MAUI Blazor Hybrid
- **Consistent User Experience**: Users get the same functionality whether in browser or native app
- **Unified Service Discovery**: Configuration is generated from the same Aspire host that configures the backend

## Implementing This Pattern in Your Hybrid Apps
1. Create shared component and service interface libraries
2. Build web client implementations using HTTP APIs
3. Implement the same interfaces in native clients

---
*This README describes the hybrid web techniques demonstrated by the Hybridisms.Client.WebAssembly project as of May 2025.*
