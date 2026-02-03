---
name: Generate TypeScript Navigator Files
overview: Implement recursive folder processing in `TsBuilder.cs` to generate TypeScript navigator files that match the structure of `nav-10.ts`, processing only folders (skipping reports) with auto-generated folder IDs.
todos:
  - id: recursive_method
    content: Create recursive ProcessFoldersRecursive method to traverse folder hierarchy
    status: completed
  - id: navigator_props
    content: Extract navigator properties (navigatorId, appId, customerId, useReportBuilder, title) from VNavigator
    status: completed
  - id: ts_generation
    content: Generate TypeScript code string with proper imports, exports, and Navigator structure
    status: completed
  - id: folder_id_counter
    content: Implement folder ID counter starting from initialFolderId (10000)
    status: completed
  - id: update_build_method
    content: Update BuildNavigators() to use recursive processing and write generated TypeScript to files
    status: completed
---

# Generate TypeScript Navigator Files from VNavigator Structure

## Overview

Enhance `BuildNavigators()` method in [`SqlBuilder.DevTools/TsBuilder.cs`](C:\Repos\github\sql-builder-web\net\SqlBuilder.DevTools\TsBuilder.cs) to recursively process folders from `VNavigator` and generate TypeScript files matching the structure of [`asuse-ai-reports/reports-config/navigators/nav-10.ts`](C:\Repos\ai\asuse-ai\asuse-ai-reports\reports-config\navigators\nav-10.ts).

## Implementation Details

### 1. Create Recursive Folder Processing Method

- Add a recursive method `ProcessFoldersRecursive()` that:
  - Takes a `VSXElement` parent and a `ref int folderIdCounter` parameter
  - Iterates through `parent.GetElementsP()` 
  - For each `VFolder`, creates a `new Folder({...})` TypeScript code block
  - Recursively processes child folders
  - Skips `VUseReport` items (as requested)
  - Returns a list of TypeScript code strings representing folders

### 2. Extract Navigator Properties

- Parse `navigatorId` from `nav.P_IdName` by extracting the number (e.g., "nav10" → 10, "nav310" → 310)
- Set navigator properties with fixed values:
  - `navigatorId`: Extract number from `nav.P_IdName` (remove "nav" prefix and parse)
  - `appId`: Always `10`
  - `customerId`: Same as `navigatorId`
  - `useReportBuilder`: Always `false`
  - `title`: Always `'Отчеты. Навигатор'`

### 3. Generate TypeScript File Content

- Build the TypeScript file structure:
  - Import statements: `Navigator`, `Folder`
  - Export default async function
  - Navigator constructor with extracted properties
  - Items array containing recursively processed folders
- Use proper indentation and formatting matching the example file

### 4. Folder ID Generation

- Use `initialFolderId` (10000) as starting point
- Increment `folderIdCounter` for each folder encountered (including nested ones)
- Each folder gets: `folderId: folderIdCounter++`

### 5. Folder Properties

- Extract folder title from `folder.P_Title` or `folder.P_SelfTitle`
- Generate `items` array by recursively processing child elements
- Only include folders in the `items` array (skip reports)

## Files to Modify

- [`SqlBuilder.DevTools/TsBuilder.cs`](C:\Repos\github\sql-builder-web\net\SqlBuilder.DevTools\TsBuilder.cs) - Enhance `BuildNavigators()` method

## Example Output Structure

```typescript
import { Navigator } from '@/system/reports/types/Navigator';
import { Folder } from '@/system/reports/types/Folder';

export default async () =>
    new Navigator({
        title: 'Отчеты. Навигатор',
        navigatorId: 10,
        appId: 10,
        customerId: 10,
        useReportBuilder: false,
        items: [
            new Folder({
                title: 'Folder Title',
                folderId: 10000,
                items: [
                    new Folder({
                        title: 'Nested Folder',
                        folderId: 10001,
                        items: [],
                    }),
                ],
            }),
        ],
    });
```