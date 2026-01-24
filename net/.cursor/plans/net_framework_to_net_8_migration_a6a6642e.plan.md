---
name: NET Framework to NET 8 Migration
overview: Migrate the .NET Framework 4.5.1 solution (sql.builder) to .NET 8, consolidating all code into SqlBuilderLib library project and updating SqlBuilderApp to reference it.
todos:
  - id: update_lib_project_phase1
    content: "Update SqlBuilderLib.csproj: Keep SDK-style format, ensure net8.0 target (minimal setup, no dependencies yet)"
    status: pending
  - id: copy_source_files
    content: Copy all .cs files from net-framework/sql.builder to net/SqlBuilderLib preserving directory structure exactly
    status: completed
  - id: copy_resources
    content: Copy embedded resources (icons, SQL files, DDL scripts, licenses.licx) to SqlBuilderLib
    status: completed
  - id: update_solution_phase1
    content: Update SqlBuilder.slnx to properly reference SqlBuilderLib project
    status: pending
  - id: commit_phase1
    content: "COMMIT: Migrate files from .NET Framework to .NET 8 project structure (no code changes)"
    status: pending
  - id: update_lib_project_phase2
    content: "Update SqlBuilderLib.csproj: Add package references (Newtonsoft.Json 13.0.3, Devart.Data.Oracle 10.3+), add UseWindowsForms if needed"
    status: pending
  - id: handle_devart
    content: Replace Devart.Data.Oracle local DLL reference with NuGet package (version 10.3+ supports .NET 8)
    status: completed
  - id: handle_ionic_zip
    content: Replace Ionic.Zip with System.IO.Compression.ZipArchive in ExcelPrintEnv.cs (lines 13, 57, 324) - code changes required
    status: pending
  - id: handle_system_web
    content: Replace JavaScriptSerializer with Newtonsoft.Json in DataExtractHelper.cs (lines 163, 200) - add using Newtonsoft.Json
    status: pending
  - id: update_app
    content: Update SqlBuilderApp Program.cs to use migrated code from SqlBuilderLib (replace placeholder Greeter usage)
    status: completed
  - id: migrate_config
    content: Migrate app.config settings to .NET 8 configuration (appsettings.json or code-based) if needed
    status: completed
  - id: build_test
    content: Build solution and fix only necessary compilation errors (minimal code changes, preserve C# 6 syntax)
    status: completed
  - id: commit_phase2
    content: "COMMIT: Update code for .NET 8 compatibility"
    status: pending
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
- Ionic.Zip.dll (local DLL in `dll\` folder) - **Will replace with System.IO.Compression**
- Newtonsoft.Json (v13.0.3) - **Keep as-is**
- System.Drawing, System.Windows.Forms (mostly commented out)

**Target Structure (Current State - Rolled Back):**

- `SqlBuilderLib`: Basic SDK-style project (net8.0) - currently only contains placeholder `Greeter.cs`
- `SqlBuilderApp`: Basic executable project (net8.0) - references SqlBuilderLib, uses placeholder Greeter
- **Status**: No migration work completed - all source files remain in `net-framework\sql.builder\`

**Migration Status:**

- ✅ Project structure exists (SqlBuilderLib, SqlBuilderApp)
- ❌ No source files copied yet
- ❌ No dependencies configured yet
- ❌ No code compatibility updates done yet

## Migration Workflow

**Two-Phase Approach:**

1. **Phase 1**: Copy all files to new project (NO code changes) → **COMMIT**
2. **Phase 2**: Make code changes for .NET 8 compatibility → **COMMIT**

This allows clear separation between file migration and code changes for easier review.

## Migration Steps

### Phase 1: File Migration (No Code Changes)

#### 1.1 Update SqlBuilderLib Project File (Minimal Setup)

- Keep SDK-style project format (already done)
- Set TargetFramework to `net8.0` (already done)
- **DO NOT add dependencies yet** - will add in Phase 2
- **DO NOT add UseWindowsForms yet** - will add if needed in Phase 2
- Keep LangVersion at 6 (or current version) - only change if compilation requires it

#### 1.2 Copy Source Files (Preserve As-Is)

- Copy all `.cs` files from `net-framework\sql.builder\` to `net\SqlBuilderLib\`
- Preserve directory structure exactly (DataApi, Core, UI, Print, etc.)
- Copy SQL files from `Sql\` directory
- Copy embedded resources (icons, licenses.licx)
- Copy DDL scripts if needed
- **Important**: Copy files exactly as-is - no modifications, no namespace changes, no code updates

#### 1.3 Update Solution File

- Update `SqlBuilder.slnx` to reference SqlBuilderLib properly
- Ensure SqlBuilderApp references SqlBuilderLib

#### 1.4 Commit Point

- **COMMIT**: "Migrate files from .NET Framework to .NET 8 project structure"
- This commit contains only file copies with no code changes

### Phase 2: Code Compatibility Updates

#### 2.1 Update SqlBuilderLib Project File (Add Dependencies)

- Add package references:
- Newtonsoft.Json (version 13.0.3 - already in packages.config)
- Devart.Data.Oracle (version 10.3+ - .NET 8 compatible, replace local DLL)
- Add `<UseWindowsForms>true</UseWindowsForms>` if needed for System.Drawing
- **Note**: Ionic.Zip will be replaced with System.IO.Compression (built-in, no package needed)

#### 2.2 Handle Dependencies

#### 3.1 Devart.Data.Oracle (RESEARCHED)

- **Status**: ✅ **.NET 8 COMPATIBLE**
- **Action**: Use NuGet package `Devart.Data.Oracle` version 10.3+ (supports .NET 8)
- **Package**: `Install-Package Devart.Data.Oracle` or via NuGet UI
- **Current Version**: 8.5.583.0 (local DLL reference)
- **Migration**: Replace local DLL reference with NuGet package reference
- **Note**: No code changes needed - API compatible, just update package reference

#### 3.2 Ionic.Zip (RESEARCHED)

- **Status**: ⚠️ **DEPRECATED** - Need replacement
- **Current Usage**: Used in `Print\Xlsx\ExcelPrintEnv.cs`:
- Line 13: `using Ionic.Zip;`
- Line 57: `using (ZipFile xlsx = ZipFile.Read(template_path))`
- Line 324: `using (ZipFile zip = new ZipFile())`
- **Operations**: Reading ZIP files, extracting archives, creating ZIP files
- **No Password Protection**: Code doesn't use password-protected ZIPs
- **Replacement**: Use `System.IO.Compression.ZipArchive` (built-in, no external dependency)
- **Code Changes Required**: 
- Replace `using Ionic.Zip;` with `using System.IO.Compression;`
- Replace `ZipFile.Read()` with `ZipFile.OpenRead()`
- Replace `new ZipFile()` with `new ZipArchive()`
- Replace `ExtractAll()` with manual extraction using `ZipArchive.Entries`

#### 3.3 System.Web / System.Web.Extensions (RESEARCHED)

- **Status**: ⚠️ **NOT AVAILABLE IN .NET 8**
- **Current Usage**:

1. `System.Web.Script.Serialization.JavaScriptSerializer` in `DashboardUtils\DataExtractHelper.cs`

- Used for JSON deserialization (lines 163 and 200)
- **Replacement**: Use `Newtonsoft.Json` (already a dependency in packages.config)
- **Code Changes**: 
- Add `using Newtonsoft.Json;`
- Replace `new JavaScriptSerializer().DeserializeObject()` with `JsonConvert.DeserializeObject()`
- Both usages are in `#if DEBUG` blocks

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

#### 2.3 Update Namespaces

- Keep existing namespace `sql.builder` unless it causes conflicts
- Only update namespaces if absolutely necessary for compilation
- Preserve all existing using statements

#### 2.4 Migrate Configuration

- Convert `app.config` to `appsettings.json` (if needed)
- Migrate user settings from app.config to appropriate .NET 8 configuration system
- Handle connection strings appropriately

#### 2.5 Code Compatibility Updates

- **Only fix compilation errors** - do not proactively modernize code
- Review and update any .NET Framework-specific APIs that cause compilation errors:
- `System.Web` references (if any) - only change if compilation fails
- `System.ServiceModel` / WCF - if causes errors, comment out rather than fix (WCF functionality probably not needed)
- Update deprecated APIs only if they cause build errors
- **UI Code**: If UI-related code (System.Windows.Forms, System.Drawing UI components) causes compilation errors, comment it out rather than fixing it, as the application is primarily a console application now
- **WCF Code**: If WCF-related code (`WCFHelper.cs`, `System.ServiceModel` references) causes compilation errors, comment it out rather than fixing it, as WCF functionality is probably not needed
- **Do NOT enable nullable reference types** - keep existing code style
- **Do NOT upgrade C# language features** - keep C# 6 syntax unless required

#### 2.6 Update SqlBuilderApp

- Update Program.cs to use migrated code from SqlBuilderLib (replace placeholder Greeter)
- Ensure proper entry point configuration

#### 2.7 Build and Test

- Build the solution
- Address compilation errors
- Test basic functionality

#### 2.8 Commit Point

- **COMMIT**: "Update code for .NET 8 compatibility"
- This commit contains all code changes for .NET 8 compatibility

## Files to Modify

### Phase 1 (File Migration - No Code Changes):

1. `net\SqlBuilderLib\SqlBuilderLib.csproj` - Minimal setup (keep SDK-style, ensure net8.0)
2. `net\SqlBuilder.slnx` - Ensure SqlBuilderLib is properly referenced
3. All source files - Copy from `net-framework\sql.builder\` to `net\SqlBuilderLib\` preserving directory structure exactly

### Phase 2 (Code Compatibility Updates):

1. `net\SqlBuilderLib\SqlBuilderLib.csproj` - Add dependencies (Newtonsoft.Json 13.0.3, Devart.Data.Oracle 10.3+), add UseWindowsForms if needed
2. `net\SqlBuilderLib\Print\Xlsx\ExcelPrintEnv.cs` - Replace Ionic.Zip with System.IO.Compression (3 locations)
3. `net\SqlBuilderLib\DashboardUtils\DataExtractHelper.cs` - Replace JavaScriptSerializer with Newtonsoft.Json (2 locations)
4. `net\SqlBuilderApp\Program.cs` - Update to use migrated code (currently uses placeholder Greeter)

## Potential Challenges

1. **Ionic.Zip Migration**: 

- Requires code changes in `ExcelPrintEnv.cs` to use `System.IO.Compression.ZipArchive`
- API differences: `ZipFile.Read()` → `ZipFile.OpenRead()`, `new ZipFile()` → `new ZipArchive()`
- Need to test Excel file generation functionality after migration

2. **System.Web.Script.Serialization Replacement**:

- Replace `JavaScriptSerializer` with `Newtonsoft.Json` in `DataExtractHelper.cs`
- Simple replacement: `new JavaScriptSerializer().DeserializeObject()` → `JsonConvert.DeserializeObject()`

3. **WinForms Dependencies**: Most WinForms code is commented out, but System.Drawing is used. May need `<UseWindowsForms>true</UseWindowsForms>`. If any remaining UI code causes compilation errors, it can be commented out since the application is primarily a console application now.

4. **WCF Dependencies**: `WCFHelper.cs` and `System.ServiceModel` references exist in the codebase. If these cause compilation errors, they can be commented out rather than fixed, as WCF functionality is probably not needed.

5. **Configuration Migration**: app.config contains custom sections and user settings that need proper migration.

6. **Build Configurations**: Source has multiple build configurations (Academic, Basic, Community, etc.). May need to simplify or preserve as needed.

## Automation Script Considerations

If automation is needed:

- C# script to copy files preserving directory structure
- PowerShell script to update project references
- Script to analyze and report incompatible APIs

## Notes

- **Current State**: All migration work has been rolled back. SqlBuilderLib contains only placeholder `Greeter.cs`. All source files remain in `net-framework\sql.builder\`.
- **Two-Phase Approach**: 
- Phase 1: Copy files only (no code changes) → Commit
- Phase 2: Make code changes for compatibility → Commit
- This allows clear separation for review
- The source Program.cs shows it's primarily a console application now (WinForms mostly commented out)
- Most System.Windows.Forms references are commented out, suggesting UI migration already in progress
- **UI Code Handling**: If any UI-related code (System.Windows.Forms, System.Drawing UI components) remains and causes compilation errors, it's probably not used and can be commented out rather than fixed, since the application is now primarily a console application
- **WCF Code Handling**: WCF (Windows Communication Foundation) code is probably not needed. If `WCFHelper.cs` or `System.ServiceModel` references cause compilation errors, they can be commented out rather than fixed
- **Migration Philosophy**: Only change code when necessary for .NET 8 compatibility. Keep existing C# 6 syntax, namespaces, and code patterns unless they cause compilation errors.
- **Specific Files Requiring Code Changes (Phase 2)**:
- `ExcelPrintEnv.cs`: 3 locations using Ionic.Zip (lines 13, 57, 324)
- `DataExtractHelper.cs`: 2 locations using JavaScriptSerializer (lines 163, 200)