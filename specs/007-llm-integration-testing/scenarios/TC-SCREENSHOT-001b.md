# Test Case: TC-SCREENSHOT-001b

## Metadata

| Field | Value |
|-------|-------|
| **ID** | TC-SCREENSHOT-001b |
| **Category** | SCREENSHOT |
| **Priority** | P1 |
| **Target App** | System |
| **Target Monitor** | All Monitors |
| **Timeout** | 60 seconds |

## Objective

Verify that monitor region metadata from an all-monitors composite screenshot can be used to identify and describe content on specific monitors.

## Preconditions

- [ ] Multi-monitor setup (2+ monitors recommended)
- [ ] Different content visible on each monitor (e.g., different applications)
- [ ] No secure desktop (UAC/lock screen)

## Steps

### Step 1: Capture All Monitors with Metadata

**MCP Tool**: `screenshot_control`  
**Action**: `capture`  
**Parameters**:
- `target`: `"all_monitors"`

### Step 2: Parse Monitor Regions

From the response, extract `compositeMetadata.monitors` and identify:
- Primary monitor (`is_primary: true`)
- Secondary monitor(s) (`is_primary: false`)
- Each monitor's region: `{x, y, width, height}`

### Step 3: Analyze Primary Monitor Region

Using the primary monitor's coordinates from the metadata:
- Describe what content is visible in that region of the composite image
- Identify the application(s) visible on the primary monitor

### Step 4: Analyze Secondary Monitor Region(s)

For each non-primary monitor:
- Use the region coordinates to identify the corresponding area in the composite
- Describe what content is visible in that region
- Identify the application(s) visible on that monitor

### Step 5: Verify Content Isolation

Confirm that:
- Content described for each monitor matches actual monitor content
- There is no content bleeding between monitor regions
- Each monitor's content is correctly isolated by its region bounds

## Expected Result

The LLM can use the monitor region metadata to accurately identify and describe content on each individual monitor within the composite screenshot, demonstrating that the metadata correctly maps to visual content.

## Pass Criteria

- [ ] All-monitors capture returns valid composite image with metadata
- [ ] Primary monitor region correctly identifies primary display content
- [ ] Secondary monitor region(s) correctly identify secondary display content
- [ ] Content descriptions for each monitor match actual visible content
- [ ] No confusion or mixing of content between monitors
- [ ] Region bounds correctly isolate each monitor's content

## Failure Indicators

- Cannot parse monitor regions from metadata
- Content described doesn't match actual monitor content
- Same content appears in multiple monitor descriptions
- Region coordinates don't match visual boundaries
- Missing or incorrect primary monitor identification

## Test Variations

### Multi-Monitor Configurations

| Config | Primary | Secondary | Notes |
|--------|---------|-----------|-------|
| 2 monitors horizontal | Left or Right | Remaining | Most common setup |
| 2 monitors vertical | Top or Bottom | Remaining | Stacked displays |
| 3+ monitors | User-defined | Multiple | Extended desktop |

### Content Differentiation

For reliable testing, ensure each monitor displays unique, identifiable content:
- Different applications open on each monitor
- Different wallpapers (if no apps)
- Distinct window titles visible

## Notes

- This scenario validates the usability of the metadata for real analysis tasks
- The metadata enables LLM to answer questions like "What's on monitor 2?"
- Without metadata, the LLM can only describe the composite as a whole
- This is foundational for multi-monitor workflow testing
- Consider pairing with window_management to position windows on specific monitors
