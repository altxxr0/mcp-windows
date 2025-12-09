# Workflow Testing Guide

This guide explains how to execute multi-step workflow test scenarios that combine multiple MCP tools to accomplish complex UI automation tasks.

## Overview

Workflow tests validate tool chaining - the ability to use multiple MCP tools together in sequence to accomplish a realistic task. Unlike single-tool tests that verify individual operations, workflow tests simulate real user interactions.

## Workflow Patterns

### 1. Find-and-Activate Pattern

**Used By**: TC-WORKFLOW-001, TC-WORKFLOW-003, TC-WORKFLOW-004, TC-WORKFLOW-006

This fundamental pattern establishes context for all subsequent operations:

```
1. screenshot_control (list_monitors) → Identify target monitor
2. window_management (find) → Locate target window
3. window_management (activate) → Bring window to foreground
4. [Perform operation]
5. screenshot_control (capture) → Verify result
```

**Key Points**:
- Always identify monitors first for multi-monitor targeting
- Store window handle for subsequent operations
- Activate before keyboard/mouse input
- Capture screenshots before and after for verification

### 2. Mouse-Interaction Pattern

**Used By**: TC-WORKFLOW-004, TC-WORKFLOW-009

```
1. Find target window (using Find-and-Activate)
2. Calculate click coordinates (relative to window bounds)
3. mouse_control (move) → Position cursor
4. screenshot_control (capture) → Verify cursor position
5. mouse_control (click/drag) → Perform interaction
6. screenshot_control (capture) → Verify result
```

**Key Points**:
- Always calculate coordinates relative to window position
- Take pre-click screenshot to verify cursor position
- Consider DPI scaling when calculating positions

### 3. Keyboard-Interaction Pattern

**Used By**: TC-WORKFLOW-003, TC-WORKFLOW-005, TC-WORKFLOW-007, TC-WORKFLOW-010

```
1. Find and activate target window
2. keyboard_control (type/press/combo) → Send input
3. screenshot_control (capture) → Verify input registered
```

**Key Points**:
- Window must have focus before keyboard input
- Use `type` for text, `press` for single keys, `combo` for shortcuts
- Capture screenshots after each significant keyboard action

### 4. Multi-Window Pattern

**Used By**: TC-WORKFLOW-008

```
1. Find all target windows
2. Set window sizes (resize)
3. Position windows (move)
4. Activate to control Z-order
5. Capture to verify arrangement
```

## Execution Guidelines

### Before Starting

1. **Check Prerequisites**:
   - Target applications open
   - Applications on correct monitor
   - Windows in restored (not maximized) state
   - No modal dialogs blocking

2. **Document Initial State**:
   - Take "before" screenshot
   - Record window handles and positions

### During Execution

1. **Execute Sequentially**:
   - Run each step in order
   - Wait for completion before next step
   - Capture intermediate screenshots as specified

2. **Handle Errors**:
   - Document any failures
   - Note which step failed
   - Capture error screenshot if possible

3. **Validate Progress**:
   - Verify each step's result before proceeding
   - If step fails, document and decide whether to continue

### After Completion

1. **Visual Verification**:
   - Compare before/after screenshots
   - Verify expected changes occurred
   - Check intermediate screenshots for proper progression

2. **Document Results**:
   - Fill out result template
   - Attach all screenshots
   - Note any anomalies or issues

## Coordinate Calculations

Many workflows require calculating screen positions. Here are common formulas:

### Button in Window
```
button_x = window_x + (window_width * relative_x)
button_y = window_y + (window_height * relative_y)
```

### Center of Window
```
center_x = window_x + (window_width / 2)
center_y = window_y + (window_height / 2)
```

### Cascade Offset
```
window1: x = monitor_x + 50, y = monitor_y + 50
window2: x = monitor_x + 100, y = monitor_y + 100
```

## Tool Dependencies

| Workflow | Primary Tools | Supporting Tools |
|----------|---------------|------------------|
| Find & Activate | window_management | screenshot_control |
| Move Window | window_management | screenshot_control |
| Type Text | keyboard_control | window_management, screenshot_control |
| Click Button | mouse_control | window_management, screenshot_control |
| Copy-Paste | keyboard_control | window_management, screenshot_control |
| Drag-Drop | mouse_control | keyboard_control, window_management, screenshot_control |

## Common Issues

### Focus Lost
- **Symptom**: Keyboard input goes to wrong window
- **Solution**: Re-activate target window before input

### Coordinate Mismatch
- **Symptom**: Click misses target
- **Solution**: Verify window bounds, check DPI scaling

