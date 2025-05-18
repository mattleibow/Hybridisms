# Hybridisms.Client.WebAssembly.ServiceDefaults

## Overview
Hybridisms.Client.WebAssembly.ServiceDefaults is a configuration library for Blazor WebAssembly hybrid clients in the Hybridisms solution. It provides service discovery, resilience, and telemetry defaults for browser-based hybrid apps.

## What the Project Does
- Configures service discovery for Blazor WebAssembly clients.
- Adds HTTP resilience and OpenTelemetry instrumentation.
- Centralizes environment and logging settings for hybrid deployments.

## Implementation Architecture
- **Service Discovery**: Integrates with Microsoft.Extensions.ServiceDiscovery for dynamic endpoint resolution.
- **Resilience**: Adds HTTP resilience policies for robust hybrid communication.
- **Telemetry**: Configures OpenTelemetry for distributed tracing and monitoring.

## Hybrid App Enablement
- **Dynamic Endpoints**: Enables WASM hybrid clients to discover and connect to backend services in any environment.
- **Unified Telemetry**: Ensures consistent monitoring and diagnostics across hybrid app components.
- **Centralized Configuration**: Simplifies hybrid deployment and management.

## Example: Service Discovery Configuration
```xml
<PackageReference Include="Microsoft.Extensions.ServiceDiscovery" />
```

## Summary
Hybridisms.Client.WebAssembly.ServiceDefaults provides essential configuration for robust, discoverable, and observable hybrid WASM clients.
