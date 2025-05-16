# Hybridisms.AppHost

This project demonstrates how to integrate native mobile and web clients into a .NET Aspire distributed application, enabling a true hybrid app experience with shared configuration and service discovery.

## Hybrid Techniques Demonstrated
- **Mobile/Web Integration with Aspire**: Shows how to connect native MAUI and Blazor WebAssembly clients to an Aspire distributed application
- **Client Settings Generation**: Automatically creates client-specific configuration from Aspire environment variables
- **Unified Service Discovery**: Provides a consistent way for hybrid clients to discover backend services

## Key Hybrid Features
- **AddMobileProject Extension**: Extends Aspire to support MAUI native clients:
  ```csharp
  builder.AddMobileProject("hybridisms-native-app", "../Hybridisms.Client.NativeApp");
  ```
- **Configuration Generation**: Creates client-specific settings files that get embedded in the client apps
- **Cross-Platform Resource Access**: Enables both web and native clients to access the same resources with consistent configuration

## How the Hybrid Integration Works
- **Environment Variable Translation**: Aspire environment variables are converted to static C# settings files for clients
- **Configuration at Build Time**: Since mobile apps can't receive environment variables at runtime like web services, settings are generated during development
- **Service Discovery Integration**: Mobile and web clients get the same service endpoint information, enabling consistent communication

---
*This README describes the hybrid integration techniques demonstrated by the Hybridisms.AppHost project as of May 2025.*
