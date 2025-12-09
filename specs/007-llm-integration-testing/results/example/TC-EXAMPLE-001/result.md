# Test Result: TC-EXAMPLE-001

<!--
  This is a sample filled-in result for reference.
  Use this as a guide when filling out result-template.md.
-->

**Run ID**: 2025-12-08T14:30:45  
**Scenario**: [TC-EXAMPLE-001](../../scenarios/TC-MOUSE-001.md)  
**Status**: PASS

---

## Summary

| Metric | Value |
|--------|-------|
| **Start Time** | 2025-12-08 14:30:45 |
| **End Time** | 2025-12-08 14:30:52 |
| **Duration** | 7.2 seconds |
| **Monitor Used** | Secondary |
| **Monitor Index** | 0 |
| **Target App** | None |

## Pass Criteria Results

| Criterion | Result | Notes |
|-----------|--------|-------|
| list_monitors returns at least 2 monitors | ✅ PASS | Returned 2 monitors |
| move action completes without error | ✅ PASS | Move returned success |
| "After" screenshot shows cursor at target | ✅ PASS | Cursor visible at center |
| Cursor position in response matches target | ✅ PASS | Position: (3392, 1128) |

## Screenshots

### Before
**Filename**: `before.png`  
**Capture Time**: 14:30:46  
**Monitor Index**: 0  
**Step Number**: 2

![Before](before.png)

**LLM Observation**: The secondary monitor shows the Windows desktop with several icons visible. The mouse cursor is located in the upper-left area of the screen, near the desktop icons. No applications are open.

### After
**Filename**: `after.png`  
**Capture Time**: 14:30:51  
**Monitor Index**: 0  
**Step Number**: 4

![After](after.png)

**LLM Observation**: The secondary monitor shows the same Windows desktop. The mouse cursor is now positioned in the center of the screen, clearly visible. The desktop icons remain in the same position.

### Visual Changes Detected

The primary visual difference between the before and after screenshots is the position of the mouse cursor:
- **Before**: Cursor in upper-left quadrant (approximately x=2680, y=650)
- **After**: Cursor in center of monitor (approximately x=3392, y=1128)

The cursor has clearly moved from its original position to the calculated center of the secondary monitor. No other changes are visible on the desktop.

## Tool Invocations

### Step 1: Detect Secondary Monitor

**Tool**: `mcp_windows_mcp_s_screenshot_control`  
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
    {
      "index": 0,
      "x": 2560,
      "y": 574,
      "width": 1664,
      "height": 1109,
      "is_primary": false
    },
    {
      "index": 1,
      "x": 0,
      "y": 0,
      "width": 2560,
      "height": 1600,
      "is_primary": true
    }
  ]
}
```

**Elapsed**: 0.3s

### Step 2: Before Screenshot

**Tool**: `mcp_windows_mcp_s_screenshot_control`  
**Parameters**:
```json
{
  "target": "monitor",
  "monitorIndex": 0,
  "includeCursor": true
}
```

**Response**:
```json
{
  "success": true,
  "image": "[base64 data]"
}
```

**Elapsed**: 0.8s

### Step 3: Move Mouse to Center

**Tool**: `mcp_windows_mcp_s_mouse_control`  
**Parameters**:
```json
{
  "action": "move",
  "x": 3392,
  "y": 1128
}
```

**Response**:
```json
{
  "success": true,
  "x": 3392,
  "y": 1128
}
```

**Elapsed**: 0.1s

### Step 4: After Screenshot

**Tool**: `mcp_windows_mcp_s_screenshot_control`  
**Parameters**:
```json
{
  "target": "monitor",
  "monitorIndex": 0,
  "includeCursor": true
}
```

**Response**:
```json
{
  "success": true,
  "image": "[base64 data]"
}
```

**Elapsed**: 0.7s

## Observations

The test executed successfully without any issues. The mouse cursor moved smoothly from its original position to the calculated center of the secondary monitor.

### What Happened

1. Called list_monitors to identify available displays
2. Identified secondary monitor at index 0 with bounds (2560, 574, 1664, 1109)
3. Calculated center: x = 2560 + (1664/2) = 3392, y = 574 + (1109/2) = 1128
4. Captured before screenshot showing cursor in upper-left area
5. Executed move command to center coordinates
6. Captured after screenshot confirming cursor at center
7. Visually verified cursor position matches target

### Anomalies

None observed. All operations completed successfully.

## Environment

| Property | Value |
|----------|-------|
| **OS** | Windows 11 |
| **OS Build** | 22631.4460 |
| **Screen Resolution** | 1664x1109 (secondary) |
| **DPI Scaling** | 100% |
| **Monitor Count** | 2 |
| **Target Monitor Index** | 0 |
| **MCP Server Version** | 1.0.0 |
