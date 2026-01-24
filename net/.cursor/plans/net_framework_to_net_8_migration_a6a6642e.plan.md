---
name: NET Framework to NET 8 Migration
overview: Migrate the .NET Framework 4.5.1 solution (sql.builder) to .NET 8, consolidating all code into SqlBuilderLib library project and updating SqlBuilderApp to reference it.
todos:
  - id: update_lib_project
    content: "Update SqlBuilderLib.csproj: Convert to SDK-style, set net8.0, add package references (Newtonsoft.Json, DotNetZip), handle Devart.Data.Oracle dependency"
    status: completed
  - id: copy_source_files
    content: Copy all .cs files from net-framework/sql.builder to net/SqlBuilderLib preserving directory structure
    status: in_progress
  - id: copy_resources
    content: Copy embedded resources (icons, SQL files, DDL scripts) to SqlBuilderLib
    status: pending
  - id: update_namespaces
    content: Keep existing namespaces (sql.builder) unless conflicts occur, preserve all using statements
    status: completed
  - id: migrate_config
    content: Migrate app.config settings to .NET 8 configuration (appsettings.json or code-based)
    status: completed
  - id: handle_devart
    content: Replace Devart.Data.Oracle local DLL with NuGet package (version 10.3+ supports .NET 8)
    status: pending
  - id: handle_ionic_zip
    content: Replace Ionic.Zip with System.IO.Compression.ZipArchive in ExcelPrintEnv.cs (code changes required)
    status: pending
  - id: handle_system_web
    content: Replace JavaScriptSerializer with Newtonsoft.Json in DataExtractHelper.cs
    status: pending
  - id: update_solution
    content: Update SqlBuilder.slnx to properly reference SqlBuilderLib project
    status: completed
  - id: update_app
    content: Update SqlBuilderApp Program.cs to use migrated code from SqlBuilderLib
    status: completed
  - id: build_test
    content: Build solution and fix only necessary compilation errors (minimal code changes)
    status: in_progress
isProject: false
---

# .NET Framework to .NET 8 Migration Plan

## Overview

Migrate the `sql.builder` project from .NET Framework 4.5.1 to .NET 8, organizing all code into `SqlBuilderLib` library and updating `SqlBuilderApp` to reference it.

## Migration Philosophy

**Minimal Changes Approach**: Only modify code when absolutely necessary for .NET 8 compatibility. Preserve existing:

- C# language version and syntax (keep C# 6 unless compilation requires upgrade)
- Namespaces (keep `sql.builder` unless conflicts occur)
- Code patterns and style
- API usage (unless APIs are removed/deprecated in .NET 8)

**Change Only When**:

- Code does not compile due to .NET 8 incompatibilities
- Required dependencies are unavailable
- Build errors occur that prevent successful compilation

## Dependency Research Summary

**Non-Microsoft Libraries Migration Status:**

| Library | Status | Action Required | Code Changes |
|---------|--------|----------------|--------------|
| **Devart.Data.Oracle** | ✅ .NET 8 Compatible | Replace local DLL with NuGet (v10.3+) | None - API compatible |
| **Ionic.Zip** | ⚠️ Deprecated | Replace with System.IO.Compression | Yes - Update ExcelPrintEnv.cs |
| **Newtonsoft.Json** | ✅ Compatible | Keep existing version (13.0.3) | None |
| **System.Web.Extensions** | ❌ Not Available | Replace JavaScriptSerializer | Yes - Update DataExtractHelper.cs |

**Key Findings:**

- Devart.Data.Oracle has official .NET 8 support (version 10.3+) - no code changes needed
- Ionic.Zip is deprecated; replace with built-in `System.IO.Compression.ZipArchive` (requires code changes)
- System.Web.Script.Serialization needs replacement with Newtonsoft.Json (already a dependency)

## Current State Analysis

**Source Project:**

