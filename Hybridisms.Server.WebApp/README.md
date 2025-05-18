# Hybridisms.Server.WebApp

## Overview
Hybridisms.Server.WebApp is an ASP.NET Core Web API and Blazor Server project that provides backend services for the Hybridisms demonstration app. It exposes RESTful endpoints and interactive Blazor components to support hybrid client applications, including web, native, and WebAssembly frontends.

## What the Project Does
- Hosts API endpoints for notes, topics, notebooks, and AI-powered intelligence features.
- Serves Blazor Server and WebAssembly components for interactive web UIs.
- Integrates with a local SQLite database for persistent storage.
- Provides AI-powered note content generation and topic recommendation via pluggable intelligence services.

## Implementation Architecture
- **Controllers**: Implements REST APIs for notes (`NotesController`), topics (`TopicsController`), notebooks (`NotebookController`), and AI features (`IntelligenceController`).
- **Blazor Components**: Supports both server-side and WebAssembly rendering for hybrid UI scenarios.
- **Dependency Injection**: Registers services for data access (`DbNotesService`), AI (`AdvancedIntelligenceService`), and database context (`HybridismsDbContext`).
- **Service Defaults**: Uses `AddServiceDefaults()` for consistent configuration across hybrid environments.
- **AI Integration**: Configures Azure OpenAI clients for chat and embedding scenarios.

## Hybrid App Enablement
This project is designed to be consumed by multiple types of clients:
- **WebAssembly/Blazor**: Exposes interactive components and APIs for browser-based hybrid apps.
- **Native Apps**: Provides REST endpoints for mobile/desktop clients to sync data and leverage server-side AI.
- **Service Discovery**: Uses service defaults and configuration to enable seamless communication in distributed/hybrid deployments.

## Example: AI-Powered Note Content Generation
The `IntelligenceController` exposes endpoints for hybrid clients to generate note content using AI:

```csharp
[HttpPost("generate-note-contents")]
public async Task<ActionResult> GenerateNoteContents([FromBody] string prompt)
{
    var response = await intelligenceService.GenerateNoteContentsAsync(prompt);
    return Ok(response);
}
```

## Example: Hybrid Razor Component Registration
The server registers both server and WebAssembly render modes for Blazor components:

```csharp
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Hybridisms.Hosting._Imports).Assembly)
    .AddAdditionalAssemblies(typeof(Hybridisms.Shared._Imports).Assembly)
    .AddAdditionalAssemblies(typeof(Hybridisms.Client.WebAssembly._Imports).Assembly);
```

## Summary
Hybridisms.Server.WebApp is the central backend for the Hybridisms solution, enabling hybrid scenarios by exposing APIs, interactive components, and AI services to a variety of client types.
