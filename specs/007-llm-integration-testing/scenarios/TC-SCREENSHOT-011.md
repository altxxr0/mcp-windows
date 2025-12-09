# Test Case: TC-SCREENSHOT-011

## Metadata

| Field | Value |
|-------|-------|
| **ID** | TC-SCREENSHOT-011 |
| **Category** | SCREENSHOT |
| **Priority** | P2 |
| **Target App** | Notepad |
| **Target Monitor** | Primary |
| **Timeout** | 30 seconds |
| **Tools** | screenshot_control, window_management |
| **Dependencies** | None |

## Objective

Verify that the screenshot_control tool correctly handles attempts to capture a minimized window, returning the `WindowMinimized` error (code 3).

## Preconditions

- [ ] Notepad is installed (Windows 11 built-in)
- [ ] Notepad is not already running
- [ ] Primary monitor is active
- [ ] Window can be minimized and restored

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

Then type "notepad" and press Enter, wait for window to open.

### Step 2: Get Notepad Window Handle

**MCP Tool**: `window_management`  
**Action**: `find`  
**Parameters**:
- `title`: `"Notepad"`

**Record**: The window handle from the response

### Step 3: Capture Notepad While Visible

**MCP Tool**: `screenshot_control`  
**Action**: `capture`  
**Parameters**:
- `target`: `"window"`
- `windowHandle`: `{notepad_handle}`
- `includeCursor`: `true`

**Save As**: `visible.png`

**Expected**: Success - image data returned

### Step 4: Minimize Notepad Window

**MCP Tool**: `window_management`  
**Action**: `minimize`  
**Parameters**:
- `handle`: `{notepad_handle}`

**Expected**: Window minimized

### Step 5: Attempt Capture of Minimized Window

**MCP Tool**: `screenshot_control`  
**Action**: `capture`  
**Parameters**:
- `target`: `"window"`
- `windowHandle`: `{notepad_handle}`
- `includeCursor`: `true`

**Expected**: Error response with error code 3 or message containing "minimized"

### Step 6: Restore Notepad Window

**MCP Tool**: `window_management`  
**Action**: `restore`  
**Parameters**:
- `handle`: `{notepad_handle}`

**Expected**: Window restored to visible state

### Step 7: Verify Capture Works Again

**MCP Tool**: `screenshot_control`  
**Action**: `capture`  
**Parameters**:
- `target`: `"window"`
- `windowHandle`: `{notepad_handle}`
- `includeCursor`: `true`

**Save As**: `restored.png`

**Expected**: Success - image data returned

### Step 8: Cleanup - Close Notepad

**MCP Tool**: `window_management`  
**Action**: `close`  
**Parameters**:
- `handle`: `{notepad_handle}`

## Expected Result

The screenshot_control tool:
- Successfully captures a visible window
- Returns `WindowMinimized` error when attempting to capture minimized window
- Error message is clear and descriptive (includes window state)
- Successfully captures again after window is restored
- Maintains stability throughout state transitions

## Pass Criteria

- [ ] Capture of visible window returns success with image data
- [ ] Capture of minimized window returns error code 3 (WindowMinimized)
- [ ] Error message includes context about minimized state
- [ ] Capture of restored window returns success with image data
- [ ] Tool does not crash or hang during state transitions
- [ ] Same window handle works for all operations

## Failure Indicators

- Capture of minimized window succeeds with image (bug: state check missing)
- Tool crashes or hangs
- Error message is generic without state information
- Wrong error code returned
- Restored window capture fails

## Notes

- This test validates the window state validation in IScreenshotService
- PrintWindow API call may fail or return blank image for minimized windows
- Error code should be `WindowMinimized = 3` from ScreenshotErrorCode enum
- Exposes if state validation happens before or after attempting capture
- Important for reliability when windows change state during automation

