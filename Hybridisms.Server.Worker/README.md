# Hybridisms.Server.Worker

## Overview
Hybridisms.Server.Worker is a .NET Worker Service that seeds and migrates the database for the Hybridisms solution. It ensures the backend data store is ready for hybrid app scenarios.

## What the Project Does
- Runs as a background service to migrate and seed the database on startup.
- Ensures the database schema is up-to-date and initial data is present.
- Supports hybrid deployments by preparing the backend for all client types.

## Implementation Architecture
- **BackgroundService**: Implements a .NET BackgroundService for database migration and seeding.
- **Entity Framework Core**: Uses EF Core to manage database schema and seed data.
- **Logging**: Provides detailed logs for migration and seeding operations.

## Hybrid App Enablement
- **Automated Setup**: Ensures the backend is ready for hybrid clients (web, native, wasm) without manual intervention.
- **Consistent Data**: Seeds initial topics, notebooks, and notes for a unified hybrid experience.

## Example: Background Database Seeding
```csharp
protected override async Task ExecuteAsync(CancellationToken stoppingToken)
{
    // ...
    await dbContext.Database.EnsureCreatedAsync(stoppingToken);
    // ...
}
```

## Summary
Hybridisms.Server.Worker automates backend setup for hybrid apps, ensuring a consistent and ready-to-use data store for all scenarios.
