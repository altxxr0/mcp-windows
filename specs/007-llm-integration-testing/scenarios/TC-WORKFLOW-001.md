# Test Case: TC-WORKFLOW-001

## Metadata

| Field | Value |
|-------|-------|
| **ID** | TC-WORKFLOW-001 |
| **Category** | WORKFLOW |
| **Priority** | P2 |
| **Target App** | Notepad |
| **Target Monitor** | Secondary (fallback: Primary) |
| **Timeout** | 60 seconds |

## Objective

Verify the complete workflow of finding a window by title and activating it to the foreground using multiple MCP tool calls in sequence.

## Preconditions

- [ ] Secondary monitor available (or fallback to primary acknowledged)
- [ ] Notepad is open with a known title
- [ ] Another window is currently in foreground (Notepad is in background)
- [ ] No modal dialogs blocking

## Steps

### Step 1: Detect and Target Secondary Monitor

**MCP Tool**: `screenshot_control`  
**Action**: `list_monitors`  
**Purpose**: Identify all monitors and their configuration

### Step 2: Before Screenshot (Initial State)

**MCP Tool**: `screenshot_control`  
**Action**: `capture`  
**Parameters**: `target="all_monitors"`, `includeCursor=true`  
**Save As**: `step-1-before.png`  
**Save Metadata**: `step-1-before-meta.json`

**Verify**: 
- Parse `compositeMetadata.monitors` to identify all monitor regions
- Locate Notepad window within a monitor region
- Note: Notepad is visible but NOT in the foreground (another window is active)

### Step 3: Find Window by Title

**MCP Tool**: `window_management`  
**Action**: `find`  
**Parameters**:
- `title`: `"Notepad"`
- `regex`: `true`

**Record**: Store the returned handle for use in Step 5.

### Step 4: Intermediate Screenshot (After Find)

**MCP Tool**: `screenshot_control`  
**Action**: `capture`  
**Parameters**: `target="all_monitors"`, `includeCursor=true`  
**Save As**: `step-2-after-find.png`  
**Save Metadata**: `step-2-after-find-meta.json`

**Verify**: State should be unchanged across all monitors (find doesn't affect window).

### Step 5: Activate Window by Handle

**MCP Tool**: `window_management`  
**Action**: `activate`  
**Parameters**:
- `handle`: `"{notepad_handle}"`

### Step 6: After Screenshot (Final State)

**MCP Tool**: `screenshot_control`  
**Action**: `capture`  
**Parameters**: `target="all_monitors"`, `includeCursor=true`  
**Save As**: `step-3-after.png`  
**Save Metadata**: `step-3-after-meta.json`

### Step 7: Visual Verification

Compare all-monitors composite screenshots:
- "step-1-before.png": Notepad visible but in background (identify which monitor region)
- "step-2-after-find.png": Same as before across all monitors (no change)
- "step-3-after.png": Notepad now in foreground with active title bar

Using the metadata, verify:
- Notepad remains on the same monitor throughout the workflow
- No unexpected changes on other monitors
- Title bar shows active/focused state in the final screenshot

## Expected Result

The workflow successfully:
1. Finds the Notepad window by partial title match
2. Retrieves a valid window handle
3. Activates the window, bringing it to the foreground

## Pass Criteria

- [ ] `list_monitors` returns at least 1 monitor (secondary preferred)
- [ ] `find` action returns success with at least one match
- [ ] `find` returns a valid window handle
- [ ] `activate` action returns success
- [ ] "After" screenshot shows Notepad in foreground
- [ ] Notepad title bar shows active/focused state
- [ ] All three screenshots captured successfully

## Failure Indicators

- No monitors detected
- Notepad window not found
- Invalid or null handle returned
- Activate action fails
- Notepad still in background after activation
- Any tool returns an error response

## Notes

- This is a multi-step workflow that tests tool chaining
- The intermediate screenshot verifies that `find` is non-destructive
- If Notepad is already in foreground, activate should still succeed (no-op)
- Window handles are session-specific and may change between tests
- Total execution time should be under 60 seconds

## Workflow Dependencies

This scenario establishes the **Find-and-Activate** pattern used by:
- TC-WORKFLOW-003 (Type text in window)
- TC-WORKFLOW-004 (Click button and verify state)
- TC-WORKFLOW-006 (Resize and screenshot window)
