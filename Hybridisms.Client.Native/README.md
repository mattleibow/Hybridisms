# Hybridisms.Client.Native

## Overview
Hybridisms.Client.Native is a class library that provides native data and AI service implementations for the Hybridisms hybrid app. It enables on-device storage and intelligence for native and hybrid scenarios.

## What the Project Does
- Implements local data storage using SQLite and Entity Framework Core for native clients.
- Provides embedded AI services using ONNX models for offline/hybrid intelligence.
- Supplies hybrid service implementations that can switch between local and remote services.

## Implementation Architecture
- **Embedded Services**: `EmbeddedNotesService` and `EmbeddedIntelligenceService` provide on-device data and AI.
- **Hybrid Services**: `HybridNotesService` and `HybridIntelligenceService` combine local and remote services for resilience.
- **ONNX Integration**: Uses ONNX models for local AI tasks (embeddings, chat, recommendations).
- **DbContext**: `HybridismsEmbeddedDbContext` manages local data persistence.

## Hybrid App Enablement
- **Offline Support**: Enables hybrid apps to function offline with local data and AI.
- **Service Fallback**: Hybrid services automatically switch between local and remote as needed.
- **Shared Contracts**: Implements shared interfaces for seamless integration with other hybrid app components.

## Example: Hybrid Intelligence Service
```csharp
public class HybridIntelligenceService : IIntelligenceService
{
    public Task<string> GenerateNoteContentsAsync(string prompt, CancellationToken cancellationToken = default)
    {
        // Try remote, fallback to local
    }
}
```

## Summary
Hybridisms.Client.Native enables robust, offline-capable hybrid apps by providing native data and AI services for use in MAUI and other native clients.
