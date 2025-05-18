# Hybridisms.Server

## Overview
Hybridisms.Server is a backend library that provides data access and advanced AI services for the Hybridisms demonstration app. It implements persistent storage, entity mapping, and server-side intelligence features for hybrid scenarios.

## What the Project Does
- Implements Entity Framework Core data models and DbContext for notes, notebooks, and topics.
- Provides server-side implementations of notes and intelligence services.
- Integrates with AI services (e.g., Azure OpenAI) for advanced topic recommendations and note content generation.
- Maps between database entities and shared models for seamless hybrid data flow.

## Implementation Architecture
- **Entity Framework Core**: Defines `HybridismsDbContext` and entity classes for persistent storage.
- **DbNotesService**: Implements `INotesService` for CRUD operations on notes, notebooks, and topics.
- **AdvancedIntelligenceService**: Implements `IIntelligenceService` using AI chat clients for recommendations and content generation.
- **Entity Mapping**: Maps between database entities and shared models for hybrid compatibility.

## Hybrid App Enablement
- **Shared Contracts**: Uses shared models and interfaces for interoperability with all hybrid clients.
- **AI Integration**: Exposes advanced AI features to hybrid clients via shared service interfaces.
- **Data Synchronization**: Supports hybrid data flows between local/native and remote/server storage.

## Example: AI-Powered Topic Recommendation
```csharp
public async Task<ICollection<TopicRecommendation>> RecommendTopicsAsync(Note note, int count = 3, CancellationToken cancellationToken = default)
{
    // ...
    var selectedLabels = await chatClient.GetResponseAsync<List<SelectedLabel>>(systemPrompt, prompt, null, cancellationToken);
    // ...
}
```

## Example: Entity Framework Core DbContext
```csharp
public class HybridismsDbContext(DbContextOptions<HybridismsDbContext> options) : DbContext(options)
{
    public DbSet<NoteEntity> Notes => Set<NoteEntity>();
    public DbSet<TopicEntity> Topics => Set<TopicEntity>();
    public DbSet<NotebookEntity> Notebooks => Set<NotebookEntity>();
    // ...
}
```

## Summary
Hybridisms.Server provides the backend data and AI services for the hybrid solution, enabling persistent storage, advanced intelligence, and seamless data flow for all hybrid app clients.
