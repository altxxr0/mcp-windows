# Contributing Test Scenarios

This guide explains how to write and contribute new test scenarios for the LLM-Based Integration Testing Framework.

## Quick Start

1. Copy the template: `templates/scenario-template.md`
2. Rename to: `scenarios/TC-{CATEGORY}-{NNN}.md`
3. Fill in all required sections (marked with *)
4. Submit for review

## Scenario Naming Convention

Test scenarios follow this pattern:

```
TC-{CATEGORY}-{NNN}
```

| Component | Description | Values |
|-----------|-------------|--------|
| `TC` | Fixed prefix | "TC" (Test Case) |
| `CATEGORY` | Test category | MOUSE, KEYBOARD, WINDOW, SCREENSHOT, WORKFLOW, ERROR, VISUAL |
| `NNN` | Three-digit number | 001-999 |

**Examples**:
- `TC-MOUSE-001` - First mouse control test
- `TC-KEYBOARD-015` - Fifteenth keyboard test
- `TC-WORKFLOW-003` - Third workflow test

### Finding the Next Number

To find the next available number for a category:

1. List existing scenarios: `ls scenarios/TC-{CATEGORY}-*.md`
2. Find the highest number
3. Increment by 1

## Required Sections

Every scenario MUST include these sections:

### 1. Metadata (table format)

```markdown
| Field | Value |
|-------|-------|
| **ID** | TC-MOUSE-001 |
| **Category** | MOUSE |
| **Priority** | P1 |
| **Target App** | Calculator |
| **Target Monitor** | Secondary (fallback: Primary) |
| **Timeout** | 30 seconds |
```

### 2. Objective

One or two sentences describing what the test verifies:

```markdown
## Objective

Verify that the mouse cursor moves to specified absolute coordinates on the secondary monitor.
```

### 3. Preconditions

Checkable requirements before test execution:

```markdown
## Preconditions

- [ ] Secondary monitor available (or fallback to primary acknowledged)
- [ ] Calculator is open on secondary monitor
- [ ] Calculator is in Standard mode
- [ ] No modal dialogs blocking
```

### 4. Steps

Ordered list of actions with MCP tool details:

```markdown
## Steps

### Step 1: Detect Monitors

**MCP Tool**: `screenshot_control`  
**Action**: `list_monitors`  
**Purpose**: Identify monitor configuration and bounds

### Step 2: Before Screenshot (All Monitors)

**MCP Tool**: `screenshot_control`  
**Action**: `capture`  
**Parameters**: `target="all_monitors"`, `includeCursor=true`  
**Save As**: `step-1-before.png`  
**Save Metadata**: `step-1-before-meta.json` (contains monitor regions for verification)
```

> **Note**: Use `target="all_monitors"` to capture a composite image of all connected monitors. 
> The metadata JSON contains `MonitorRegions` with each monitor's bounds within the composite image,
> enabling targeted visual verification on any specific monitor.

### 5. Expected Result

What should happen when the test passes:

```markdown
## Expected Result

The mouse cursor moves to the center of the secondary monitor. The "after" screenshot shows the cursor at the target coordinates.
```

### 6. Pass Criteria

Checkable items for determining pass/fail:

```markdown
## Pass Criteria

- [ ] `list_monitors` returns at least 2 monitors
- [ ] `move` action completes without error
- [ ] "After" screenshot shows cursor at target location
- [ ] Cursor coordinates in response match target
```

## Optional Sections

### Failure Indicators

Common failure patterns to watch for:

```markdown
## Failure Indicators

- Cursor not visible in "after" screenshot
- Cursor at wrong coordinates
- Tool returns error response
```

### Notes

Additional context or known issues:

```markdown
## Notes

- Windows may restrict cursor movement in some scenarios
- DPI scaling affects coordinate calculations
```

### Cleanup

Actions to reset state after test:

```markdown
## Cleanup

Close Calculator window to reset for next test.
```

## Writing Quality Steps

### DO:
- ✅ Include specific MCP tool names
- ✅ List all relevant parameters
- ✅ Use `target="all_monitors"` for composite screenshots
- ✅ Save metadata JSON alongside screenshots for region mapping
- ✅ Specify screenshot save filenames with step prefixes (e.g., `step-1-before.png`)
- ✅ Include cursor in screenshots (`includeCursor=true`)
- ✅ Describe verification criteria

### DON'T:
- ❌ Leave placeholders unfilled
- ❌ Omit before/after screenshots
- ❌ Forget to save composite metadata for multi-monitor scenarios
- ❌ Write vague verification steps
- ❌ Use single-monitor capture when all-monitors is more appropriate

## Category Guidelines

### MOUSE
- Test cursor movement, clicks, scrolling, dragging
- Include cursor in screenshots for verification
- Calculate coordinates relative to window/monitor bounds

### KEYBOARD
- Test typing, key presses, shortcuts, modifiers
- Ensure target window has focus before typing
- Use appropriate action: `type` for text, `press` for keys, `combo` for shortcuts

### WINDOW
- Test window management: find, move, resize, activate
- Store window handles for subsequent operations
- Verify window state changes visually

### SCREENSHOT
- Test screen capture capabilities
- **Use `target="all_monitors"` for composite capture** of all connected displays
- Save metadata JSON for composite screenshots to enable region identification
- Verify returned base64 data or file creation
- Test monitor targeting and region capture using metadata regions

### WORKFLOW
- Chain multiple tools together
- **Take all-monitors screenshots at key workflow steps**
- Save metadata JSON for visual diff analysis across monitors
- Take intermediate screenshots at key points
- Document the complete flow with dependencies

### ERROR
- Test error handling and edge cases
- Verify appropriate error messages
- Document expected failure behavior

### VISUAL
- Test visual verification patterns
- **Use all-monitors composite screenshots for before/after comparison**
- Use metadata regions to focus on specific monitor areas
- Focus on before/after comparison with visual diff
- Document what visual changes to look for and expected change percentage

## Validation Checklist

Before submitting, verify:

- [ ] File named correctly: `TC-{CATEGORY}-{NNN}.md`
- [ ] All required sections present
- [ ] Metadata table complete
- [ ] At least one pass criterion
- [ ] Steps include MCP tool details
- [ ] Screenshots use `target="all_monitors"` for composite capture
- [ ] Metadata JSON saved alongside composite screenshots
- [ ] Screenshots named with step prefixes (e.g., `step-1-before.png`)
- [ ] Before/after screenshots for visual verification steps

## Example Scenarios

See these scenarios as references:

- [TC-MOUSE-001](../scenarios/TC-MOUSE-001.md) - Simple single-tool test
- [TC-WINDOW-005](../scenarios/TC-WINDOW-005.md) - Window management test
- [TC-WORKFLOW-001](../scenarios/TC-WORKFLOW-001.md) - Multi-step workflow

## Getting Help

- Review `templates/scenario-template.md` for the complete template
- Check `templates/example-scenario-annotated.md` for detailed annotations
- Consult `data-model.md` for entity definitions
- See `contracts/scenario-schema.json` for validation rules
