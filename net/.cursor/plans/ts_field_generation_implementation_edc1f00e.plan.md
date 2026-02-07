---
name: TS Field Generation Implementation
overview: Implement TypeScript field definition generation in ProcessFieldGenTs method. Generate field files in generated/fields subfolder, handling single fields and FieldGroup wrapping for multiple fields. Support all editor types and omit null properties.
todos:
  - id: create-directory-helper
    content: Add helper method to ensure fields directory exists
    status: completed
  - id: generate-imports
    content: Implement GenerateFieldImports to collect and generate required import statements based on editor types
    status: completed
  - id: generate-editor-code
    content: Implement GenerateEditorCode and GenerateSelectEditorCode to convert EditorProps to TypeScript code
    status: completed
  - id: generate-field-code
    content: Implement GenerateFieldCode to convert FieldProps to TypeScript Field instance code
    status: completed
  - id: implement-main-method
    content: Implement ProcessFieldGenTs to handle single/multiple fields, generate code, and write file
    status: completed
  - id: fix-number-editor-bug
    content: "Fix bug in CreateEditor method: UINumber should map to NumberEditor, not CheckEditor"
    status: completed
---

# TS Field Generation Implementation

## Overview

Implement the `ProcessFieldGenTs` method in [`TsBuilder.Field.GenTs.cs`](net/SqlBuilder.DevTools/TsBuilder.Field.GenTs.cs) to generate TypeScript field definition files based on `fieldsProps` data structure.

## Key Requirements

1. Generate files in `generated/fields` subfolder
2. If `fieldsProps` contains multiple items, wrap them in `FieldGroup`
3. Omit null/empty properties from generated code
4. Support all editor types: SelectEditor, DateEditor, TextEditor, NumberEditor, CheckEditor
5. Match the structure of example files (`field_example.ts`, `field_example2.ts`)

## Implementation Details

### File Structure

- **Location**: `BasePath/fields/field_{clearedName}.ts`
- **Format**: Default export function accepting `overrideProps` and `overrideEditorProps`
- **Single field**: Returns `new Field({...})`
- **Multiple fields**: Returns `new FieldGroup({ label: '', items: [...] })`

### Code Generation Components

1. **Import Generation**

- Determine required editor imports based on `fieldsProps`
- Import `Field` and `PartialFieldProps` from `@/system/reports/types/Field`
- Import `FieldGroup` and `FieldGroupProps` if multiple fields
- Import editor types: `SelectEditor`, `DateEditor`, `TextEditor`, `NumberEditor`, `CheckEditor`
- Import `execQueryByName` from `./utils` if query-based methods are used

2. **Field Properties Generation**

- `label`: String literal
- `name`: String literal
- `editor`: Editor instance based on `FieldProps.editor.editorType`
- `defaultValue`: Use `MethodInfo.ToTs()` if not null
- `defaultValueDeps`: Array if not null/empty
- `required`: Use `MethodInfo.ToTs()` if not null
- `requiredDeps`: Array if not null/empty
- `validation`: Use `MethodInfo.ToTs()` if not null
- `validationDeps`: Array if not null/empty
- `exists`: Use `MethodInfo.ToTs()` if not null
- `existsDeps`: Array if not null/empty
- `enabled`: Use `MethodInfo.ToTs()` if not null
- `enabledDeps`: Array if not null/empty
- `visible`: Use `MethodInfo.ToTs()` if not null
- `visibleDeps`: Array if not null/empty

3. **Editor Generation**

- **SelectEditor**: Generate `SelectEditorProps` with columns, keyField, displayField, listItems, listItemsDeps, singleSelection, remoteOperations
- **DateEditor**: Generate empty props `{}` (format/editMask are deprecated)
- **TextEditor**: Generate empty props `{}`
- **NumberEditor**: Generate empty props `{}` (step/format/editMask are deprecated)
- **CheckEditor**: Generate empty props `{}`
- Only include non-null properties

4. **MethodInfo to TypeScript**

- Use existing `MethodInfo.ToTs()` method
- Handle query names: `$ => execQueryByName('queryName', $)`
- Handle field references: `($) => $.formValues['fieldName']`
- Handle boolean/string values: `() => value`

5. **Multiple Fields Handling**

- If `fieldsProps.Count() > 1`, wrap in `FieldGroup`
- Use field label or generate group label
- Generate array of Field instances

### Implementation Steps

1. **Create directory structure**: Ensure `BasePath/fields` exists
2. **Determine output structure**: Check if single or multiple fields
3. **Collect imports**: Scan all fields to determine required editor types
4. **Generate imports section**: Build import statements
5. **Generate field/group code**: Build Field or FieldGroup instance
6. **Write file**: Use `File.WriteAllText` with UTF-8 encoding

### Helper Methods Needed

- `GenerateFieldImports(List<FieldProps> fieldsProps)`: Returns import statements
- `GenerateFieldCode(FieldProps props)`: Returns field TypeScript code
- `GenerateEditorCode(EditorProps editor)`: Returns editor TypeScript code
- `GenerateSelectEditorCode(SelectEditorProps editor)`: Returns SelectEditor code
- `ShouldIncludeProperty(object value)`: Checks if property should be included
- `EscapeString(string value)`: Escape strings for TypeScript (already exists)

### Notes

- Fix bug in `CreateEditor`: Line 312 maps `UINumber` to `CheckEditor` but should map to `NumberEditor`
- MethodInfo.ToTs() already handles query names, field refs, and values
- Column arrays in SelectEditor should be formatted as `[{ dataField: '...', caption: '...' }]`
- Dependencies arrays should be formatted as `['dep1', 'dep2']`