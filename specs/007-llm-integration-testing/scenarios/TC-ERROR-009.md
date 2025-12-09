# Test Case: TC-ERROR-009

## Metadata

| Field | Value |
|-------|-------|
| **ID** | TC-ERROR-009 |
| **Category** | ERROR |
| **Priority** | P2 |
| **Target App** | None |
| **Target Monitor** | Primary |
| **Timeout** | 30 seconds |
| **Tools** | keyboard_control, screenshot_control |
| **Dependencies** | None |

## Objective

Verify that the keyboard_control tool correctly detects and reports when attempting to hold a key that is already held, returning the `KeyAlreadyHeld` error (code 11).

## Preconditions

- [ ] No keys are currently held down
- [ ] Keyboard layout is standard (en-US or compatible)
- [ ] No modal dialogs blocking

## Steps

### Step 1: Before Screenshot

**MCP Tool**: `screenshot_control`  
**Action**: `capture`  
**Parameters**: `target="primary_screen"`, `includeCursor=true`  
**Save As**: `before.png`

### Step 2: Hold Down Shift Key (First Time)

**MCP Tool**: `keyboard_control`  
**Action**: `key_down`  
**Parameters**:
- `key`: `"shift"`

**Expected**: Success response

### Step 3: Attempt to Hold Shift Again (Should Fail)

**MCP Tool**: `keyboard_control`  
**Action**: `key_down`  
**Parameters**:
- `key`: `"shift"`

**Expected**: Error response with error code 11 or error message containing "already held"

### Step 4: Release Shift Key

**MCP Tool**: `keyboard_control`  
**Action**: `key_up`  
**Parameters**:
- `key`: `"shift"`

**Expected**: Success response

### Step 5: Attempt Alt+Shift Edge Case

Hold Alt, then try to hold Alt again (test modifier key tracking)

**MCP Tool**: `keyboard_control`  
**Action**: `key_down`  
**Parameters**:
- `key`: `"alt"`

**Expected**: Success

**MCP Tool**: `keyboard_control`  
**Action**: `key_down`  
**Parameters**:
- `key`: `"alt"`

**Expected**: Error with `KeyAlreadyHeld`

### Step 6: Cleanup - Release All Keys

**MCP Tool**: `keyboard_control`  
**Action**: `release_all`

**Expected**: Success and all held keys cleared

### Step 7: After Screenshot

**MCP Tool**: `screenshot_control`  
**Action**: `capture`  
**Parameters**: `target="primary_screen"`, `includeCursor=true`  
**Save As**: `after.png`

## Expected Result

The tool correctly identifies when a key is already held and returns an appropriate error:
- First `key_down` on Shift succeeds
- Second `key_down` on same Shift key fails with `KeyAlreadyHeld` error
- Error message is descriptive (e.g., "Key 'shift' is already being held")
- Same behavior applies to Alt, Ctrl, and other modifier keys
- System state remains stable and responsive

## Pass Criteria

- [ ] First `key_down` returns success
- [ ] Second `key_down` returns error with code 11 or message containing "already held"
- [ ] Error message includes the key name ("shift")
- [ ] Tool remains responsive after error
- [ ] `release_all` successfully clears held keys
- [ ] Before/after screenshots show no unintended side effects

## Failure Indicators

- Second `key_down` returns success (bug: key tracking failure)
- Error message is cryptic or generic
- Tool crashes or hangs
- Keys remain stuck after error

## Notes

- This tests the held key tracking mechanism in KeyboardInputService
- The error code should be `KeyAlreadyHeld = 11` from KeyboardControlErrorCode enum
- This edge case could be missed if key tracking isn't properly implemented
- Critical for preventing stuck key scenarios in automation

