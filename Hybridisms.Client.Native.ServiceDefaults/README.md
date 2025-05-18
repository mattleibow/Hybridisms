# Hybridisms.Client.Native.ServiceDefaults

## Overview
Hybridisms.Client.Native.ServiceDefaults is a configuration library for native hybrid clients in the Hybridisms solution. It provides service discovery, resilience, and telemetry defaults for MAUI and other native apps.

## What the Project Does
- Configures service discovery for hybrid native clients.
- Adds HTTP resilience and OpenTelemetry instrumentation.
- Centralizes environment and logging settings for hybrid deployments.

## Implementation Architecture
- **Service Discovery**: Integrates with Microsoft.Extensions.ServiceDiscovery for dynamic endpoint resolution.
- **Resilience**: Adds HTTP resilience policies for robust hybrid communication.
- **Telemetry**: Configures OpenTelemetry for distributed tracing and monitoring.

## Hybrid App Enablement
- **Dynamic Endpoints**: Enables native hybrid clients to discover and connect to backend services in any environment.
- **Unified Telemetry**: Ensures consistent monitoring and diagnostics across hybrid app components.
- **Centralized Configuration**: Simplifies hybrid deployment and management.

## Example: Service Discovery Configuration
```xml
<PackageReference Include="Microsoft.Extensions.ServiceDiscovery" />
```

## Summary
Hybridisms.Client.Native.ServiceDefaults provides essential configuration for robust, discoverable, and observable hybrid native apps.
