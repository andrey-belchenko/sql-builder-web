---
name: Fix Corrupted Cyrillic Encoding
overview: Fix corrupted Cyrillic text in .cs files by matching lines with the old project at c:\Repos\ai-tfs\root\main\all\sql.builder\. When a match is found, replace corrupted text with correct text. When not found, remove corrupted symbols.
todos:
  - id: scan-corrupted
    content: Create script to scan all .cs files and identify lines with corrupted Cyrillic text
    status: completed
  - id: match-context
    content: For each corrupted line, find corresponding file in old project and match by context (surrounding code)
    status: completed
  - id: fix-found
    content: Replace corrupted text with correct text when match is found in old project
    status: in_progress
  - id: clean-not-found
    content: Remove corrupted symbols from lines when no match is found in old project
    status: pending
  - id: verify-fix
    content: Verify fixes by reading sample files and checking Cyrillic text displays correctly
    status: pending
isProject: false
---

# Fix Corrupted Cyrillic Encoding

## Problem

Many .cs files have corrupted Cyrillic text (showing as `я┐╜я┐╜я┐╜` or similar garbled characters) due to encoding issues. The original text can be found in the old project at `c:\Repos\ai-tfs\root\main\all\sql.builder\`.

## Primary Rule

**CRITICAL**: Only fix corrupted Cyrillic text in the following locations:

- **Text literals** (string values)
- **Comments** (single-line `//` and multi-line `/* */`)
- **Region names** (`#region` and `#endregion` directives)

**DO NOT change**:

- Method signatures
- Class/interface/struct definitions
- Property declarations
- Variable names
- Code logic
- Any other code structure

**Note**: The current solution works correctly. The older implementation did not work and has been rolled back. Only apply fixes to text literals, comments, and region names - preserve all code logic exactly as-is.

## Approach

1. **Scan for corrupted lines**: Find all lines containing corrupted Cyrillic patterns
2. **Match with old project**: For each corrupted line, find the corresponding file in the old project and match by context
3. **Fix when found**: Replace corrupted text with correct text from the old project
4. **Clean when not found**: Remove corrupted symbols if no match is found

## Implementation Steps

### Step 1: Create a script to identify corrupted lines

- Scan all `.cs` files in `SqlBuilderLib` for corrupted Cyrillic patterns
- Patterns to detect:
  - `я┐╜` sequences
  - Question marks in comment contexts (`// ????`)
  - Garbled characters that should be Cyrillic

### Step 2: Match corrupted lines with old project

- For each corrupted line found:
  - Determine the relative path in the old project (e.g., `Print\Xlsx\ExcelCell.cs`)
  - Read the corresponding file from `c:\Repos\ai-tfs\root\main\all\sql.builder\`
  - Match lines by context (surrounding code, method signatures, etc.)
  - Extract the correct Cyrillic text

### Step 3: Fix files

- Replace corrupted text with correct text when match is found
- Remove corrupted symbols (clean the line) when no match is found
- Preserve all other code structure

## Files to Process

Based on initial scan, these files have encoding issues:

- `SqlBuilderLib\Clean\CleanFrmExpressReport.cs` (may not exist in old project)
- `SqlBuilderLib\DevTools\DevAnalyzer.cs` (may not exist in old project)
- `SqlBuilderLib\Print\Xlsx\ExcelCell.cs` ✓ (exists in old project)
- `SqlBuilderLib\Print\Xlsx\ExcelWorksheet.cs` ✓ (exists in old project)
- `SqlBuilderLib\Print\Xlsx\ExcelRow.cs` ✓ (exists in old project)
- Plus potentially 168+ more files found in grep search

## Example Fix

**Current (corrupted):**

```csharp
// ����� �������� ������������� ��� ��������
```

**Old project (correct):**

```csharp
// чтобы значения пересчитались при открытии
```

**Result:**

```csharp
// чтобы значения пересчитались при открытии
```

## Technical Details

- Use line-by-line comparison with context matching
- Match by surrounding code (3-5 lines before/after)
- Handle cases where files don't exist in old project
- Handle cases where lines have changed significantly
- Preserve file encoding as UTF-8
- **Only modify text literals, comments, and region names** - all code logic must remain unchanged
- When matching with old project, extract only the text content from comments/literals/regions, not code structure

## Validation

After fixing:

- Verify Cyrillic text displays correctly in comments, text literals, and region names
- **CRITICAL**: Ensure no code logic was changed - verify method signatures, class definitions, and code structure remain identical
- Check that comments are readable
- Verify that only text content was modified, not code syntax or structure