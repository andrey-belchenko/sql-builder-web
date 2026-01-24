# Cursor IDE Setup for .NET Development

## Important: Cursor IDE Limitations

Cursor IDE cannot use Microsoft's official C# debugger (`coreclr`) due to licensing restrictions. Microsoft's debugger is only available in official Microsoft VS Code distributions.

## Solution: Install Anysphere's C# Extension

### Step 1: Install the Extension

1. Open Cursor IDE
2. Press `Ctrl+Shift+X` (or `Cmd+Shift+X` on Mac) to open Extensions
3. Search for: `@id:anysphere.csharp`
4. Install **"C#" by Anysphere** (this is specifically made for Cursor IDE)
5. Reload Cursor IDE when prompted

### Step 2: Verify Installation

After installing the extension:
- Open any `.cs` file (e.g., `SqlBuilderApp/Program.cs`)
- You should see IntelliSense working (autocomplete, hover info)
- Code navigation (F12, Shift+F12) should work
- The debugger should now work with the `coreclr` type

### Step 3: Test Debugging

1. Open `SqlBuilderApp/Program.cs`
2. Set a breakpoint (click left of line number)
3. Press `F5` or go to Run and Debug
4. Select ".NET Core Launch (console)"
5. Debugging should start successfully

## Troubleshooting

### If IntelliSense Still Doesn't Work

1. Open Command Palette (`Ctrl+Shift+P` / `Cmd+Shift+P`)
2. Run: `OmniSharp: Restart OmniSharp`
3. Wait for OmniSharp to finish loading (check status bar)

### If Solution Not Detected

1. Open Command Palette (`Ctrl+Shift+P`)
2. Run: `Solution: Open Solution`
3. Select `SqlBuilder.slnx`

### If Debugging Still Fails

- Make sure you've installed `anysphere.csharp` (not just `ms-dotnettools.csharp`)
- Check that the project builds: `Ctrl+Shift+B`
- Verify `.NET 8 SDK` is installed: Run `dotnet --version` in terminal

## Alternative Extensions

If Anysphere's extension doesn't work, you can try:
- **DotRush** (`@id:owen-dotrush`) - Alternative C# support for Cursor

## Notes

- The `externalTerminal` console setting in `launch.json` is recommended for Cursor IDE
- IntelliSense uses OmniSharp, which should work with the Anysphere extension
- Build tasks (`Ctrl+Shift+B`) work regardless of extension installation
