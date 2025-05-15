# Hybridisms.Client.Native.ServiceDefaults

This project demonstrates how to standardize service discovery and resilience patterns across native clients in hybrid applications. It provides configuration extensions that ensure native apps can reliably communicate with backend services regardless of network conditions.

## Hybrid Techniques Demonstrated
- **Native App Service Discovery**: Enables MAUI clients to find and connect to services in a distributed hybrid application
- **Cross-Platform Resilience Policies**: Implements retry and circuit-breaker patterns for native clients
- **Observability in Hybrid Apps**: Configures consistent telemetry across web and native platforms

## Key Hybrid Features
- **Client-Side Service Discovery**: Extensions to locate services without hardcoded endpoints:
  ```csharp
  // Add service discovery for native clients
  builder.Services.AddServiceDiscovery(options => { /* configuration */ });
  ```
- **Resilience Policies**: Configures HTTP clients with retry and circuit-breaker patterns tailored for mobile environments:
  ```csharp
  // Configure resilience strategies for native network conditions
  builder.Services.ConfigureHttpClientDefaults(http => {
      http.AddStandardResilienceHandler(options => {
          options.Retry.MaxRetryAttempts = 5;
          // Mobile-optimized retry delays and timeouts
      });
  });
  ```
- **Cross-Platform Telemetry**: Ensures consistent observability across web and native clients

## How the Hybrid Service Configuration Works
- **Unified Configuration**: Shared service configuration patterns across different platforms
- **Platform-Specific Adaptations**: Mobile-optimized resilience strategies
- **Native Service Discovery**: Mobile clients can discover backend services using the same mechanisms as web clients
- **Health-Aware Clients**: Native clients can check service health before attempting operations

## Implementing This Pattern in Your Hybrid Apps
1. Create service defaults packages for each client type (native, web)
2. Implement shared core functionality with platform-specific optimizations
3. Ensure consistent service discovery and resilience strategies
4. Configure native-specific telemetry collection

---
*This README describes the hybrid service configuration techniques demonstrated by the Hybridisms.Client.Native.ServiceDefaults project as of May 2025.*
