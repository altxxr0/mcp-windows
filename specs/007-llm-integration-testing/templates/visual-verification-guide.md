# Visual Verification Guide

This guide documents patterns and best practices for LLM-based visual verification in test scenarios.

## Purpose

Visual verification uses the LLM's ability to analyze screenshots and compare before/after states to determine if an action produced the expected result. This is the core mechanism for test validation in the LLM-based integration testing framework.

## Core Principles

### 1. Capture Before State
Always capture a screenshot **before** the action under test:
- Document what's visible
- Note specific elements to watch for
- Establish a baseline for comparison

### 2. Perform Action
Execute the MCP tool action being tested:
- Single action per verification (when possible)
- Allow time for UI to update
- Capture any return values

### 3. Capture After State
Capture a screenshot **after** the action:
- Use same capture parameters as before
- Same monitor, same region (if applicable)
- Consistent cursor inclusion setting

### 4. Compare and Verify
Have the LLM compare screenshots and identify:
- What changed
- Does the change match expectations
- Any unexpected changes

## Verification Prompts

### Window Position Change

```
Compare these two screenshots. Identify:
1. What changed between the before and after images?
2. Did the [window name] window move? If so, in what direction?
3. Did the window size change?
4. Are there any other differences?
```

### Text Content Change

```
Compare these two screenshots of [application]. Identify:
1. What text is visible in the 'before' screenshot?
2. What text is visible in the 'after' screenshot?
3. What text was added, modified, or removed?
4. Is the text change what we expected?
```

### No Change (Negative Test)

```
Compare these two screenshots carefully.
1. Are there ANY differences between the before and after images?
2. Has the window position changed?
3. Has any text changed?
4. If you see no changes, confirm that the images appear identical.
```

### UI State Change

```
Compare these two screenshots of [application].
1. What is the state of [element] in the 'before' image?
2. What is the state of [element] in the 'after' image?
3. Did the action cause the expected change?
4. Are there any unexpected side effects?
```

### Window Close

```
Compare these two screenshots.
1. Is [window name] visible in the 'before' screenshot?
2. Is [window name] visible in the 'after' screenshot?
3. If not visible, what is shown in its place?
4. Confirm whether the window was successfully closed.
```

## Best Practices

### For Multi-Monitor Environments

1. **Check window's `monitor_index`**: After any window operation, verify which monitor the window is on by checking the `monitor_index` field in the response
2. **Capture the correct monitor**: Always capture screenshots from the monitor where the target window is located, not a fixed monitor index
3. **Capture both monitors for positioning tests**: When testing window move/position operations, consider capturing both monitors to detect if the window unexpectedly moved to a different monitor or is straddling monitors
4. **Verify window bounds are within monitor**: Check that the window's x, y, width, height keep it fully within a single monitor's bounds
5. **Watch for cross-monitor straddling**: A bug may cause windows to span multiple monitors - calculate if `x + width` exceeds the current monitor's boundary

### For Reliable Detection

1. **Use high contrast**: Dark text on light backgrounds
2. **Use standard fonts**: System fonts render consistently
3. **Avoid animations**: Wait for animations to complete
4. **Use sufficient size**: Larger UI elements are easier to detect
5. **Static backgrounds**: Minimize desktop wallpaper interference

### For Accurate Comparison

1. **Same capture parameters**: Consistency between before/after
2. **Exclude cursor when comparing**: Cursor position may change
3. **Focus on specific regions**: Large screenshots have more noise
4. **One change at a time**: Isolate the change being tested
5. **Capture based on window location**: Use the window's `monitor_index` to determine which monitor to screenshot

### For Clear Pass/Fail

1. **Define specific expectations**: "Window at (500, 300)" not "Window moved"
2. **Use measurable criteria**: Text content, coordinates, visibility
3. **Document expected vs actual**: Compare specific values
4. **Handle minor variations**: Pixel-perfect not required

## Common Patterns

### Before/After with Move Action

```
1. Screenshot before (note position)
2. window_management move action
3. Screenshot after (note new position)
4. LLM compares: Did position change correctly?
```

### Before/After with Type Action

```
1. Screenshot before (note text content)
2. keyboard_control type action
3. Screenshot after (note new text)
4. LLM compares: Does text match expected?
```

### Before/After with State Toggle

```
1. Screenshot before (note window state: normal)
2. window_management maximize action
3. Screenshot after (note window state: maximized)
4. LLM compares: Did state change correctly?
```

## Limitations

### LLM Visual Analysis Limitations

