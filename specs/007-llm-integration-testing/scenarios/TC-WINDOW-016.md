# Test Case: TC-WINDOW-016

## Metadata

| Field | Value |
|-------|-------|
| **ID** | TC-WINDOW-016 |
| **Category** | WINDOW |
| **Priority** | P2 |
| **Target App** | Notepad |
| **Target Monitor** | Primary |
| **Timeout** | 30 seconds |
| **Tools** | window_management, screenshot_control |
| **Dependencies** | TC-WINDOW-010 |

## Objective

Verify that the window_management tool correctly validates resize parameters and returns appropriate errors when attempting to resize a window to zero or invalid dimensions.

## Preconditions

- [ ] Notepad is installed (Windows 11 built-in)
- [ ] Notepad is not already running
- [ ] Primary monitor is active
- [ ] Window can be resized
- [ ] No modal dialogs blocking

## Steps

### Step 1: Open Notepad

**MCP Tool**: `window_management`  
**Action**: `find`  
**Parameters**:
- `title`: `"Notepad"`

If not found, use keyboard shortcut:

**MCP Tool**: `keyboard_control`  
**Action**: `combo`  
**Parameters**:
- `key`: `"r"`
- `modifiers`: `"win"`

Then type "notepad" and press Enter.

### Step 2: Get Notepad Window Handle

**MCP Tool**: `window_management`  
**Action**: `find`  
**Parameters**:
- `title`: `"Notepad"`

**Record**: The window handle

### Step 3: Before Screenshot

**MCP Tool**: `screenshot_control`  
**Action**: `capture`  
**Parameters**: `target="primary_screen"`, `includeCursor=true`  
**Save As**: `before.png`

### Step 4: Verify Initial Size

**MCP Tool**: `window_management`  
**Action**: `activate`  
**Parameters**:
- `handle`: `{notepad_handle}`

### Step 5: Resize to Normal Dimensions (Baseline)

**MCP Tool**: `window_management`  
**Action**: `resize`  
**Parameters**:
- `handle`: `{notepad_handle}`
- `width`: `400`
- `height`: `300`

**Expected**: Success

### Step 6: Attempt Resize with Width = 0

**MCP Tool**: `window_management`  
**Action**: `resize`  
**Parameters**:
- `handle`: `{notepad_handle}`
- `width`: `0`
- `height`: `300`

**Expected**: Error response (should reject zero or negative dimensions)

### Step 7: Attempt Resize with Height = 0

**MCP Tool**: `window_management`  
**Action**: `resize`  
**Parameters**:
- `handle`: `{notepad_handle}`
- `width`: `400`
- `height`: `0`

**Expected**: Error response

### Step 8: Attempt Resize with Both Dimensions = 0

**MCP Tool**: `window_management`  
**Action**: `resize`  
**Parameters**:
- `handle`: `{notepad_handle}`
- `width`: `0`
- `height`: `0`

**Expected**: Error response

### Step 9: Attempt Resize with Negative Width

**MCP Tool**: `window_management`  
**Action**: `resize`  
**Parameters**:
- `handle`: `{notepad_handle}`
- `width`: `-100`
- `height`: `300`

**Expected**: Error response

### Step 10: Attempt Resize with Negative Height

**MCP Tool**: `window_management`  
**Action**: `resize`  
**Parameters**:
- `handle`: `{notepad_handle}`
- `width`: `400`
- `height`: `-100`

**Expected**: Error response

### Step 11: Verify Window Still Exists and Responds

Resize back to normal to verify window is still functional.

**MCP Tool**: `window_management`  
**Action**: `resize`  
**Parameters**:
- `handle`: `{notepad_handle}`
- `width`: `500`
- `height`: `400`

**Expected**: Success

### Step 12: After Screenshot

**MCP Tool**: `screenshot_control`  
**Action**: `capture`  
**Parameters**: `target="primary_screen"`, `includeCursor=true`  
**Save As**: `after.png`

### Step 13: Cleanup - Close Notepad

**MCP Tool**: `window_management`  
**Action**: `close`  
**Parameters**:
- `handle`: `{notepad_handle}`

## Expected Result

The window_management tool:
- Successfully resizes to normal dimensions
- Rejects all invalid dimension parameters (0, negative values)
- Returns descriptive error messages for invalid inputs
- Does not crash the window or system
- Window remains usable after invalid resize attempts
- Validation happens before attempting the resize operation

## Pass Criteria

- [ ] Resize to (400, 300) succeeds
- [ ] Resize with width=0 returns error (InvalidCoordinates or similar)
- [ ] Resize with height=0 returns error
- [ ] Resize with width=-100 returns error
- [ ] Resize with height=-100 returns error
- [ ] Error messages mention dimension constraints or validity
- [ ] Window remains responsive after each error
- [ ] Resize to (500, 400) succeeds after errors

## Failure Indicators

- Any zero/negative resize succeeds (bug: validation missing)
- Tool crashes or window becomes unusable
- Unhandled exception thrown
- Error messages are generic without context
- Window becomes corrupted (invisible, immovable)

## Notes

- This test validates input validation in IWindowService.ResizeAsync()
- Windows has minimum window size constraints (~75x50 pixels typically)
- Zero or negative dimensions should be rejected before calling SetWindowPos API
- Exposes if validation is defensive (happens before operation) or reactive (handles API errors)
- Critical for preventing visual glitches and invalid window states
- This is a gap in resize parameter validation coverage
- Related to TC-WINDOW-017 which tests negative coordinates for move operation

