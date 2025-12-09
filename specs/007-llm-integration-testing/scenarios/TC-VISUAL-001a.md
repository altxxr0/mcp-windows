# Test Case: TC-VISUAL-001a

## Metadata

| Field | Value |
|-------|-------|
| **ID** | TC-VISUAL-001a |
| **Category** | VISUAL |
| **Priority** | P1 |
| **Target App** | Notepad |
| **Target Monitor** | All Monitors |
| **Timeout** | 90 seconds |

## Objective

Verify that the LLM can detect a window position change across monitors by comparing before/after composite screenshots using all-monitors capture and visual diff analysis.

## Preconditions

- [ ] Multi-monitor setup (2+ monitors recommended)
- [ ] Notepad is open in windowed mode
- [ ] Window position is within screen bounds

## Steps

### Step 1: Detect Monitor Configuration

**MCP Tool**: `screenshot_control`  
**Action**: `list_monitors`  
**Purpose**: Identify all monitors and their positions

Note the monitor indices and which one contains the Notepad window initially.

### Step 2: Setup Notepad on Primary Monitor

1. Open Notepad on primary monitor
2. Position at (200, 200) relative to primary monitor
3. Size to 600x400

### Step 3: Before Screenshot (All Monitors)

**MCP Tool**: `screenshot_control`  
**Action**: `capture`  
**Parameters**: `target="all_monitors"`, `includeCursor=false`  
**Save As**: `step-1-before.png`  
**Save Metadata**: `step-1-before-meta.json`

**LLM Observation**: 
- Note the position of Notepad window using `compositeMetadata.monitors[primary].x/y` offset
- Record which monitor region contains Notepad

### Step 4: Move Window to Different Monitor

**MCP Tool**: `window_management`  
**Action**: `move`  
**Parameters**:
- `handle`: `"{notepad_handle}"`
- `x`: Calculate target X on secondary monitor using its `compositeMetadata` region
- `y`: `300`

Move the window to the secondary monitor's region.

### Step 5: After Screenshot (All Monitors)

**MCP Tool**: `screenshot_control`  
**Action**: `capture`  
**Parameters**: `target="all_monitors"`, `includeCursor=false`  
**Save As**: `step-1-after.png`  
**Save Metadata**: `step-1-after-meta.json`

### Step 6: Visual Comparison

Compare the two composite screenshots:

1. **Monitor Change**: Did the window move between monitors?
2. **Source Monitor**: Which monitor region contained Notepad in "before"?
3. **Target Monitor**: Which monitor region contains Notepad in "after"?
4. **Position Within Monitor**: Where is the window within its new monitor region?

### Step 7: Visual Diff Analysis (Optional)

If using the VisualDiffService:
- Compare `step-1-before.png` with `step-1-after.png`
- Expected `changePercentage`: Significant (window moved across monitors)
- Expected `changedRegions`: Two regions - original and new window locations

## Expected Result

The LLM correctly identifies that the Notepad window moved from the primary monitor region to the secondary monitor region within the composite screenshot.

## Pass Criteria

- [ ] All-monitors capture returns valid composite with metadata
- [ ] LLM correctly identifies window was on primary monitor in "before"
- [ ] LLM correctly identifies window is on secondary monitor in "after"
- [ ] LLM correctly notes window moved between monitors (not just within one)
- [ ] LLM correctly describes relative position within each monitor region
- [ ] Monitor region metadata correctly maps to visual content

## Failure Indicators

- LLM reports no change when window moved
- LLM reports same monitor when window changed monitors
- LLM cannot identify which monitor region contains the window
- Composite metadata doesn't match visual layout
- Window appears in wrong region of composite

## Visual Verification Prompt

> "Compare these two all-monitors composite screenshots. Using the monitor region metadata:
> 1. In the 'before' image, which monitor region contains the Notepad window?
> 2. In the 'after' image, which monitor region contains the Notepad window?
> 3. Did the window move between monitors or within the same monitor?
> 4. Describe the window's position relative to its containing monitor."

## Notes

- This is a critical test for multi-monitor workflow validation
- Uses composite screenshots to track window movement across entire desktop
- Monitor region metadata enables precise identification of window location
- More reliable than single-monitor capture when windows can move between monitors
- Foundation for cross-monitor drag-and-drop testing
