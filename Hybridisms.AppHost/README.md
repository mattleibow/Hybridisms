# Hybridisms.AppHost

## Overview
Hybridisms.AppHost is the orchestration and entry point for the Hybridisms demonstration app, built using Aspire. It provisions and configures all resources and services required for running the solution in a distributed, hybrid environment.

## What the Project Does
- Orchestrates the deployment of backend, web, mobile, and WebAssembly projects.
- Provisions Azure OpenAI resources for AI-powered features.
- Sets up a SQLite database and supporting infrastructure.
- Groups and manages resources for logical separation (data, apps, AI).
- Ensures all hybrid app components are connected and discoverable.

## Implementation Architecture
- **Aspire Distributed Application**: Uses Aspire's builder API to define and wire up all resources and projects.
- **Resource Provisioning**: Provisions Azure OpenAI (with custom SKU), SQLite, and supporting services.
- **Project Grouping**: Organizes resources into logical groups (e.g., `data`, `apps`) for clarity and dependency management.
- **Hybrid App Registration**: Registers web, mobile (MAUI), and WebAssembly projects, ensuring they reference the correct backend and AI resources.
- **Custom Extensions**: Provides helpers for adding MAUI and WASM projects as stubs for hybrid scenarios.

## Hybrid App Enablement
- **Unified Orchestration**: All hybrid app components (web, native, wasm) are managed and launched together.
- **Service Discovery**: Ensures all services are discoverable and properly referenced for hybrid communication.
- **AI and Data Integration**: Centralizes configuration for AI and data resources, making them available to all hybrid clients.

## Example: Adding a MAUI Hybrid Client
```csharp
builder.AddMauiProject("mobile", "Hybridisms.Client.NativeApp")
    .InGroup(appsGroup)
    .WithReference(web)
    .ExcludeFromManifest();
```

## Example: Provisioning Azure OpenAI
```csharp
var ai = builder.AddAzureOpenAI("ai")
    .ConfigureInfrastructure(infra =>
    {
        var resources = infra.GetProvisionableResources();
        var account = resources.OfType<CognitiveServicesAccountDeployment>().Single();
        account.Sku.Name = "GlobalStandard";
    });
ai.AddDeployment(
    name: "ai-model",
    modelName: "gpt-4o-mini",
    modelVersion: "2024-07-18");
```

## Summary
Hybridisms.AppHost is the backbone of the hybrid solution, orchestrating all resources and projects for a seamless, distributed hybrid app experience.
