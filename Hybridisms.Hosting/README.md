# Hybridisms.Hosting

This project demonstrates server-side hosting patterns for hybrid applications, providing shared components and routes that enable consistent server-rendered experiences for both web and native clients.

## Hybrid Techniques Demonstrated
- **Server-Side Rendering**: Enables hybrid rendering from a single server codebase
- **Shared Server Components**: Components that can be used by both web and native clients
- **Common Routing**: Unified routing that works across different client types

## Key Hybrid Features
- **Routes.razor**: Centralized routing that works across different client types and rendering modes:
  ```razor
  <Router AppAssembly="@typeof(Routes).Assembly"
          AdditionalAssemblies="@AdditionalAssemblies">
      <!-- Common routing configuration for all clients -->
  </Router>
  ```
- **Components**: Server-side components that adapt to different client contexts
- **Auto-Discovery**: Support for dynamically loading components regardless of client type

## How the Hybrid Server Pattern Works
- **Single Host**: One server codebase serves both web and native clients
- **Adaptive Rendering**: Components detect client capabilities and adjust accordingly
- **Component Sharing**: Server components are shared with client projects where appropriate
- **Unified Routes**: Common routing ensures consistent navigation patterns

## Implementing This Pattern in Your Hybrid Apps
1. Create a shared server-side hosting project
2. Implement common routes and server components
3. Configure rendering modes that work with both web and native clients
4. Use the project in your server applications with references to shared client code

---
*This README describes the hybrid hosting techniques demonstrated by the Hybridisms.Hosting project as of May 2025.*
