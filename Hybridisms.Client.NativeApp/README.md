# Hybridisms.Client.NativeApp

This project is the .NET MAUI (native) client for the Hybridisms suite. It provides a cross-platform mobile and desktop app for managing notes, notebooks, and topics, with integrated AI features.

## Purpose
- Delivers a native app experience for Hybridisms on mobile and desktop platforms.
- Supports offline data storage and hybrid cloud/local operation.
- Integrates AI-powered features for note-taking and topic recommendations.

## Key Features
- **Blazor Hybrid UI**: Uses Blazor for cross-platform UI with .NET MAUI.
- **Local Database**: Uses SQLite for offline data storage.
- **Hybrid Services**: Combines local and remote services for notes and intelligence.
- **ONNX AI Integration**: Supports local AI models for embeddings and chat.
- **Reusable Components**: Shares UI and logic with Blazor web client via Hybridisms.Client.Shared.

## Structure
- **Components/**: Blazor UI components for the native app.
- **Data/**: Local database context and models.
- **Services/**: Local, remote, and hybrid service implementations for notes and intelligence.
- **Platforms/**: Platform-specific code for MAUI.
- **Resources/**: App resources and assets.
- **wwwroot/**: Static assets for Blazor components.

## How It Works
- Registers both local and remote services for notes and intelligence.
- Uses ONNX models for local AI features when available.
- Supports hybrid operation: works offline and syncs with the cloud when online.

## Usage
- Build and run this project to launch the native Hybridisms app.
- Works on Windows, macOS, Android, and iOS (where supported by .NET MAUI).

---
*This README was generated automatically to describe the structure and purpose of the Hybridisms.Client.NativeApp project as of May 2025.*