### Timing Issues
- **Symptom**: Action executes before previous completes
- **Solution**: Add verification step between actions

### Monitor Confusion
- **Symptom**: Operations target wrong monitor
- **Solution**: Always list monitors first, use explicit indices

## Workflow Test Inventory

| ID | Description | Complexity | Tools Used |
|----|-------------|------------|------------|
| TC-WORKFLOW-001 | Find and activate window | Low | window, screenshot |
| TC-WORKFLOW-002 | Move window and verify | Low | window, screenshot |
| TC-WORKFLOW-003 | Type text in window | Medium | window, keyboard, screenshot |
| TC-WORKFLOW-004 | Click button in app | Medium | window, mouse, screenshot |
| TC-WORKFLOW-005 | Open app via keyboard | Medium | keyboard, window, screenshot |
| TC-WORKFLOW-006 | Resize and screenshot | Low | window, screenshot |
| TC-WORKFLOW-007 | Copy-paste workflow | High | keyboard, window, screenshot |
| TC-WORKFLOW-008 | Multi-window cascade | High | window, screenshot |
| TC-WORKFLOW-009 | Drag and drop | High | mouse, keyboard, window, screenshot |
| TC-WORKFLOW-010 | Full calculation sequence | High | keyboard, window, screenshot |

---

## All-Monitors Capture in Workflows (v1.1+)

For comprehensive multi-monitor testing, use `target="all_monitors"` to capture all connected monitors as a single composite image at each workflow step.

### Why Use All-Monitors Capture in Workflows

1. **Track window movement across monitors**: Windows may move between monitors during operations
2. **Capture complete desktop state**: See the full context including all visible windows
3. **Validate no unexpected changes**: Verify other monitors remain unchanged during operations
4. **Support cross-monitor workflows**: Drag-drop or move operations between monitors

### All-Monitors Workflow Pattern

```
1. screenshot_control (list_monitors) → Identify all monitors
2. screenshot_control (capture, target="all_monitors") → Before state of all monitors
   → Save: step-N-before.png + step-N-before-meta.json
3. [Perform operation]
4. screenshot_control (capture, target="all_monitors") → After state of all monitors
   → Save: step-N-after.png + step-N-after-meta.json
5. Compare specific monitor regions using metadata
```

### Using Composite Metadata for Verification

The `compositeMetadata` in the response enables targeted analysis:

```json
{
  "virtual_screen": { "x": -1920, "y": 0, "width": 3840, "height": 1080 },
  "monitors": [
    { "index": 0, "x": 0, "y": 0, "width": 1920, "height": 1080, "is_primary": true },
    { "index": 1, "x": 1920, "y": 0, "width": 1920, "height": 1080, "is_primary": false }
  ]
}
```

Using this metadata, the LLM can:
- Identify which monitor contains a target window
- Focus analysis on specific monitor regions
- Detect cross-monitor window movement
- Verify isolation (no changes on other monitors)

### Workflow Verification Prompts with All-Monitors

**After Window Move**:
```
Compare the before and after composite screenshots.
Using compositeMetadata.monitors:
1. Which monitor region contained the target window in the "before" image?
2. Which monitor region contains the window in the "after" image?
3. Did the window move within a monitor or between monitors?
4. Are there any unexpected changes on other monitors?
```

**After Keyboard/Mouse Action**:
```
Compare the composite screenshots, focusing on the monitor where the target app is located.
Using compositeMetadata to identify the correct region:
1. What changed in the target application?
2. Did the change match the expected result?
3. Is the rest of the desktop unchanged?
```

### File Naming for All-Monitors Workflows

```
results/{YYYY-MM-DD}/TC-WORKFLOW-XXX/
├── step-1-before.png           # Composite before step 1
├── step-1-before-meta.json     # Metadata for step 1 before
├── step-1-after.png            # Composite after step 1
├── step-1-after-meta.json      # Metadata for step 1 after
├── step-2-before.png           # Composite before step 2
├── step-2-before-meta.json
├── step-2-after.png
├── step-2-after-meta.json
├── ...
└── result.md
```

### Best Practices for All-Monitors Workflows

1. **Save metadata alongside screenshots**: Always save the `compositeMetadata` JSON for later analysis
2. **Use consistent capture settings**: Same `includeCursor` value throughout the workflow
3. **Reference monitor indices**: Use `compositeMetadata.monitors[N]` to identify specific regions
4. **Handle negative virtual screen coordinates**: Virtual screen origin may be negative if monitors extend left/above primary
5. **Compare equivalent regions**: When comparing before/after, focus on the same monitor region using metadata

