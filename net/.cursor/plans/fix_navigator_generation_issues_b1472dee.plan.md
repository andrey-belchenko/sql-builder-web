---
name: Fix Navigator Generation Issues
overview: Fix duplicate reports, double commas, and indentation issues in generated navigator TypeScript files by removing redundant report additions and fixing comma handling in the C# code generator.
todos:
  - id: remove-duplicate-reports
    content: Remove lines 62-66 that add DirectReports again (they are already in FolderCode)
    status: completed
  - id: fix-comma-handling
    content: Fix folder code building to directly append FolderCode instead of using intermediate list to avoid double commas
    status: in_progress
    dependencies:
      - remove-duplicate-reports
  - id: verify-formatting
    content: Ensure consistent indentation and proper comma placement in generated files
    status: pending
    dependencies:
      - fix-comma-handling
---

# Fix Navigator Generation Issues

## Problems Identified

1. **Duplicate Reports**: Reports appear twice in folder items arrays because:

   - `childResult.FolderCode` already contains all reports (from recursive processing)
   - Lines 62-66 explicitly add `childResult.DirectReports` again, causing duplication

2. **Double Commas**: Folder definitions end with `}),` but when joined, this creates `}),,` syntax errors

3. **Inconsistent Indentation**: Reports use different indent levels depending on how they're added

## Solution

### Fix 1: Remove Duplicate Report Addition ([net/SqlBuilder.DevTools/TsBuilder.Navigator.cs](net/SqlBuilder.DevTools/TsBuilder.Navigator.cs))

Remove lines 62-66 that add `DirectReports` again. The `childResult.FolderCode` already contains all folder items including reports in the correct order.

**Current code (lines 56-66):**

```csharp
// Add child folders and reports in order (from childResult.FolderCode)
if (!string.IsNullOrWhiteSpace(childResult.FolderCode))
{
    childFolderItems.Add(childResult.FolderCode);
}

// Add direct reports from the child folder (these are reports that are direct children of the child folder)
foreach (var reportName in childResult.DirectReports)
{
    childFolderItems.Add($"{childIndent}    report_{reportName}");
}
```

**Fixed code:**

```csharp
// Add child folders and reports in order (from childResult.FolderCode)
// Note: FolderCode already contains all items including direct reports
if (!string.IsNullOrWhiteSpace(childResult.FolderCode))
{
    folderCode += $"\n{childResult.FolderCode}";
}
```

### Fix 2: Fix Comma Handling

The issue is that `childResult.FolderCode` contains items joined with commas, but when we add it to `childFolderItems` and join again, we get double commas. Since `FolderCode` already contains properly formatted items, we should append it directly to `folderCode` instead of adding to a list and joining.

**Change the folder code building logic (lines 47-74):**

- Instead of building `childFolderItems` list and joining, directly append `childResult.FolderCode` to `folderCode`
- Ensure proper comma handling - `FolderCode` should end items with proper formatting

### Fix 3: Ensure Consistent Formatting

The `FolderCode` returned from recursive calls should have consistent indentation. Since we're building it at the correct indent level in the recursive call, we just need to ensure it's appended correctly.

## Implementation Details

1. **Remove duplicate report addition** (lines 62-66)
2. **Simplify folder code building** - directly append `FolderCode` instead of using intermediate list
3. **Fix comma handling** - ensure `FolderCode` items are properly formatted without trailing commas that cause double commas

## Expected Result

- Each report appears exactly once in folder items
- No double commas (`}),,`) - only single comma (`}),`)
- Consistent indentation throughout
- Maintains source order of folders and reports