# Hybridisms.Client.WebAssembly.ClientStub

This project demonstrates how to bridge the configuration gap between Aspire distributed applications and Blazor WebAssembly clients. It's a key component of the hybrid application pattern, enabling consistent configuration across web and native clients.

## Hybrid Technique Demonstrated
- **Configuration Bridge Pattern**: Converting Aspire environment settings to WebAssembly-compatible configuration
- **Build-Time Settings Generation**: Creating static settings that can be embedded in the browser-based client

## How the Hybrid Configuration Bridge Works
- **Aspire Integration**: The stub project is configured in the Aspire application model
- **Environment Variables**: Aspire provides service URLs and other settings as environment variables
- **Settings Generation**: The stub converts these to a C# settings file that works in the browser context
- **Unified Configuration**: Both WebAssembly and native clients get settings from the same source

## Implementing This Pattern in Your Hybrid Apps
1. Create stub projects for both WebAssembly and native clients
2. Configure them in your Aspire AppHost
3. Access the same services with consistent configuration in both contexts
4. Benefit from service discovery and configuration changes without duplicating settings

---
*This README describes the hybrid configuration technique demonstrated by the Hybridisms.Client.WebAssembly.ClientStub project as of May 2025.*
