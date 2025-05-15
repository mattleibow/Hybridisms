# Hybridisms.Server

This project contains the core backend services and data models for the Hybridisms suite. It provides the main data access layer and business logic for notes, notebooks, and topics, and is used by the server-side web application.

## Purpose
- Implements the main data access and business logic for notes, notebooks, and topics.
- Provides Entity Framework Core models and context for persistent storage.
- Supplies services for CRUD operations and AI-powered features.

## Structure
- **Data/**: Entity Framework Core context and entity models (`HybridismsDbContext`, `NotebookEntity`, `NoteEntity`, `TopicEntity`).
- **Services/**: Business logic for notes, notebooks, topics, and advanced intelligence features (`DbNotesService`, `AdvancedIntelligenceService`).

## Usage
- Used as a library by `Hybridisms.Server.WebApp` to provide backend functionality.
- Not intended to be run directly; reference from the main server project.

---
*This README was generated automatically to describe the structure and purpose of the Hybridisms.Server project as of May 2025.*