- **Small text**: May be hard to read
- **Similar colors**: Low contrast reduces accuracy
- **Complex layouts**: Dense UIs may be confusing
- **Animations**: Captured mid-animation may be unclear
- **Dynamic content**: Clock, timestamps may change

### Mitigation Strategies

- Use larger windows and fonts
- Simplify the visual environment
- Wait for UI to settle before capture
- Mask or ignore known dynamic areas
- Use focused region captures instead of full screen

## Screenshot Naming Convention

- `before.png` - State before action
- `after.png` - State after action
- `context.png` - Additional context screenshot
- `step-N.png` - Multi-step workflow screenshots

---

## All-Monitors Composite Screenshot Patterns

For multi-monitor environments, use the `target="all_monitors"` option to capture all connected monitors as a single composite image with metadata describing each monitor's region.

### When to Use All-Monitors Capture

1. **Cross-monitor window movement**: When a window may move between monitors
2. **Multi-monitor layouts**: When verifying window positions across multiple displays
3. **Secondary monitor testing**: When changes occur on non-primary monitors
4. **Full desktop state**: When you need to see the entire desktop environment

### All-Monitors Capture Pattern

```
1. screenshot_control capture target="all_monitors"
2. Parse compositeMetadata for monitor regions
3. Perform action under test
4. screenshot_control capture target="all_monitors"
5. Compare specific monitor regions using metadata
```

### Response Structure

The all-monitors capture returns:

```json
{
  "success": true,
  "imageData": "<base64 PNG>",
  "width": 3840,
  "height": 1080,
  "format": "png",
  "compositeMetadata": {
    "capture_time": "2025-01-15T10:30:00Z",
    "virtual_screen": { "x": 0, "y": 0, "width": 3840, "height": 1080 },
    "monitors": [
      { "index": 0, "x": 0, "y": 0, "width": 1920, "height": 1080, "is_primary": true },
      { "index": 1, "x": 1920, "y": 0, "width": 1920, "height": 1080, "is_primary": false }
    ],
    "image_width": 3840,
    "image_height": 1080
  }
}
```

### Key Concepts

- **Virtual Screen**: The bounding rectangle containing all monitors (may have negative X/Y)
- **Monitor Regions**: Each monitor's position within the composite image (always non-negative)
- **Region Isolation**: Use region coordinates to analyze specific monitors

### Visual Verification Prompts for All-Monitors

#### Cross-Monitor Window Movement

```
Compare these two all-monitors composite screenshots.
Using the monitor region metadata:
1. In the 'before' image, which monitor region contains the [window] window?
2. In the 'after' image, which monitor region contains the [window] window?
3. Did the window move between monitors or within the same monitor?
4. Describe the window's position relative to its containing monitor.
```

#### Secondary Monitor Changes

```
Compare these composite screenshots, focusing on the secondary monitor region.
Using compositeMetadata.monitors to identify the secondary monitor:
1. What content is visible in the secondary monitor region of the 'before' image?
2. What changed in the secondary monitor region of the 'after' image?
3. Did anything change in the primary monitor region?
4. Are the changes isolated to the expected monitor?
```

#### Multi-Monitor Window State

```
Analyze this all-monitors composite screenshot.
For each monitor region in compositeMetadata.monitors:
1. List any visible windows within that monitor's region
2. Describe each window's state (normal, maximized, minimized if visible)
3. Identify which applications are running on which monitors
```

### All-Monitors Best Practices

1. **Save metadata alongside screenshots**: Store `step-N-before-meta.json` with `step-N-before.png`
2. **Use consistent capture**: Same `includeCursor` setting for before/after
3. **Parse regions for analysis**: Extract specific monitor regions when comparing
4. **Handle negative coordinates**: Virtual screen origin may be negative if monitors are left/above primary
5. **Validate monitor count**: Ensure expected number of monitors in response

### File Naming for All-Monitors Captures

```
step-1-before.png           # Before composite screenshot
step-1-before-meta.json     # Before composite metadata
step-1-after.png            # After composite screenshot  
step-1-after-meta.json      # After composite metadata
step-1-diff.json            # Visual diff result (optional)
step-1-diff.png             # Diff image highlighting changes (optional)
```

### Visual Diff Integration

The VisualDiffService can compare composite screenshots:

```json
{
  "before_image": "step-1-before.png",
  "after_image": "step-1-after.png",
  "changed_pixels": 45000,
  "total_pixels": 4147200,
  "change_percentage": 1.08,
  "threshold": 1.0,
  "is_significant_change": true,
  "diff_image_data": "<base64 PNG with red highlights>"
}
```

The diff image shows changed pixels highlighted in red, making it easy to identify what changed across the entire desktop.

