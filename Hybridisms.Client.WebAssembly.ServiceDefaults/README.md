# Hybridisms.Client.WebAssembly.ServiceDefaults

This project demonstrates how to standardize service configuration for the web portion of hybrid applications, ensuring consistency between browser-based clients and their native counterparts. It showcases browser-optimized service discovery and resilience patterns.

## Hybrid Techniques Demonstrated
- **Browser-Based Service Discovery**: Enables WebAssembly clients to locate services in a hybrid application
- **Web-Optimized Resilience**: Implements retry and timeout policies tailored for browser environments
- **Configuration Consistency**: Ensures web clients configure services similarly to native clients

## Key Hybrid Features
- **Browser-Optimized Service Extensions**: Configuration tailored for web clients:
  ```csharp
  public static class Extensions
  {
      public static IServiceCollection AddServiceDefaults(this IServiceCollection services)
      {
          // Web-specific service discovery configuration
          services.AddServiceDiscovery();
          
          // Browser-optimized HTTP client configuration
          services.ConfigureHttpClientDefaults(http => {
              http.AddStandardResilienceHandler(options => {
                  // Browser-appropriate resilience settings
              });
          });
          
          return services;
      }
  }
  ```
- **Consistent Client-Side Discovery**: Uses the same discovery mechanisms as native clients
- **Parallel Configuration**: Mirrors the configuration in native clients but with web-specific optimizations

## How the Hybrid Web Configuration Works
- **Extensions Methods**: Provide the same interface as native equivalents but with browser-specific implementations
- **Common Discovery Mechanism**: Ensures web clients can find the same services as native clients
- **Platform-Appropriate Defaults**: Timeouts and retry policies tailored for browser environments

## Implementing This Pattern in Your Hybrid Apps
1. Create parallel service defaults for web and native clients
2. Ensure consistent naming and usage patterns across platforms
3. Optimize each implementation for its target platform
4. Use in tandem with native service defaults to maintain consistency

---
*This README describes the hybrid web configuration techniques demonstrated by the Hybridisms.Client.WebAssembly.ServiceDefaults project as of May 2025.*
