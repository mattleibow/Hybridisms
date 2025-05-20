# Hybridisms.Client.NativeApp

## Overview
Hybridisms.Client.NativeApp is a .NET MAUI Blazor Hybrid project that serves as the native mobile and desktop client for the Hybridisms demonstration app. It combines native device features with Blazor-based UI and supports both local and remote AI/data services for hybrid scenarios.

## What the Project Does
- Provides a cross-platform native app (Android, iOS, Windows, MacCatalyst) using .NET MAUI and Blazor Hybrid.
- Integrates Blazor WebView for a rich, reusable UI across platforms.
- Supports both local (on-device) and remote (cloud/server) AI and data services.
- Uses ONNX models for embedded AI capabilities when offline or remote services are unavailable.
- Synchronizes data with backend APIs and supports offline-first scenarios.

## Implementation Architecture
- **MAUI Blazor Hybrid**: Combines native app shell with Blazor WebView for hybrid UI.
- **Hybrid Service Registration**: Registers both remote (HTTP) and embedded (local) implementations for notes and intelligence services, using a hybrid fallback strategy.
- **ONNX Integration**: Bundles ONNX models for on-device AI (embeddings, chat, recommendations).
- **SQLite Local Storage**: Uses a local SQLite database for offline data persistence.
- **Service Defaults**: Configures service discovery and environment settings for hybrid deployments.

## Hybrid App Enablement
- **Hybrid Service Fallback**: The app uses remote services when available, falling back to local/embedded services when offline or remote is unavailable.
- **Shared UI and Logic**: Reuses Blazor components and shared contracts for consistency with web and WASM clients.
- **On-Device AI**: Enables hybrid intelligence scenarios by running ONNX models locally when needed.

## Example: Hybrid Service Registration
```csharp
// Register the hybrid services that we will use
builder.Services.AddSingleton<INotesService, HybridNotesService>();
builder.Services.AddSingleton<IIntelligenceService, HybridIntelligenceService>();
```

## Example: ONNX Model Integration for Embedded AI
```csharp
builder.Services.AddOptions<OnnxEmbeddingClient.EmbeddingClientOptions>()
    .Configure(options => {
        options.BundledPath = "Models/miniml_model.zip";
        options.ExtractedPath = Path.Combine(FileSystem.AppDataDirectory, "Models", "embedding_model");
    });
builder.Services.AddSingleton<OnnxEmbeddingClient>();
```

## Summary
Hybridisms.Client.NativeApp demonstrates a true hybrid approach, combining native device features, Blazor UI, and both local and remote AI/data services for a seamless, resilient user experience across platforms.
