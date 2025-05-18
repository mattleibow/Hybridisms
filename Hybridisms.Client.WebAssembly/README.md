# Hybridisms.Client.WebAssembly

## Overview
Hybridisms.Client.WebAssembly is a Blazor WebAssembly (WASM) project that serves as a browser-based client for the Hybridisms demonstration app. It demonstrates how hybrid web clients can interact with shared backend services and leverage remote intelligence features.

## What the Project Does
- Provides a rich, interactive UI running entirely in the browser using Blazor WebAssembly.
- Connects to backend APIs for notes, topics, notebooks, and AI features via HTTP.
- Uses shared service interfaces and models to enable code reuse across hybrid clients.
- Configures service discovery and environment settings for hybrid deployments.

## Implementation Architecture
- **Blazor WebAssembly**: Runs C# code in the browser, enabling a modern SPA experience.
- **Remote Services**: Registers `INotesService` and `IIntelligenceService` as HTTP clients, pointing to the backend server (`webapp`).
- **Service Defaults**: Uses `AddServiceDefaults()` for consistent configuration and service discovery in hybrid environments.
- **Shared Contracts**: Reuses models and interfaces from `Hybridisms.Shared` for seamless interoperability with other clients and the server.

## Hybrid App Enablement
- **Remote Service Integration**: The client is designed to work with remote APIs, making it easy to swap between local and cloud-hosted backends.
- **Shared Codebase**: By referencing shared contracts, the project ensures consistent data models and business logic across all hybrid clients.
- **Environment Configuration**: Uses generated Aspire app settings and service defaults for flexible deployment in hybrid scenarios.

## Example: Registering Remote Services
The client configures its data and AI services to use HTTP APIs exposed by the server:

```csharp
builder.Services.AddHttpClient<INotesService, RemoteNotesService>(static client => client.BaseAddress = new("https+http://webapp/"));
builder.Services.AddHttpClient<IIntelligenceService, RemoteIntelligenceService>(static client => client.BaseAddress = new("https+http://webapp/"));
```

## Example: Service Defaults for Hybrid Environments

```csharp
builder.AddServiceDefaults();
```

## Summary
Hybridisms.Client.WebAssembly demonstrates how to build a browser-based hybrid client using Blazor WebAssembly, shared contracts, and remote service integration for a seamless hybrid app experience.
