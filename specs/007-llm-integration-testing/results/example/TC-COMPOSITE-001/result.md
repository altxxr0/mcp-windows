# Test Result: TC-COMPOSITE-001

<!--
  This is a sample composite screenshot test result demonstrating:
  - All-monitors composite capture
  - Metadata JSON with monitor regions
  - Visual diff analysis
-->

**Run ID**: 2025-01-15T14:30:00  
**Scenario**: [TC-VISUAL-001a](../../scenarios/TC-VISUAL-001a.md)  
**Status**: ✅ PASS

---

## Summary

| Metric | Value |
|--------|-------|
| **Start Time** | 2025-01-15 14:30:00 |
| **End Time** | 2025-01-15 14:30:06 |
| **Duration** | 6.2 seconds |
| **Monitors Used** | Primary + Secondary (All) |
| **Target App** | Notepad |

## Pass Criteria Results

| Criterion | Result | Notes |
|-----------|--------|-------|
| All-monitors composite screenshot captured successfully | ✅ PASS | Both monitors captured in single 3840x1080 image |
| Metadata JSON contains both monitor regions | ✅ PASS | MonitorRegions array has 2 entries |
| Window visible on primary monitor before move | ✅ PASS | Notepad window at position (100, 100) on primary |
| Window visible on secondary monitor after move | ✅ PASS | Notepad window at position (-1720, 100) on secondary |
| Visual diff detects significant change | ✅ PASS | 3.0% change (threshold: 1.0%) |
| Visual diff highlights both old and new window positions | ✅ PASS | Red overlay visible at both locations |

## Screenshots

### Before (All Monitors Composite)

**Filename**: `step-1-before.png`  
**Metadata**: `step-1-before-meta.json`  
**Capture Time**: 14:30:01  
**Target**: All Monitors (Composite)  
**Dimensions**: 3840 x 1080

<details>
<summary>Composite Metadata</summary>

```json
{
  "VirtualScreenBounds": {
    "X": -1920, "Y": 0, "Width": 3840, "Height": 1080
  },
  "MonitorRegions": [
    {
      "Index": 0,
      "IsPrimary": true,
      "DeviceName": "\\\\.\\DISPLAY1",
      "Bounds": { "X": 0, "Y": 0, "Width": 1920, "Height": 1080 },
      "ImageX": 1920,
      "ImageY": 0
    },
    {
      "Index": 1,
      "IsPrimary": false,
      "DeviceName": "\\\\.\\DISPLAY2",
      "Bounds": { "X": -1920, "Y": 0, "Width": 1920, "Height": 1080 },
      "ImageX": 0,
      "ImageY": 0
    }
  ]
}
```
</details>

**LLM Observation**: The composite screenshot shows both monitors side by side. The primary monitor (right side, pixels 1920-3840) displays a Notepad window in the upper-left corner. The secondary monitor (left side, pixels 0-1919) shows only the desktop wallpaper with no application windows.

### After (All Monitors Composite)

**Filename**: `step-1-after.png`  
**Metadata**: `step-1-after-meta.json`  
**Capture Time**: 14:30:05  
**Target**: All Monitors (Composite)  
**Dimensions**: 3840 x 1080

**LLM Observation**: The composite screenshot now shows the Notepad window has moved from the primary monitor (right side) to the secondary monitor (left side). The primary monitor region now shows only the desktop. The window maintained its size during the move.

## Visual Diff Analysis

### Diff Summary

| Metric | Value |
|--------|-------|
| **Before Image** | step-1-before.png |
| **After Image** | step-1-after.png |
| **Total Pixels** | 4,147,200 |
| **Changed Pixels** | 124,416 |
| **Change Percentage** | 3.00% |
| **Threshold** | 1.00% |
| **Is Significant** | ✅ Yes |
| **Dimensions Match** | ✅ Yes |

### Diff Image

**Filename**: `step-1-diff.png`

**Changed Regions**: The diff image shows two distinct highlighted areas:
1. Primary monitor region (right side): Semi-transparent red overlay where the Notepad window was previously located
2. Secondary monitor region (left side): Semi-transparent red overlay where the Notepad window now appears

