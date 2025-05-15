# Hybridisms

Hybridisms is a demo application that showcases hybrid development techniques using .NET. It demonstrates how to build a cross-platform note-taking application that works seamlessly across web, desktop, and mobile with shared code and UI components. The key hybrid techniques demonstrated include:

1. **Code sharing** between web and native platforms using Blazor
2. **Hybrid rendering modes** that adapt to different environments
3. **Online/offline operation** with synchronized data
4. **Hybrid AI features** that work both in the cloud and locally on-device
5. **Distributed application architecture** with .NET Aspire

## Key Hybrid Techniques Demonstrated

### 1. Cross-Platform UI with Blazor
- Single UI component library used across web and native platforms
- Adaptive rendering based on platform capabilities
- Consistent user experience regardless of device

### 2. Hybrid Data Access
- Works online (cloud-connected) and offline (local database)
- Automatic synchronization when connectivity is restored
- Common service interfaces with different implementations

### 3. Hybrid AI Services
- Cloud-based AI using Azure OpenAI for web clients
- On-device AI using ONNX models for native clients
- Consistent intelligence features regardless of connectivity

### 4. Aspire Mobile Integration
- Client-side settings generation from distributed application configuration
- Mobile-friendly service discovery and resilience patterns

## Solution Architecture

The Hybridisms solution demonstrates these hybrid techniques through several key projects:

### 1. **Hybridisms.Shared** ([README](Hybridisms.Shared/README.md))
The core of the hybrid strategy, containing:
- **Cross-Platform UI Components**: Blazor components used by both web and native clients
- **Service Interfaces**: Common abstractions allowing for different implementations
- **HybridRenderMode**: Utility that adapts rendering based on the runtime environment
- **Shared Models**: Domain models used across all platforms

### 2. **Hybridisms.Client.Native** ([README](Hybridisms.Client.Native/README.md))
Demonstrates hybrid data access and intelligence:
- **HybridNotesService**: Combines local and remote data access transparently
- **HybridIntelligenceService**: Uses on-device AI when offline, cloud AI when online
- **Local Database**: SQLite implementation for offline operation
- **ONNX Integration**: On-device ML models for local intelligence features

### 3. **Hybridisms.Client.NativeApp** ([README](Hybridisms.Client.NativeApp/README.md))
Showcases Blazor Hybrid in MAUI:
- **Blazor Hybrid UI**: Using web technologies in a native app shell
- **Shared Components**: Reuses components from Hybridisms.Shared
- **Online/Offline Support**: Works with or without connectivity

### 4. **Hybridisms.Client.WebAssembly** ([README](Hybridisms.Client.WebAssembly/README.md))
The browser-based client that:
- **Shares UI**: Uses the same components as the native app
- **Remote Services**: Connects to backend APIs for data and intelligence

### 5. **Hybridisms.AppHost** ([README](Hybridisms.AppHost/README.md))
Demonstrates .NET Aspire integration:
- **Mobile Project Integration**: Uses AspireMobile to connect mobile clients
- **Configuration Generation**: Creates settings files for mobile/WASM clients

### 6. **ClientStub Projects**
- **Hybridisms.Client.NativeApp.ClientStub** & **Hybridisms.Client.WebAssembly.ClientStub**
  - Bridge between Aspire distributed app model and client-side settings
  - Enables mobile and WASM apps to participate in the distributed application

## Key Hybrid Implementation Techniques

### Shared UI with Blazor
- **Component Library**: Components in `Hybridisms.Shared` are designed to be used in both web and native contexts
- **HybridRenderMode**: Custom utility that selects the appropriate render mode (Server, WebAssembly, Auto) based on runtime platform
- **Responsive Design**: UI adapts to different form factors and screen sizes

### Hybrid Data Services
- **Service Interfaces**: Common interfaces like `INotesService` and `IIntelligenceService` define capabilities
- **Multiple Implementations**: Remote (HTTP), Local (SQLite), and Hybrid implementations
- **Hybrid Pattern**: The `HybridNotesService` in Client.Native transparently switches between local and remote data sources
  - Uses local database when offline
  - Syncs with cloud when online
  - Provides consistent API regardless of connectivity

### Hybrid AI Features
- **Abstracted Intelligence**: Common `IIntelligenceService` interface
- **Web Implementation**: Uses Azure OpenAI via REST APIs
- **Native Implementation**: Uses ONNX runtime with local ML models
  - `OnnxChatClient`: On-device chat capabilities
  - `OnnxEmbeddingClient`: On-device text embeddings for semantic search

### Hybrid App Configuration
- **AspireMobile Integration**: Extends .NET Aspire to work with mobile/WASM clients
- **Client Stubs**: Generate settings from Aspire environment variables
- **Common Service Defaults**: Shared configuration for resilience and service discovery

## Learning From This Demo

- Examine `HybridRenderMode.cs` to see how UI adapts to different platforms
- Study `HybridNotesService.cs` and `HybridIntelligenceService.cs` to understand online/offline strategies
- Look at `ClientStub` projects to learn how Aspire configuration works with client apps
- View common Blazor components in the `Shared` project to see cross-platform UI patterns

---

*This demo shows hybrid app development techniques using .NET technologies as of May 2025.*