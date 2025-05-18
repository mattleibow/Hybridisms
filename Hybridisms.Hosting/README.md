# Hybridisms.Hosting

## Overview
Hybridisms.Hosting is a shared Blazor component library that provides common layouts, navigation, and UI elements for the Hybridisms hybrid app ecosystem. It enables a consistent look and feel across web, native, and WebAssembly clients.

## What the Project Does
- Supplies shared Blazor components for layouts, navigation menus, and pages.
- Centralizes UI styling and structure for all hybrid app frontends.
- Enables code and UI reuse across Blazor Server, WebAssembly, and MAUI Blazor Hybrid projects.

## Implementation Architecture
- **Blazor Components**: Includes layouts (e.g., `MainLayout`), navigation menus (`NavMenu`), and shared pages (e.g., `Home`).
- **Razor Imports**: Uses shared `_Imports.razor` for common namespaces and directives.
- **Routing**: Provides shared route definitions for hybrid navigation.
- **Styling**: Centralizes CSS and theming for a unified hybrid experience.

## Hybrid App Enablement
- **UI Consistency**: Ensures all hybrid clients share the same layouts and navigation, regardless of platform.
- **Component Reuse**: Allows Blazor Server, WASM, and MAUI Blazor Hybrid projects to reference and use the same UI components.
- **Hybrid Navigation**: Shared navigation logic and components work seamlessly across web and native hosts.

## Example: Using Shared Layout in a Hybrid App
```razor
@layout MainLayout
```

## Example: Shared Navigation Menu
```razor
<NavMenu />
```

## Summary
Hybridisms.Hosting provides the shared UI foundation for all hybrid app clients, enabling a consistent, reusable, and maintainable user experience across platforms.
