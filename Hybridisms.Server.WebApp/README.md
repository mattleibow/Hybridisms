# Hybridisms.Server.WebApp

This project is the main server-side web application for the Hybridisms suite. It provides the backend API, database access, and server-side Blazor components for managing notes, notebooks, and topics, as well as integrating AI-powered features.

## Purpose
- Hosts the main HTTP API for notes, notebooks, topics, and AI features.
- Serves Blazor server and WebAssembly clients.
- Manages persistent data storage using Entity Framework Core and SQLite.
- Integrates with Azure OpenAI for advanced intelligence features.

## Key Features
- **API Controllers**: Endpoints for CRUD operations on notes, notebooks, and topics, and for AI-powered features.
- **Blazor Components**: Server-side UI components for interactive web experiences.
- **Database Integration**: Uses `HybridismsDbContext` for data persistence.
- **Service Registration**: Registers local and remote services for notes and intelligence.
- **OpenAI Integration**: Connects to Azure OpenAI for chat and topic recommendation features.

## Structure
- **Controllers/**: API endpoints for notes, topics, and intelligence.
- **Data/**: Entity Framework Core context and data models for persistent storage.
- **Services/**: Business logic for notes, topics, and AI features.
- **Components/**: Blazor server-side UI components.
- **wwwroot/**: Static web assets.

## How It Works
- The app starts by applying database migrations and seeding initial data.
- API controllers expose endpoints for CRUD and AI operations.
- Blazor components provide interactive UI for web clients.
- AI features are powered by Azure OpenAI and exposed via the intelligence service.

## Usage
- Run this project to start the main backend and web UI for Hybridisms.
- Connect Blazor WebAssembly or MAUI clients to this server for full functionality.

---
*This README was generated automatically to describe the structure and purpose of the Hybridisms.Server.WebApp project as of May 2025.*
