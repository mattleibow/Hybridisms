# Hybridisms

Hybridisms is a distributed, cross-platform note-taking and knowledge management solution that leverages .NET Aspire, Blazor, .NET MAUI, and Azure OpenAI to deliver a seamless experience across web, desktop, and mobile. The solution is designed for hybrid operation—supporting both offline and cloud-connected scenarios, with integrated AI features for content generation and topic recommendations.

## Solution Overview

The Hybridisms solution is composed of several projects, each with a specific role:

### 1. **Hybridisms.AppHost** ([README](Hybridisms.AppHost/README.md))
- **Purpose:** The main entry point and orchestrator for the solution, using .NET Aspire to coordinate all services and clients.
- **Role:** Starts and configures the backend server, web client, and native/mobile clients. Manages distributed application settings and cloud resources (e.g., Azure OpenAI).

### 2. **Hybridisms.Server.WebApp** ([README](Hybridisms.Server.WebApp/README.md))
- **Purpose:** The main backend server and web application.
- **Role:**
  - Hosts the HTTP API for notes, notebooks, topics, and AI features.
  - Serves Blazor Server and WebAssembly clients.
  - Manages persistent data storage using Entity Framework Core and SQLite.
  - Integrates with Azure OpenAI for advanced intelligence features.
  - Provides Blazor server-side UI components.

### 3. **Hybridisms.Client.WebAssembly** ([README](Hybridisms.Client.WebAssembly/README.md))
- **Purpose:** The Blazor WebAssembly (WASM) client for browser-based access.
- **Role:**
  - Runs entirely in the browser using WebAssembly.
  - Connects to the backend server for data and AI features.
  - Shares UI and logic with the native client via `Hybridisms.Client.Shared`.

### 4. **Hybridisms.Client.NativeApp** ([README](Hybridisms.Client.NativeApp/README.md))
- **Purpose:** The .NET MAUI (native) client for cross-platform mobile and desktop use.
- **Role:**
  - Delivers a native app experience on Windows, macOS, Android, and iOS.
  - Supports offline data storage and hybrid cloud/local operation.
  - Integrates ONNX models for local AI features when available.
  - Shares UI and logic with the web client via `Hybridisms.Client.Shared`.

### 5. **Hybridisms.Client.Shared** ([README](Hybridisms.Client.Shared/README.md))
- **Purpose:** Shared business logic, data models, and Blazor UI components for both web and native clients.
- **Role:**
  - Defines models for notes, notebooks, and topics.
  - Provides service interfaces and remote/local implementations.
  - Supplies reusable Blazor components and utilities for hybrid rendering.

### 6. **Hybridisms.Server.Shared** ([README](Hybridisms.Server.Shared/README.md))
- **Purpose:** Shared code, components, and resources for server-side projects.
- **Role:**
  - Provides reusable Blazor components and shared logic for server-side applications.

### 7. **ServiceDefaults Projects**
- **Hybridisms.Server.Web.ServiceDefaults** ([README](Hybridisms.Server.Web.ServiceDefaults/README.md)): Shared service configuration and extension methods for server-side projects (e.g., service discovery, resilience, health checks, OpenTelemetry).
- **Hybridisms.Client.Native.ServiceDefaults** ([README](Hybridisms.Client.Native.ServiceDefaults/README.md)): Shared service configuration for MAUI (native) clients.
- **Hybridisms.Client.WebAssembly.ServiceDefaults** ([README](Hybridisms.Client.WebAssembly.ServiceDefaults/README.md)): Shared service configuration for Blazor WebAssembly clients.

### 8. **ClientStub Projects**
- **Hybridisms.Client.NativeApp.ClientStub** ([README](Hybridisms.Client.NativeApp.ClientStub/README.md)): Generates strongly-typed settings from Aspire environment variables for the native client.
- **Hybridisms.Client.WebAssembly.ClientStub** ([README](Hybridisms.Client.WebAssembly.ClientStub/README.md)): Generates strongly-typed settings for the WASM client.

## How the Projects Fit Together

- **AppHost** launches and coordinates the server, web, and native/mobile clients, ensuring all services are configured and connected.
- **Server.WebApp** provides the backend API, database, and server-side Blazor UI, and exposes AI features via Azure OpenAI.
- **Client.WebAssembly** and **Client.NativeApp** are the main user-facing apps, sharing UI and logic via **Client.Shared**.
- **ServiceDefaults** projects ensure consistent service configuration and observability across all app types.
- **ClientStub** projects automate configuration for client apps based on the Aspire environment.
- **Server.Shared** and **Client.Shared** maximize code reuse and consistency across the solution.

## Typical Usage

- **Developers** run `Hybridisms.AppHost` to start the entire solution for local development or deployment.
- **End users** interact with the system via the web app (browser) or the native app (desktop/mobile), both offering similar features and UI.
- **AI features** (topic recommendations, content generation) are available in both clients, powered by Azure OpenAI (cloud) or ONNX (local, in the native app).

---
*This README was generated to describe the structure and integration of the Hybridisms solution as of May 2025.*