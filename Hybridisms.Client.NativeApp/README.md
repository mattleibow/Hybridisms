# Hybridisms.Client.NativeApp

This project demonstrates Blazor Hybrid in a .NET MAUI app with online/offline capabilities. It showcases how to build a truly hybrid native application that works across platforms, shares code with web clients, and functions regardless of connectivity state.

## Hybrid Techniques Demonstrated
- **Blazor Hybrid**: Using web technologies inside a native shell
- **Online/Offline Operation**: Seamless switching between local and cloud data
- **Code Sharing with Web**: Same components and interfaces as the Blazor WebAssembly client
- **On-Device AI**: ML capabilities that work without cloud connectivity

## Key Hybrid Features
- **Hybrid Service Registration**: Configures services based on connectivity and capabilities:
  ```csharp
  // Register the hybrid intelligence service that switches between local and remote
  builder.Services.AddSingleton<IIntelligenceService, HybridIntelligenceService>();
  
  // Register the hybrid notes service that works online and offline
  builder.Services.AddSingleton<INotesService, HybridNotesService>();
  ```
  
- **BlazorWebView Integration**: Embeds web UI technology in native containers:
  ```xaml
  <BlazorWebView x:Name="blazorWebView" HostPage="wwwroot/index.html">
      <BlazorWebView.RootComponents>
          <RootComponent Selector="#app" ComponentType="{x:Type components:Routes}" />
      </BlazorWebView.RootComponents>
  </BlazorWebView>
  ```
  
- **Connectivity Adaptation**: Detects network state and adjusts behavior accordingly
- **Local AI Models**: Embedded ML capabilities that don't require cloud access
- **Shared Components**: Uses the exact same UI components as the web version

## Structure
- **Components/**: Blazor UI components for the native app.
- **Data/**: Local database context and models.
- **Services/**: Local, remote, and hybrid service implementations for notes and intelligence.
- **Platforms/**: Platform-specific code for MAUI.
- **Resources/**: App resources and assets.
- **wwwroot/**: Static assets for Blazor components.

## How the Hybrid Native Pattern Works
- **Application Shell**: MAUI provides the native platform capabilities and OS integration
- **BlazorWebView**: Renders web-based UI inside the native container
- **Service Layer Switching**: Transparently changes between:
  - Local SQLite database when offline 
  - Remote API when online
  - Synchronization when transitioning between states
- **Component Reuse**: The same Blazor components work in both contexts
- **Platform Services**: Native-only features are provided through dependency injection

## Implementing This Pattern in Your Hybrid Apps
1. Create a MAUI Blazor app with BlazorWebView
2. Integrate shared component libraries
3. Implement hybrid services that can switch between local and remote implementations
4. Include local database and synchronization logic
5. Embed ONNX models for offline AI capabilities

---
*This README describes the hybrid native techniques demonstrated by the Hybridisms.Client.NativeApp project as of May 2025.*
