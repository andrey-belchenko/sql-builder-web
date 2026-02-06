---
name: Generate TS Reports and Forms
overview: Implement TypeScript code generation for reports and forms. Generate form files in `generated/forms/`, report files in `generated/reports/`, and integrate reports into navigator items.
todos:
  - id: generate-forms
    content: Implement TypeScript form generation in ProcessForm method - generate form files with empty items array
    status: pending
  - id: generate-reports
    content: Implement TypeScript report generation in ProcessReport method - generate report files importing forms
    status: pending
    dependencies:
      - generate-forms
  - id: integrate-navigators
    content: Modify ProcessFoldersRecursive to collect reports and add them to navigator items with imports
    status: pending
    dependencies:
      - generate-reports
---

# Generate TypeScript Code for Reports and Forms

## Overview

Implement TypeScript code generation for reports and forms based on the C# SqlBuilder structure. Forms and reports will be generated to separate directories, and reports will be integrated into navigator items.

## Implementation Details

### 1. Form Generation (`TsBuilder.Form.cs`)

Modify `ProcessForm` method to:

- Generate TypeScript form file similar to `form_example.ts`
- File naming: `form_{ClearName(form.P_IdName)}.ts`
- Save to `generated/forms/` directory
- Form items array should be empty for now (as requested)
- Return the form import name (e.g., `form_example`) for use in report generation

**File structure:**

```typescript
import { Form } from '@/system/reports/types/Form';

export default new Form({
    items: [],
});
```

### 2. Report Generation (`TsBuilder.Report.cs`)

Modify `ProcessReport` method to:

- Generate TypeScript report file similar to `report_example.ts`
- File naming: `report_{ClearName(repFullName)}.ts` where `repFullName = "{P_Project}.{P_Report}"`
- Save to `generated/reports/` directory
- Import the corresponding form from `../forms/`
- Use `useReport.P_Title` for the report title
- Return the report import name (e.g., `report_example`) for use in navigator generation

**File structure:**

```typescript
import { RegularReport } from '@/system/reports/types/reports/RegularReport';
import form_{clearedName} from '../forms/form_{clearedName}';

export default new RegularReport({
    definedIn: __filename,
    title: '{escaped title}',
    paramsForm: form_{clearedName},
});
```

### 3. Navigator Integration (`TsBuilder.Navigator.cs`)

Modify `ProcessFoldersRecursive` method to:

- Collect reports when processing `VUseReport` items
- Generate import statements for reports at the top of navigator file
- Add reports directly to folder items array (like in `nav_example.ts`)
- Update `GenerateTypeScriptFile` to include report imports

**Changes:**

- Track reports per folder during recursive processing
- Generate imports: `import report_{name} from '../reports/report_{name}';`
- Add reports to folder items: `report_{name}`

## File Changes

1. **[net/SqlBuilder.DevTools/TsBuilder.Form.cs](net/SqlBuilder.DevTools/TsBuilder.Form.cs)**

   - Modify `ProcessForm` to generate TypeScript form file
   - Return form import name

2. **[net/SqlBuilder.DevTools/TsBuilder.Report.cs](net/SqlBuilder.DevTools/TsBuilder.Report.cs)**

   - Modify `ProcessReport` to generate TypeScript report file
   - Return report import name

3. **[net/SqlBuilder.DevTools/TsBuilder.Navigator.cs](net/SqlBuilder.DevTools/TsBuilder.Navigator.cs)**

   - Modify `ProcessFoldersRecursive` to collect and track reports
   - Modify `GenerateTypeScriptFile` to include report imports
   - Add reports to folder items arrays

## Directory Structure

- `generated/forms/` - Form TypeScript files
- `generated/reports/` - Report TypeScript files  
- `generated/navigators/` - Navigator TypeScript files (already exists)