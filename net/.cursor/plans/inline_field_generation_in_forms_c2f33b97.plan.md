---
name: Inline Field Generation in Forms
overview: Refactor field generation to embed field definitions directly into form TypeScript files instead of separate files. Reuse existing field generation code but remove wrapper functions and accumulate form-level imports and items.
todos:
  - id: create-form-state-tracking
    content: Create FormGenerationState class and static dictionary to track form-level items and imports
    status: completed
  - id: refactor-field-generation
    content: Refactor ProcessFieldGenTs to return inline field code instead of writing files, remove wrapper functions
    status: completed
    dependencies:
      - create-form-state-tracking
  - id: create-inline-helpers
    content: Create GenerateInlineFieldCode and GenerateInlineFieldGroupCode methods for inline field/group generation
    status: completed
    dependencies:
      - refactor-field-generation
  - id: update-process-field
    content: Update ProcessField to accumulate field code in form state instead of writing files
    status: completed
    dependencies:
      - create-inline-helpers
  - id: implement-field-group
    content: Implement ProcessFieldGroup to generate FieldGroup code and handle nested fields
    status: completed
    dependencies:
      - create-inline-helpers
  - id: update-content-children
    content: Update ProcessContentChildren to accumulate items in form state as fields are processed
    status: completed
    dependencies:
      - update-process-field
      - implement-field-group
  - id: refactor-form-generation
    content: Refactor GenerateFormTypeScript to generate complete form with populated items array and imports
    status: completed
    dependencies:
      - update-content-children
  - id: create-import-generation
    content: Create GenerateFormImports method to generate all required imports for a form
    status: completed
    dependencies:
      - refactor-form-generation
  - id: cleanup
    content: Remove processedFields HashSet, field file directory creation, and any unused code
    status: completed
    dependencies:
      - refactor-form-generation
---

# Inline Field Generation in Forms

## Overview

Refactor the TypeScript generation to embed field definitions directly into form files instead of generating separate field files. Fields will be added to the form's `items` array in the order they appear in the XML structure.

## Key Changes

### 1. Form-Level State Tracking

Create a mechanism to track form-level data during processing:

- **Location**: Add static dictionary in `TsBuilder` class
- **Purpose**: Track items (fields/groups) and required imports per form
- **Structure**: `Dictionary<VForm, FormGenerationState>` where `FormGenerationState` contains:
  - `List<string> items` - Field/FieldGroup code strings
  - `HashSet<string> editorTypes` - Required editor types
  - `bool needsExecQueryByName` - Whether execQueryByName import needed
  - `bool needsGetFirstValue` - Whether getFirstValue import needed

### 2. Refactor ProcessFieldGenTs

**File**: [`net/SqlBuilder.DevTools/TsBuilder.Field.GenTs.cs`](net/SqlBuilder.DevTools/TsBuilder.Field.GenTs.cs)

- **Remove**: File writing logic, wrapper function generation, override props
- **Change**: Return field code string instead of writing to file
- **Reuse**: Keep `GenerateSingleFieldCode` and `GenerateFieldGroupCode` logic but modify to:
  - Remove `export default` wrapper
  - Remove `overrideProps` and `overrideEditorProps` parameters
  - Return just the Field instance code (e.g., `new Field({...})`)
- **New method**: `GenerateInlineFieldCode(FieldProps props)` - generates Field instance without wrapper
- **New method**: `GenerateInlineFieldGroupCode(List<FieldProps> fieldsProps, VField field)` - generates FieldGroup instance without wrapper

### 3. Update ProcessField Method

**File**: [`net/SqlBuilder.DevTools/TsBuilder.Field.cs`](net/SqlBuilder.DevTools/TsBuilder.Field.cs)

- **Change**: Instead of calling `ProcessFieldGenTs` to write files, call it to get field code
- **Action**: Add field code to form's state tracking
- **Collect imports**: Update form state with required editor types and utility imports

### 4. Implement ProcessFieldGroup

**File**: [`net/SqlBuilder.DevTools/TsBuilder.FieldGroup.cs`](net/SqlBuilder.DevTools/TsBuilder.FieldGroup.cs)

- **Implement**: Generate FieldGroup code for VFieldGroup elements
- **Process**: Recursively process child fields within the group
- **Return**: FieldGroup instance code (e.g., `new FieldGroup({ label: '...', items: [...] })`)
- **Reuse**: Use similar logic from `GenerateFieldGroupCode` but adapted for inline generation

### 5. Update ProcessContentChildren

**File**: [`net/SqlBuilder.DevTools/TsBuilder.Form.cs`](net/SqlBuilder.DevTools/TsBuilder.Form.cs)

- **Modify**: Accumulate field/group code strings in form state as fields are processed
- **Order**: Maintain XML order by adding items as they're processed recursively
- **Track**: Update form state imports as fields are processed

### 6. Refactor GenerateFormTypeScript

**File**: [`net/SqlBuilder.DevTools/TsBuilder.Form.cs`](net/SqlBuilder.DevTools/TsBuilder.Form.cs)

- **Change**: Generate complete form with populated items array
- **Steps**:

  1. Retrieve form state (items and imports)
  2. Generate import statements using `GenerateFieldImports` logic (reuse from `TsBuilder.Field.GenTs.cs`)
  3. Generate form with items array populated from accumulated field/group code
  4. Write complete form file

### 7. Import Generation

**Reuse**: Logic from `GenerateFieldImports` in `TsBuilder.Field.GenTs.cs`

- **New method**: `GenerateFormImports(FormGenerationState state)` - generates all required imports for a form
- **Include**: Editor imports, Field/FieldGroup imports, utility imports (execQueryByName, getFirstValue)

### 8. Code Generation Helpers

**New methods** in `TsBuilder.Field.GenTs.cs`:

- `GenerateInlineFieldCode(FieldProps props, string indent)` - generates `new Field({...})` code
- `GenerateInlineFieldGroupCode(List<FieldProps> fieldsProps, VField field, string indent)` - generates `new FieldGroup({...})` code
- **Reuse**: Existing `GenerateEditorCode`, `GenerateSelectEditorCode`, `GenerateFieldProperty`, `GenerateFieldPropertyArray` methods

## Implementation Details

### Field Code Format (Inline)

Instead of:

```typescript
export default (overrideProps: PartialFieldProps = {}, overrideEditorProps: PartialSelectEditorProps = {}) =>
    new Field({...})
```

Generate:

```typescript
new Field({
    label: '...',
    name: '...',
    editor: new SelectEditor({...}),
    ...
})
```

### FieldGroup Code Format (Inline)

Generate:

```typescript
new FieldGroup({
    label: '...',
    items: [
        new Field({...}),
        new Field({...}),
    ]
})
```

### Form Code Format

```typescript
import { Form } from '@/system/reports/types/Form';
import { Field } from '@/system/reports/types/Field';
import { FieldGroup } from '@/system/reports/types/FieldGroup';
import { SelectEditor } from '@/system/reports/types/editors/SelectEditor';
// ... other imports ...

export default new Form({
    items: [
        new Field({...}),
        new FieldGroup({
            label: '...',
            items: [
                new Field({...}),
            ]
        }),
        // ... more items ...
    ],
});
```

## Cleanup

- **Remove**: `processedFields` HashSet (no longer needed for file deduplication)
- **Remove**: Directory creation for `fields` folder
- **Keep**: All field property generation logic (reuse as-is)

## Testing Considerations

- Verify fields appear in correct XML order
- Verify FieldGroup nesting matches XML structure
- Verify all imports are correctly generated
- Verify no separate field files are created