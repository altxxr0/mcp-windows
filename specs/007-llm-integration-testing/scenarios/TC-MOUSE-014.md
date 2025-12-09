# Test Case: TC-MOUSE-014

## Metadata

| Field | Value |
|-------|-------|
| **ID** | TC-MOUSE-014 |
| **Category** | MOUSE |
| **Priority** | P2 |
| **Target App** | Notepad |
| **Target Monitor** | Primary |
| **Timeout** | 30 seconds |
| **Tools** | mouse_control, screenshot_control, window_management |
| **Dependencies** | TC-MOUSE-001 |

## Objective

Verify that the mouse_control tool correctly executes drag operations using right-click and middle-click buttons, not just the default left-click button.

## Preconditions

- [ ] Notepad is installed (Windows 11 built-in)
- [ ] Notepad is not already running
- [ ] Primary monitor is active
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

### Step 2: Create Test Content

**MCP Tool**: `keyboard_control`  
**Action**: `type`  
**Parameters**:
- `text`: `"Item1\nItem2\nItem3"`

### Step 3: Before Screenshot

**MCP Tool**: `screenshot_control`  
**Action**: `capture`  
**Parameters**: `target="primary_screen"`, `includeCursor=true`  
**Save As**: `before.png`

### Step 4: Right-Click Drag (Button = "right")

Select "Item1" by dragging with right button from start to end of text.

**MCP Tool**: `mouse_control`  
**Action**: `drag`  
**Parameters**:
- `x`: `20`
- `y`: `20`
- `endX`: `80`
- `endY`: `20`
- `button`: `"right"`

**Expected**: Right-click drag executes (may open context menu or perform right-drag behavior depending on application)

### Step 5: Close Context Menu

**MCP Tool**: `keyboard_control`  
**Action**: `press`  
**Parameters**:
- `key`: `"escape"`

### Step 6: Middle-Click Drag (Button = "middle")

Test middle-click drag operation.

**MCP Tool**: `mouse_control`  
**Action**: `drag`  
**Parameters**:
- `x`: `20`
- `y`: `40`
- `endX`: `100`
- `endY`: `40`
- `button`: `"middle"`

**Expected**: Middle-click drag executes (behavior depends on application - may paste or perform auto-scroll)

### Step 7: Left-Click Drag (Button = "left" or Default)

Test left-click drag for comparison.

**MCP Tool**: `mouse_control`  
**Action**: `drag`  
**Parameters**:
- `x`: `20`
- `y`: `60`
- `endX`: `120`
- `endY`: `60`
- `button`: `"left"`

**Expected**: Left-click drag (default) performs normal selection

### Step 8: After Screenshot

**MCP Tool**: `screenshot_control`  
**Action**: `capture`  
**Parameters**: `target="primary_screen"`, `includeCursor=true`  
**Save As**: `after.png`

### Step 9: Cleanup - Close Notepad

**MCP Tool**: `window_management`  
**Action**: `close`  
**Parameters**:
- `handle`: `{notepad_handle}`

## Expected Result

The drag operation executes successfully with all three mouse button types:
- Right-click drag: executes and returns success
- Middle-click drag: executes and returns success
- Left-click drag: executes and returns success (default behavior)
- Each button type produces appropriate application response
- No crashes or errors

## Pass Criteria

- [ ] `mouse_control` drag with `button="right"` returns success
- [ ] `mouse_control` drag with `button="middle"` returns success
- [ ] `mouse_control` drag with `button="left"` returns success
- [ ] Tool does not crash when different button types are used
- [ ] Application responds appropriately to each drag type
- [ ] No unhandled exceptions thrown

## Failure Indicators

- Drag fails with unsupported button error
- Tool crashes or hangs
- Button parameter is ignored (always performs left-click)
- Wrong button is pressed (sends left when middle requested)
- Error message indicates button validation issue

## Notes

- This test exposes if the button parameter is properly wired through the drag operation
- Currently TC-MOUSE-011 only tests left-click drag
- Middle-click may trigger paste in some applications
- Right-click may trigger context menus
- This is a gap in multi-button drag coverage

