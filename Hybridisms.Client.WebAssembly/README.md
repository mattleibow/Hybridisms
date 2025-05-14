# Hybridisms.Client.WebAssembly

This project is the Blazor WebAssembly client for the Hybridisms suite. It provides a browser-based app for managing notes, notebooks, and topics, with integrated AI features.

## Purpose
- Delivers a web app experience for Hybridisms in modern browsers.
- Connects to the Hybridisms server for data and AI features.

## Key Features
- **Blazor WebAssembly UI**: Runs entirely in the browser using WebAssembly.
- **Remote Services**: Uses HTTP APIs to interact with the backend for notes and intelligence.
- **Reusable Components**: Shares UI and logic with the native client via Hybridisms.Client.Shared.

## Structure
- **wwwroot/**: Static web assets.
- **_Imports.razor**: Common Razor imports for the client.
- **Program.cs**: Client startup and service registration.

## How It Works
- Registers remote service implementations for notes and intelligence.
- Uses Blazor WebAssembly for client-side UI and logic.

## Usage
- Build and run this project to launch the Hybridisms web app in your browser.
- Requires the Hybridisms server to be running for full functionality.

---
*This README was generated automatically to describe the structure and purpose of the Hybridisms.Client.WebAssembly project as of May 2025.*