- Location: `C:\Repos\github\sql-builder-web\net-framework\sql.builder\`
- Target Framework: .NET Framework 4.5.1
- Project Type: WinExe (Windows Forms executable)
- Files: 229 C# files
- Key Dependencies:
- Devart.Data.Oracle (v8.5.583.0) - Oracle database provider - **Will migrate to NuGet v10.3+**
- Ionic.Zip.dll (local DLL) - **Will replace with System.IO.Compression**
- Newtonsoft.Json (v13.0.3) - **Keep as-is**
- System.Drawing, System.Windows.Forms (mostly commented out)

**Target Structure:**

- `SqlBuilderLib`: Library project (net8.0) - will contain all migrated code
- `SqlBuilderApp`: Executable project (net8.0) - references SqlBuilderLib

## Migration Steps

### 1. Update SqlBuilderLib Project File

- Convert to SDK-style project format
- Set TargetFramework to `net8.0`
- Add UseWindowsForms if needed for System.Drawing
- Keep LangVersion at 6 (or current version) - only change if compilation requires it
- Add package references:
- Newtonsoft.Json (version 13.0.3 - already in packages.config)
- Devart.Data.Oracle (version 10.3+ - .NET 8 compatible, replace local DLL)
- **Note**: Ionic.Zip will be replaced with System.IO.Compression (built-in, no package needed)

### 2. Copy Source Files

- Copy all `.cs` files from `net-framework\sql.builder\` to `net\SqlBuilderLib\`
- Preserve directory structure (DataApi, Core, UI, Print, etc.)
- Copy SQL files from `Sql\` directory
- Copy embedded resources (icons, licenses.licx)
- Copy DDL scripts if needed

### 3. Handle Dependencies

#### 3.1 Devart.Data.Oracle (RESEARCHED)

- **Status**: ✅ **.NET 8 COMPATIBLE**
- **Action**: Use NuGet package `Devart.Data.Oracle` version 10.3+ (supports .NET 8)
- **Package**: `Install-Package Devart.Data.Oracle` or via NuGet UI
- **Current Version**: 8.5.583.0 (local DLL reference)
- **Migration**: Replace local DLL reference with NuGet package reference
- **Note**: No code changes needed - API compatible, just update package reference

#### 3.2 Ionic.Zip (RESEARCHED)

- **Status**: ⚠️ **DEPRECATED** - Need replacement
- **Current Usage**: Used in `ExcelPrintEnv.cs` for:
- `ZipFile.Read()` - reading ZIP files
- `ZipFile()` constructor - creating ZIP files
- `ExtractAll()` - extracting ZIP archives
- **No Password Protection**: Code doesn't use password-protected ZIPs
- **Options**:

1. **System.IO.Compression.ZipArchive** (Recommended for .NET 8)

- Built-in, no external dependency
- Supports basic ZIP operations (read/write/extract)
- **Limitation**: No password protection (not needed here)
- **Code Changes Required**: Update `ExcelPrintEnv.cs` to use `ZipArchive` instead of `Ionic.Zip.ZipFile`

2. **DotNetZip.Semverd** (Community fork, archived May 2024)

- Not recommended - project archived
- **Action**: Replace with `System.IO.Compression.ZipArchive` - requires code changes in `ExcelPrintEnv.cs`

#### 3.3 System.Web / System.Web.Extensions (RESEARCHED)

- **Status**: ⚠️ **NOT AVAILABLE IN .NET 8**
- **Current Usage**:

1. `System.Web.Script.Serialization.JavaScriptSerializer` in `DataExtractHelper.cs`

- Used for JSON deserialization
- **Replacement**: Use `Newtonsoft.Json` (already a dependency)
- **Code Changes**: Replace `JavaScriptSerializer().DeserializeObject()` with `JsonConvert.DeserializeObject()`

2. `System.Web.Services` in `CodeGenerationUtils.LKK.cs`

- Used in **code generation strings only** (not runtime)
- Generates code that includes `using System.Web.Services;`
- **Action**: Keep as-is (it's just a string template for generated code)
- **Action**: Replace `JavaScriptSerializer` with `Newtonsoft.Json` in `DataExtractHelper.cs`

#### 3.4 System.Drawing

- **Status**: ✅ Available in .NET 8
- **Action**: Add `<UseWindowsForms>true</UseWindowsForms>` to project file if needed

#### 3.5 System.Configuration

- **Status**: ⚠️ Different API in .NET 8
- **Action**: Migrate app.config settings to appsettings.json or code-based configuration (only if needed)

### 4. Update Namespaces

- Keep existing namespace `sql.builder` unless it causes conflicts
- Only update namespaces if absolutely necessary for compilation
- Preserve all existing using statements

### 5. Migrate Configuration

- Convert `app.config` to `appsettings.json` (if needed)
- Migrate user settings from app.config to appropriate .NET 8 configuration system
- Handle connection strings appropriately

### 6. Update Solution File

- Update `SqlBuilder.slnx` to reference SqlBuilderLib properly
- Ensure SqlBuilderApp references SqlBuilderLib

### 7. Code Compatibility Updates

- **Only fix compilation errors** - do not proactively modernize code
- Review and update any .NET Framework-specific APIs that cause compilation errors:
- `System.Web` references (if any) - only change if compilation fails
- `System.ServiceModel` - available in .NET 8, should work as-is
- Update deprecated APIs only if they cause build errors
- **Do NOT enable nullable reference types** - keep existing code style
- **Do NOT upgrade C# language features** - keep C# 6 syntax unless required

### 8. Update SqlBuilderApp

- Update Program.cs to use migrated code from SqlBuilderLib
- Ensure proper entry point configuration

### 9. Build and Test

- Build the solution
- Address compilation errors
- Test basic functionality

## Files to Modify

1. `net\SqlBuilderLib\SqlBuilderLib.csproj` - Convert to SDK-style, add dependencies
2. `net\SqlBuilderApp\SqlBuilderApp.csproj` - Ensure reference to SqlBuilderLib
3. `net\SqlBuilder.slnx` - Update solution structure
4. All source files - Copy from net-framework, update namespaces if needed

## Potential Challenges

1. **Ionic.Zip Migration**: 

- Requires code changes in `ExcelPrintEnv.cs` to use `System.IO.Compression.ZipArchive`
- API differences: `ZipFile.Read()` → `ZipFile.OpenRead()`, `new ZipFile()` → `new ZipArchive()`
- Need to test Excel file generation functionality after migration

2. **System.Web.Script.Serialization Replacement**:

- Replace `JavaScriptSerializer` with `Newtonsoft.Json` in `DataExtractHelper.cs`
- Simple replacement: `new JavaScriptSerializer().DeserializeObject()` → `JsonConvert.DeserializeObject()`

3. **WinForms Dependencies**: Most WinForms code is commented out, but System.Drawing is used. May need `<UseWindowsForms>true</UseWindowsForms>`.

4. **Configuration Migration**: app.config contains custom sections and user settings that need proper migration.

5. **Build Configurations**: Source has multiple build configurations (Academic, Basic, Community, etc.). May need to simplify or preserve as needed.

## Automation Script Considerations

If automation is needed:

- C# script to copy files preserving directory structure
- PowerShell script to update project references
- Script to analyze and report incompatible APIs

## Notes

- The current Program.cs shows it's primarily a console application now (WinForms mostly commented out)
- Most System.Windows.Forms references are commented out, suggesting UI migration already in progress
- **Migration Philosophy**: Only change code when necessary for .NET 8 compatibility. Keep existing C# 6 syntax, namespaces, and code patterns unless they cause compilation errors.