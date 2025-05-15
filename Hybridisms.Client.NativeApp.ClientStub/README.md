# Hybridisms.Client.NativeApp.ClientStub

This project demonstrates a key hybrid application technique: bridging the gap between distributed application configuration and native mobile clients. It solves the problem of how to get Aspire environment variables into a native application that doesn't receive them at runtime.

## Hybrid Technique Demonstrated
- **Configuration Bridge Pattern**: Translating server-side distributed application settings to client-side configuration
- **Build-Time Settings Generation**: Creating compile-time configurations for native apps from runtime application settings

## How the Hybrid Bridge Works
- **Aspire Integration**: The stub project is added to the Aspire application model
- **Environment Variables**: Aspire provides environment variables to the stub project at development time
- **Settings Generation**: The stub converts environment variables to a C# settings file:
  ```csharp
  // AspireAppSettings.g.cs is generated
  public static class AspireAppSettings 
  {
      public static class ServiceA 
      {
          public const string Endpoint = "https://servicea.example.com";
      }
      // ...
  }
  ```
- **Build Integration**: The generated settings file is included in the native app at build time

## Implementing This Pattern in Your Hybrid Apps
1. Create a ClientStub project for your native app
2. Configure the Aspire AppHost to reference your ClientStub project
3. Set up the ClientStub to generate settings based on environment variables
4. Reference the generated settings in your native app
5. Use the settings to connect to services in the distributed application

---
*This README describes the hybrid configuration bridge technique demonstrated by the Hybridisms.Client.NativeApp.ClientStub project as of May 2025.*
