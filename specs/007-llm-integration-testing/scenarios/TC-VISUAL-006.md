# Test Case: TC-VISUAL-006

## Metadata

| Field | Value |
|-------|-------|
| **ID** | TC-VISUAL-006 |
| **Category** | VISUAL |
| **Priority** | P1 |
| **Target App** | Any |
| **Target Monitor** | Secondary |
| **Timeout** | 60 seconds |

## Objective

Verify that changes occurring specifically on a secondary monitor can be detected using composite all-monitors screenshots and monitor region metadata.

## Preconditions

- [ ] Multi-monitor setup (2+ monitors required)
- [ ] Secondary monitor is active and displaying content
- [ ] Ability to trigger a visible change on secondary monitor

## Steps

### Step 1: Detect Monitor Configuration

**MCP Tool**: `screenshot_control`  
**Action**: `list_monitors`  
**Purpose**: Identify secondary monitor index and position

Record:
- Secondary monitor index (typically 1 or higher)
- Secondary monitor bounds (X, Y, Width, Height)

### Step 2: Position Window on Secondary Monitor

**MCP Tool**: `window_management`  
**Action**: `find`  
**Parameters**: Title containing target app name

Then:

**MCP Tool**: `window_management`  
**Action**: `move`  
**Parameters**: Move window to secondary monitor region

### Step 3: Before Screenshot (All Monitors)

**MCP Tool**: `screenshot_control`  
**Action**: `capture`  
**Parameters**: `target="all_monitors"`  
**Save As**: `step-1-before.png`

**LLM Analysis**: Using `compositeMetadata.monitors`:
- Identify the secondary monitor region by index
- Describe what's visible in that region only
- Note window position within secondary monitor bounds

### Step 4: Trigger Change on Secondary Monitor

Perform an action that modifies the secondary monitor content:
- Open a new window on secondary
- Move an existing window within secondary
- Change window state (maximize, minimize)
- Type text in an application on secondary

### Step 5: After Screenshot (All Monitors)

**MCP Tool**: `screenshot_control`  
**Action**: `capture`  
**Parameters**: `target="all_monitors"`  
**Save As**: `step-1-after.png`

### Step 6: Region-Focused Comparison

Compare the two composite screenshots, focusing specifically on the secondary monitor region:

Using `compositeMetadata.monitors[secondary_index]`:
- Extract secondary region from "before": `{x, y, width, height}`
- Extract same region from "after"
- Compare only these regions for changes

Questions to answer:
1. What changed in the secondary monitor region?
2. Did anything change in the primary monitor region?
3. Are the changes isolated to the expected monitor?

## Expected Result

The LLM correctly identifies that:
1. Changes occurred on the secondary monitor
2. The primary monitor content remained static
3. The change matches the action performed in Step 4

## Pass Criteria

- [ ] All-monitors capture includes valid secondary monitor region
- [ ] LLM correctly identifies changes in secondary monitor region
- [ ] LLM correctly identifies primary monitor is unchanged
- [ ] Changes detected match the action performed
- [ ] Region isolation works correctly (no false positives on primary)

## Failure Indicators

- Cannot identify secondary monitor in composite
- LLM reports changes on wrong monitor
- LLM misses changes on secondary
- LLM reports false changes on primary
- Region coordinates don't isolate monitors correctly

## Visual Diff Integration

If using VisualDiffService with the composite screenshots:

```json
{
  "changedPixels": 45000,
  "totalPixels": 3686400,
  "changePercentage": 1.22,
  "isSignificantChange": true,
  "threshold": 1.0
}
```

The diff should show changes concentrated in the secondary monitor region, with the primary monitor region showing minimal or no change.

## Region Extraction Example

Given `compositeMetadata`:
```json
{
  "virtual_screen": { "x": 0, "y": 0, "width": 3840, "height": 1080 },
  "monitors": [
    { "index": 0, "x": 0, "y": 0, "width": 1920, "height": 1080, "is_primary": true },
    { "index": 1, "x": 1920, "y": 0, "width": 1920, "height": 1080, "is_primary": false }
  ]
}
```

To analyze secondary monitor:
- Focus on pixels from X:1920 to X:3840, Y:0 to Y:1080
- This region represents the secondary monitor content

## Notes

- Critical for validating multi-monitor awareness
- Proves that all-monitors capture correctly stitches and maps monitors
- Enables testing of applications that span or move between monitors
- Foundation for scenarios where actions on one monitor should not affect others
- Consider extending to 3+ monitor configurations for comprehensive testing
