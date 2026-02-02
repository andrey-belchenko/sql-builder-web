---
name: VSCode Launch Configuration Setup
overview: Add a debugging configuration for Asuse.Ai.Reports to the existing .vscode/launch.json and a build task to tasks.json.
todos:
  - id: add-launch-config
    content: Add a new configuration to existing .vscode/launch.json for Asuse.Ai.Reports web app debugging
    status: completed
  - id: add-build-task
    content: Add a build task to existing .vscode/tasks.json for Asuse.Ai.Reports project
    status: completed
---

# VSCode Launch Configuration Setup

## Overview

Create a simple VSCode/Cursor debugging configuration for the `Asuse.Ai.Reports` ASP.NET Core project. This replicates Visual Studio's "startup project" functionality with a single, straightforward debug setup.

## Current State

- `.vscode/launch.json` exists with configurations for `SqlBuilder.App` (console app)
- `.vscode/tasks.json` exists with build tasks for `SqlBuilder.App`
- Project: ASP.NET Core 8.0 web application (`Asuse.Ai.Reports`)
- Launch profile: `http` (from `Properties/launchSettings.json`)
- Port: `http://localhost:5195`
- Launch URL: `http://localhost:5195/Report/Example`
- Environment: `Development`
- Solution file: `SqlBuilder.slnx` (contains multiple projects)

## Implementation

### 1. Update `.vscode/launch.json`

Add a new configuration entry for `Asuse.Ai.Reports`:

- **Name**: "Asuse.Ai.Reports" or ".NET Launch (web) - Asuse.Ai.Reports"
- **Type**: `coreclr` (for .NET debugging)
- **Request**: `launch`
- **Pre-launch task**: Build task for Asuse.Ai.Reports
- **Program**: Path to the compiled DLL: `Asuse.Ai.Reports/bin/Debug/net8.0/Asuse.Ai.Reports.dll`
- **Working directory**: `Asuse.Ai.Reports` folder
- **Environment variables**: `ASPNETCORE_ENVIRONMENT=Development`
- **Launch browser**: `true` with URL `http://localhost:5195/Report/Example`
- **Server ready action**: Configure to wait for the web server to be ready

### 2. Update `.vscode/tasks.json`

Add a new build task:

- **Label**: "build-asuse-reports" or similar
- **Command**: `dotnet build`
- **Args**: Path to `Asuse.Ai.Reports/Asuse.Ai.Reports.csproj`
- **Problem matcher**: `$msCompile`

## Files to Modify

- `net/.vscode/launch.json` - Add new configuration entry
- `net/.vscode/tasks.json` - Add new build task

## Usage After Setup

1. **Run/Debug**: Press `F5` and select "Asuse.Ai.Reports" from the debug configuration dropdown
2. **Build**: Use Command Palette (Ctrl+Shift+P) → "Tasks: Run Task" → select the Asuse.Ai.Reports build task
3. **Set Startup Project**: Select the Asuse.Ai.Reports configuration as the active debug configuration

## Notes

- The existing configurations for `SqlBuilder.App` will remain unchanged
- The web app configuration will launch a browser automatically when debugging starts
- The C# extension (ms-dotnettools.csharp) should already be installed based on extensions.json