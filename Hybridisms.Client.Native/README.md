# Hybridisms.Client.Native

This project demonstrates how to implement hybrid services that work both online and offline in native applications. It's a core part of the Hybridisms hybrid app demo, showing how to create seamless experiences regardless of connectivity.

## Hybrid Techniques Demonstrated

- **Hybrid Service Pattern**: Services that transparently switch between local and remote data
- **Offline-First Architecture**: Local database with synchronization capabilities
- **On-Device AI**: ONNX-based machine learning for offline intelligence features

## Key Hybrid Components

### Hybrid Services
- **HybridNotesService**: Implements the `INotesService` interface by combining:
  - Local storage via `EmbeddedNotesService` (works offline)
  - Remote APIs via `RemoteNotesService` (when online)
  - Synchronization logic between the two
  
- **HybridIntelligenceService**: Implements `IIntelligenceService` to provide AI features that work:
  - Via cloud when online (Azure OpenAI)
  - Via on-device ML models when offline (ONNX runtime)

### On-Device ML
- **OnnxModelClient**: Base class for on-device machine learning
- **OnnxChatClient**: Local chat capabilities using embedded models
- **OnnxEmbeddingClient**: Text embedding generation for semantic features

## How to Use This Pattern in Your Apps

1. Define common service interfaces (like we do in Hybridisms.Shared)
2. Create purely local implementations for offline use
3. Create remote implementations for online scenarios
4. Build hybrid implementations that combine both approaches
5. Use dependency injection to register the appropriate services based on app needs

---
*This README was generated automatically to describe the structure and purpose of the Hybridisms.Client.Native project as of May 2025.*
