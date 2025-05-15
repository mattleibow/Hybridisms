# Hybridisms.Shared

This project provides shared client-side logic, models, and UI components for the Hybridisms application suite. It is designed to be used by both Blazor WebAssembly and .NET MAUI (native) clients, enabling code reuse and consistent behavior across platforms.

## Purpose

- **Centralizes** business logic, data models, and reusable UI components for notes, notebooks, and topics.
- **Defines** service interfaces and remote service implementations for accessing and managing notes and AI-powered features.
- **Enables** hybrid rendering and platform-agnostic UI with Blazor components.

## Key Features

- **Data Models**: Strongly-typed models for notes, notebooks, and topics, supporting metadata, relationships, and change tracking.
- **Service Interfaces**: Abstractions for notes and intelligence (AI) services, with remote HTTP implementations.
- **Reusable Components**: Blazor components for editing and displaying notes, notebooks, and topics.
- **Hybrid Render Mode**: Utilities for supporting both server and WebAssembly rendering.

## Structure

### Models (`Services/Models`)
- **ModelBase**: Abstract base class with `Id`, `Created`, and `Modified` properties.
- **Note**: Represents a note, including title, content (Markdown), starred status, topics, and notebook association. Automatically tracks changes and generates HTML from Markdown.
- **Notebook**: Represents a notebook, with title, description, and a collection of notes.
- **Topic**: Represents a topic/tag, with name and color.
- **TopicRecommendation**: Used for AI-powered topic suggestions, pairing a `Topic` with a reason.

### Service Interfaces (`Services`)
- **INotesService**: Abstraction for CRUD operations on notebooks, notes, and topics. Used by both local and remote implementations.
- **IIntelligenceService**: Abstraction for AI-powered features, such as recommending topics for a note and generating note content from a prompt.

### Remote Service Implementations (`Services`)
- **RemoteNotesService**: Implements `INotesService` using HTTP APIs. Handles all CRUD operations by calling backend endpoints.
- **RemoteIntelligenceService**: Implements `IIntelligenceService` using HTTP APIs for AI features (topic recommendations, content generation).

### Extensions (`Services`)
- **NotesServiceExtensions**: Helper methods for working with `INotesService` (e.g., saving a single notebook).
- **ChatClientExtensions**: Helper methods for interacting with chat-based AI clients.

### Components (`Components`)
- **TagInput**: Blazor component for editing a list of tags/topics, with support for suggestions and recommendations.
- **Pages**: UI pages for editing and viewing notes and notebooks:
  - `DeviceInfo.razor`: Shows device and render mode info.
  - `Edit.razor`: Example interactive page.
  - `NotebookEdit.razor`: Edit notebook details.
  - `NotebookNew.razor`: Create a new notebook.
  - `NotebookNotes.razor`: List notes in a notebook.
  - `NoteEdit.razor`: Edit a note, including AI-powered topic suggestions and content generation.

### Utilities
- **HybridRenderMode**: Utility for selecting the appropriate Blazor render mode (server, WASM, or auto) based on runtime support.

## How It Works

- **Data Flow**: UI components interact with service interfaces (`INotesService`, `IIntelligenceService`). These are typically implemented by remote HTTP services, but can be swapped for local or hybrid implementations in platform-specific projects.
- **AI Integration**: The `IIntelligenceService` interface and its remote implementation enable features like topic recommendations and content generation using backend AI models.
- **Component Reuse**: All Blazor components in this project are designed to be used in both web and native (MAUI) clients.

## Usage

- Reference this project from your Blazor or MAUI client projects.
- Register the appropriate service implementations (remote, local, or hybrid) in your client app's dependency injection setup.
- Use the provided components and models to build consistent, cross-platform UIs for notes, notebooks, and topics.

---

*This README was generated automatically to describe the structure and purpose of the Hybridisms.Shared project as of May 2025.*
