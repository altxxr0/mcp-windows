# Test Case: TC-SCREENSHOT-001a

## Metadata

| Field | Value |
|-------|-------|
| **ID** | TC-SCREENSHOT-001a |
| **Category** | SCREENSHOT |
| **Priority** | P1 |
| **Target App** | System |
| **Target Monitor** | All Monitors |
| **Timeout** | 60 seconds |

## Objective

Verify that a composite screenshot capturing all connected monitors can be captured as a single image with metadata describing each monitor's region within the composite.

## Preconditions

- [ ] At least one monitor is connected (multi-monitor recommended for full validation)
- [ ] Desktop or applications visible on all screens
- [ ] No secure desktop (UAC/lock screen)

## Steps

### Step 1: Capture All Monitors

**MCP Tool**: `screenshot_control`  
**Action**: `capture`  
**Parameters**:
- `target`: `"all_monitors"`

### Step 2: Record Response

Save the response and verify it contains:
- `success`: `true`
- `imageData`: Base64-encoded PNG of composite image
- `width`: Total width spanning all monitors
- `height`: Total height spanning all monitors
- `format`: `"png"`
- `compositeMetadata`: Object containing:
  - `virtual_screen`: Bounding rectangle of all monitors (may have negative X/Y)
  - `monitors`: Array of monitor regions
  - `image_width`: Width of composite image
  - `image_height`: Height of composite image

### Step 3: Verify Composite Metadata

For each monitor in `compositeMetadata.monitors`:
- `index`: 0-based monitor index
- `x`: X position within composite image (non-negative)
- `y`: Y position within composite image (non-negative)
- `width`: Monitor width in pixels
- `height`: Monitor height in pixels
- `is_primary`: Boolean indicating primary monitor

### Step 4: Decode and Verify Image

Decode the base64 image and verify:
- Image is valid PNG
- Dimensions match `compositeMetadata.image_width` × `compositeMetadata.image_height`
- All monitor content is visible in correct positions

## Expected Result

A composite screenshot spanning all connected monitors is captured. The response includes both the image data and metadata describing each monitor's region within the composite, enabling the LLM to locate content on specific monitors.

## Pass Criteria

- [ ] `screenshot_control` capture action returns success
- [ ] `imageData` contains valid base64-encoded PNG
- [ ] `width` equals virtual screen width
- [ ] `height` equals virtual screen height
- [ ] `compositeMetadata` is present and valid
- [ ] `compositeMetadata.monitors` count matches number of physical monitors
- [ ] All monitor regions have non-negative image coordinates
- [ ] Exactly one monitor is marked as `is_primary: true`
- [ ] Image content shows all monitors stitched together correctly

## Failure Indicators

- No image data returned
- Missing `compositeMetadata` in response
- Monitor count mismatch
- Monitor regions overlap incorrectly
- Gap between monitor regions in composite
- Image dimensions don't match virtual screen bounds
- Error response from tool

## Notes

- This is the foundation for multi-monitor visual testing
- The composite image stitches all monitors using virtual screen coordinates
- Virtual screen can have negative X/Y origins if monitors are left/above primary
- Monitor regions use coordinates relative to the composite image (all non-negative)
- Use `list_monitors` action first if you need to verify monitor configuration
- For single-monitor systems, this produces the same result as `primary_screen` but with metadata
