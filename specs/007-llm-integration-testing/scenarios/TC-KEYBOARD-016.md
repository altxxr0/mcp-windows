# Test Case: TC-KEYBOARD-016

## Metadata

| Field | Value |
|-------|-------|
| **ID** | TC-KEYBOARD-016 |
| **Category** | KEYBOARD |
| **Priority** | P2 |
| **Target App** | Notepad |
| **Target Monitor** | Primary |
| **Timeout** | 60 seconds |
| **Tools** | keyboard_control, screenshot_control, window_management |
| **Dependencies** | TC-KEYBOARD-001 |

## Objective

Verify that the keyboard_control tool correctly handles typing very long text (5000+ characters) through the chunking mechanism, validating that no characters are lost and the process completes successfully.

## Preconditions

- [ ] Notepad is installed (Windows 11 built-in)
- [ ] Notepad is not already running
- [ ] Primary monitor is active
- [ ] Sufficient timeout for large text (60+ seconds)
- [ ] No modal dialogs or interruptions

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

### Step 2: Get Notepad Window Handle and Focus

**MCP Tool**: `window_management`  
**Action**: `find`  
**Parameters**:
- `title`: `"Notepad"`

**MCP Tool**: `window_management`  
**Action**: `activate`  
**Parameters**:
- `handle`: `{notepad_handle}`

### Step 3: Before Screenshot

**MCP Tool**: `screenshot_control`  
**Action**: `capture`  
**Parameters**: `target="primary_screen"`, `includeCursor=true`  
**Save As**: `before.png`

### Step 4: Type 5000 Character Text (Test Chunking)

Generate a long text string for testing. The text should be:
- Approximately 5000 characters
- Mix of ASCII and special characters
- Include numbers, punctuation, and letters
- Pattern: "The quick brown fox jumps over the lazy dog. " repeated ~110 times

**MCP Tool**: `keyboard_control`  
**Action**: `type`  
**Parameters**:
- `text`: `{5000_character_string}`

**Expected**: Success response with character count = 5000

Record the response time and character count returned.

### Step 5: Verify Text was Typed Completely

**MCP Tool**: `window_management`  
**Action**: `find`  
**Parameters**:
- `title`: `"Notepad"`

**MCP Tool**: `keyboard_control`  
**Action**: `combo`  
**Parameters**:
- `key`: `"end"`
- `modifiers`: `"ctrl"`

Move to end of document to verify all text was entered.

### Step 6: After Screenshot

**MCP Tool**: `screenshot_control`  
**Action**: `capture`  
**Parameters**: `target="primary_screen"`, `includeCursor=true`  
**Save As**: `after.png`

**Verify**: After screenshot shows scrolled Notepad with text content

### Step 7: Character Count Verification (Using Notepad Status)

Use Notepad's Status Bar or selection all text with Ctrl+A and check if any status information is available.

**MCP Tool**: `keyboard_control`  
**Action**: `combo`  
**Parameters**:
- `key`: `"a"`
- `modifiers`: `"ctrl"`

**Note**: On Windows 11, Notepad shows character count in status bar (if enabled).

### Step 8: Performance Analysis

Record:
- Total time to complete typing (from step 4 response time)
- Character count returned by keyboard_control
- Verify count matches expected 5000

Expected: < 10 seconds for 5000 characters (depends on inter-key delay configuration)

### Step 9: Cleanup - Close Notepad Without Saving

**MCP Tool**: `window_management`  
**Action**: `close`  
**Parameters**:
- `handle`: `{notepad_handle}`

**MCP Tool**: `keyboard_control`  
**Action**: `press`  
**Parameters**:
- `key`: `"n"`

(Press "N" for "Don't Save" if prompted)

## Expected Result

The keyboard_control tool:
- Successfully types all 5000 characters without data loss
- Returns success status with accurate character count
- Completes within reasonable timeout (< 60 seconds)
- No intermediate errors or hangs
- Chunk mechanism works transparently (user doesn't see delays between chunks)
- Handles special characters and mixed content correctly
- Cleanup (closing Notepad) does not prompt to save if no errors occurred

## Pass Criteria

- [ ] `keyboard_control` type action returns success
- [ ] Returned character count equals 5000
- [ ] No characters missing from typed text (verify via screenshot content analysis)
- [ ] Operation completes within 60-second timeout
- [ ] After screenshot shows expected text content
- [ ] Tool remains responsive (not hanging between chunks)
- [ ] No errors returned during chunking process

## Failure Indicators

- Character count mismatch (fewer characters than 5000)
- Tool timeout during typing
- Unhandled exception or crash
- Some characters missing from output
- Operation returns error mid-sequence
- System becomes unresponsive during chunking

## Notes

- This test validates the chunking mechanism in KeyboardInputService (default chunk size ~1000)
- Exposes potential timing issues between chunks
- Reveals if inter-key delays are applied globally or per-chunk
- Performance metric: Helps identify if chunking introduces bottlenecks
- Control character handling in long text is important
- This gap exists because previous tests only use small text snippets
- Critical for use cases like pasting large documents

