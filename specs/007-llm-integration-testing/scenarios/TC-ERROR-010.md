# Test Case: TC-ERROR-010

## Metadata

| Field | Value |
|-------|-------|
| **ID** | TC-ERROR-010 |
| **Category** | ERROR |
| **Priority** | P2 |
| **Target App** | None |
| **Target Monitor** | Primary |
| **Timeout** | 30 seconds |
| **Tools** | keyboard_control, screenshot_control |
| **Dependencies** | None |

## Objective

Verify that the keyboard_control tool correctly detects and reports when attempting to release a key that is not currently held, returning the `KeyNotHeld` error (code 8).

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

### Step 2: Attempt to Release Enter (Never Held)

**MCP Tool**: `keyboard_control`  
**Action**: `key_up`  
**Parameters**:
- `key`: `"enter"`

**Expected**: Error response with error code 8 or message containing "not held"

### Step 3: Hold Shift, Release Different Key

**MCP Tool**: `keyboard_control`  
**Action**: `key_down`  
**Parameters**:
- `key`: `"shift"`

**Expected**: Success

**MCP Tool**: `keyboard_control`  
**Action**: `key_up`  
**Parameters**:
- `key`: `"ctrl"`

**Expected**: Error with `KeyNotHeld` (Ctrl was never held, only Shift is held)

### Step 4: Hold Shift, Alt in Sequence

**MCP Tool**: `keyboard_control`  
**Action**: `key_down`  
**Parameters**:
- `key`: `"alt"`

**Expected**: Success (now Shift and Alt held)

**MCP Tool**: `keyboard_control`  
**Action**: `key_up`  
**Parameters**:
- `key`: `"shift"`

**Expected**: Success (Shift was held)

**MCP Tool**: `keyboard_control`  
**Action**: `key_up`  
**Parameters**:
- `key`: `"shift"`

**Expected**: Error with `KeyNotHeld` (Shift already released)

### Step 5: Cleanup - Release Remaining Keys

**MCP Tool**: `keyboard_control`  
**Action**: `key_up`  
**Parameters**:
- `key`: `"alt"`

**Expected**: Success

**MCP Tool**: `keyboard_control`  
**Action**: `release_all`

**Expected**: Success (should be no-op now)

### Step 6: After Screenshot

**MCP Tool**: `screenshot_control`  
**Action**: `capture`  
**Parameters**: `target="primary_screen"`, `includeCursor=true`  
**Save As**: `after.png`

## Expected Result

The tool correctly validates that a key is held before allowing release:
- Attempting to release a never-held key returns `KeyNotHeld` error
- Attempting to release the same key twice returns error on second attempt
- Error message includes the key name
- Held keys are tracked accurately across multiple operations
- System remains stable and responsive

## Pass Criteria

- [ ] First `key_up` on never-held Enter returns `KeyNotHeld` error
- [ ] `key_up` on Ctrl (when only Shift held) returns `KeyNotHeld` error
- [ ] Releasing same key twice shows error on second attempt
- [ ] Error messages include the key name
- [ ] Tool remains responsive after each error
- [ ] Before/after screenshots show no unintended side effects

## Failure Indicators

- `key_up` on never-held key succeeds (bug: tracking failure)
- Tool crashes or hangs
- Error message is generic without key name
- Held keys get accidentally released

## Notes

- This tests the validation logic in KeyboardInputService before calling SendInput for key up
- The error code should be `KeyNotHeld = 8` from KeyboardControlErrorCode enum
- This prevents accidental key release errors in complex automation sequences
- Works in tandem with TC-ERROR-009 to comprehensively test key state tracking

