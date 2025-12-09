# mcp-windows Development Guidelines

Auto-generated from all feature plans. Last updated: 2025-12-07

## Active Technologies
- C# 12+ (.NET 8.0 LTS) + Microsoft.Extensions.Logging, Serilog, MCP SDK, System.CommandLine (002-keyboard-control)
- N/A (stateless operations, held keys tracked in memory only) (002-keyboard-control)
- C# 12+ / .NET 8.0 LTS + MCP C# SDK, Microsoft.Extensions.Logging, Serilog (003-window-management)
- N/A (stateless window queries) (003-window-management)
- TypeScript 5.9+ (extension), C# 12+ (.NET 8.0 for bundled server) + VS Code Extension API 1.106.0+, .NET Install Tool extension (006-vscode-extension)
- C# 12+ (.NET 8.0 LTS) + Microsoft.Extensions.Logging, System.Drawing, existing MCP tools (mouse_control, keyboard_control, window_management, screenshot_control) (007-llm-integration-testing)
- File-based (PNG images, JSON metadata, Markdown scenarios) (007-llm-integration-testing)

- C# 12+ (latest stable per Constitution XIII) (001-mouse-control)

## Project Structure

```text
src/
tests/
```

## Commands

# Add commands for C# 12+ (latest stable per Constitution XIII)

## Code Style

C# 12+ (latest stable per Constitution XIII): Follow standard conventions

## Recent Changes
- 007-llm-integration-testing: Added C# 12+ (.NET 8.0 LTS) + Microsoft.Extensions.Logging, System.Drawing, existing MCP tools (mouse_control, keyboard_control, window_management, screenshot_control)
- 006-vscode-extension: Added TypeScript 5.9+ (extension), C# 12+ (.NET 8.0 for bundled server) + VS Code Extension API 1.106.0+, .NET Install Tool extension
- 003-window-management: Added C# 12+ / .NET 8.0 LTS + MCP C# SDK, Microsoft.Extensions.Logging, Serilog


<!-- MANUAL ADDITIONS START -->
<!-- MANUAL ADDITIONS END -->