### Monitor-Specific Changes

| Monitor | Index | Change Detected | Description |
|---------|-------|-----------------|-------------|
| Primary | 0 | ✅ Yes | Notepad window disappeared from position (100, 100) |
| Secondary | 1 | ✅ Yes | Notepad window appeared at position (200, 100) relative to monitor |

### Visual Changes Summary

The visual diff confirms successful cross-monitor window movement:
- **Window removal from primary**: The area previously occupied by Notepad (approximately 640x480 pixels at position 100,100 on primary) is now showing desktop background
- **Window appearance on secondary**: Notepad window now visible at position 200,100 on secondary monitor
- **Window integrity**: Window size and content appear unchanged after the move
- **No unexpected changes**: Only the expected window position changes are highlighted

## Tool Invocations

### Step 1: List Monitors

**Tool**: `screenshot_control`  
**Parameters**:
```json
{
  "action": "list_monitors"
}
```

**Response**:
```json
{
  "monitors": [
    { "index": 0, "isPrimary": true, "deviceName": "\\\\.\\DISPLAY1", "width": 1920, "height": 1080, "x": 0, "y": 0 },
    { "index": 1, "isPrimary": false, "deviceName": "\\\\.\\DISPLAY2", "width": 1920, "height": 1080, "x": -1920, "y": 0 }
  ]
}
```

**Elapsed**: 0.1s

### Step 2: Before Screenshot (All Monitors)

**Tool**: `screenshot_control`  
**Parameters**:
```json
{
  "action": "capture",
  "target": "all_monitors",
  "includeCursor": true
}
```

**Response**:
```json
{
  "success": true,
  "base64Data": "iVBORw0KGgoAAAANSUhEUgAA...",
  "compositeMetadata": {
    "VirtualScreenBounds": { "X": -1920, "Y": 0, "Width": 3840, "Height": 1080 },
    "MonitorRegions": [ ... ]
  }
}
```

**Elapsed**: 0.8s

### Step 3: Move Window to Secondary Monitor

**Tool**: `window_management`  
**Parameters**:
```json
{
  "action": "move",
  "handle": "12345678",
  "x": -1720,
  "y": 100
}
```

**Response**:
```json
{
  "success": true,
  "message": "Window moved successfully"
}
```

**Elapsed**: 0.3s

### Step 4: After Screenshot (All Monitors)

**Tool**: `screenshot_control`  
**Parameters**:
```json
{
  "action": "capture",
  "target": "all_monitors",
  "includeCursor": true
}
```

**Response**:
```json
{
  "success": true,
  "base64Data": "iVBORw0KGgoAAAANSUhEUgAA...",
  "compositeMetadata": {
    "VirtualScreenBounds": { "X": -1920, "Y": 0, "Width": 3840, "Height": 1080 },
    "MonitorRegions": [ ... ]
  }
}
```

**Elapsed**: 0.7s

## Observations

### What Happened

The test successfully verified cross-monitor window movement using all-monitors composite screenshots:

1. **Initial state captured**: The before screenshot showed Notepad positioned on the primary monitor
2. **Window moved**: The window_management tool moved Notepad from primary (x=100) to secondary (x=-1720) monitor
3. **Final state captured**: The after screenshot confirmed the window appeared on the secondary monitor
4. **Visual diff computed**: 3% of pixels changed, well above the 1% significance threshold
5. **Changes localized**: The diff correctly highlighted both the old and new window positions

### Anomalies

No anomalies detected. The test executed as expected.

## Environment

| Property | Value |
|----------|-------|
| **OS** | Windows 11 |
| **OS Build** | 22631.2861 |
| **Virtual Screen** | 3840 x 1080 |
| **Monitor Count** | 2 |
| **Primary Monitor** | 1920 x 1080 @ (0, 0) |
| **Secondary Monitor** | 1920 x 1080 @ (-1920, 0) |
| **DPI Scaling** | 100% (both monitors) |
| **MCP Server Version** | 1.0.0 |
