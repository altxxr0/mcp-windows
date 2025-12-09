# Test Result: {TC-ID}

<!--
  This is the template for recording LLM-based integration test results.
  Copy this file to results/{YYYY-MM-DD}/{TC-ID}/result.md after running a test.
  
  Matches TestRun entity from data-model.md:
  - runId: Implicit from directory path
  - scenarioId: TC-ID
  - startTime: Start Time field
  - endTime: End Time field
  - status: PASS/FAIL/BLOCKED/SKIPPED
  - screenshots: Screenshot section
  - observations: Observations section
  - passCriteriaResults: Pass Criteria Results table
-->

**Run ID**: {YYYY-MM-DDTHH:MM:SS}  
**Scenario**: [{TC-ID}](../../scenarios/{TC-ID}.md)  
**Status**: {PASS / FAIL / BLOCKED / SKIPPED}

---

## Summary

| Metric | Value |
|--------|-------|
| **Start Time** | {YYYY-MM-DD HH:MM:SS} |
| **End Time** | {YYYY-MM-DD HH:MM:SS} |
| **Duration** | {X.X seconds} |
| **Monitor Used** | {Primary / Secondary} |
| **Monitor Index** | {0-based index} |
| **Target App** | {Notepad / Calculator / None} |

## Pass Criteria Results

<!--
  List all pass criteria from the scenario.
  Mark each as PASS or FAIL with observations.
  
  Status cannot be PASS if any criterion is FAIL (per data-model.md validation rules)
-->

| Criterion | Result | Notes |
|-----------|--------|-------|
| {Criterion 1 from scenario} | ✅ PASS / ❌ FAIL | {Observation} |
| {Criterion 2 from scenario} | ✅ PASS / ❌ FAIL | {Observation} |
| {Criterion 3 from scenario} | ✅ PASS / ❌ FAIL | {Observation} |

## Screenshots

<!--
  Screenshots match Screenshot entity from data-model.md:
  - filename: File name in this directory
  - captureTime: When captured
  - monitorIndex: Which monitor (or all for composite)
  - stepNumber: Which step (optional)
  - description: LLM description
  
  At least 1 screenshot is REQUIRED per data-model.md
  
  COMPOSITE SCREENSHOTS: Use target="all_monitors" to capture all connected displays
  in a single composite image. Save the metadata JSON alongside the screenshot for
  region mapping and visual diff analysis.
-->

### Before (All Monitors Composite)

**Filename**: `step-1-before.png`  
**Metadata**: `step-1-before-meta.json`  
**Capture Time**: {HH:MM:SS}  
**Target**: All Monitors (Composite)  
**Step Number**: 2

![Before](step-1-before.png)

<details>
<summary>Composite Metadata</summary>

```json
{
  "VirtualScreenBounds": {
    "X": {left}, "Y": {top}, "Width": {width}, "Height": {height}
  },
  "MonitorRegions": [
    {
      "Index": 0,
      "IsPrimary": true,
      "DeviceName": "\\\\.\\DISPLAY1",
      "Bounds": { "X": {X}, "Y": {Y}, "Width": {W}, "Height": {H} },
      "ImageX": {offsetX},
      "ImageY": {offsetY}
    },
    {
      "Index": 1,
      "IsPrimary": false,
      "DeviceName": "\\\\.\\DISPLAY2",
      "Bounds": { "X": {X}, "Y": {Y}, "Width": {W}, "Height": {H} },
      "ImageX": {offsetX},
      "ImageY": {offsetY}
    }
  ]
}
```
</details>

**LLM Observation**: {Description of what the LLM sees in this screenshot across all monitors}

### After (All Monitors Composite)

**Filename**: `step-1-after.png`  
**Metadata**: `step-1-after-meta.json`  
**Capture Time**: {HH:MM:SS}  
**Target**: All Monitors (Composite)  
**Step Number**: 4

![After](step-1-after.png)

**LLM Observation**: {Description of what the LLM sees in this screenshot}

### Intermediate Screenshots (if applicable)

<!--
  For multi-step tests, capture at key workflow steps:
  - step-2-before.png / step-2-before-meta.json
  - step-2-after.png / step-2-after-meta.json
  - etc.
-->

{Add any step-N-*.png screenshots captured during the test}

## Visual Diff Analysis

<!--
  Visual diff is computed between before and after composite screenshots.
  Uses pixel-by-pixel comparison with configurable threshold.
  Diff results help identify what changed and where.
-->

### Diff Summary

| Metric | Value |
|--------|-------|
| **Before Image** | step-1-before.png |
| **After Image** | step-1-after.png |
| **Total Pixels** | {N} |
| **Changed Pixels** | {N} |
| **Change Percentage** | {X.XX%} |
| **Threshold** | {1.00%} |
| **Is Significant** | ✅ Yes / ❌ No |
| **Dimensions Match** | ✅ Yes / ❌ No |

### Diff Image

**Filename**: `step-1-diff.png`  

![Diff](step-1-diff.png)

**Changed Regions**: {Description of highlighted areas in diff image - red overlay indicates changed pixels}

### Monitor-Specific Changes

<!--
  Use metadata region bounds to identify which monitors had changes
-->

| Monitor | Index | Change Detected | Description |
|---------|-------|-----------------|-------------|
| Primary | 0 | ✅ Yes / ❌ No | {Description of changes on primary monitor} |
| Secondary | 1 | ✅ Yes / ❌ No | {Description of changes on secondary monitor} |

### Visual Changes Summary

{LLM description of differences between before and after screenshots, focusing on:
- Window position/size changes
- Content changes (text, images, controls)
- Cursor position changes
- Cross-monitor movements}

## Tool Invocations

### Step 1: {Action Description}

**Tool**: `{mcp_tool_name}`  
**Parameters**:
```json
{
  "action": "{action}",
  "param1": "value1"
}
```

**Response**:
```json
{
  "success": true,
  "result": "..."
}
```

**Elapsed**: {X.X}s

### Step 2: {Action Description}

{Repeat for each tool invocation}

## Observations

<!--
  LLM's detailed narrative of the test execution.
  This is the main "observations" field from TestRun entity.
-->

{LLM's detailed observations about test execution}

### What Happened

{Narrative description of the test execution}

### Anomalies

{Any unexpected behavior observed, even if test passed}

## Failure Details (if applicable)

<!--
  Required if Status is FAIL.
  Provides debugging context for investigation.
-->

**Failure Reason**: {Brief description of why test failed}

**Expected**: {What should have happened}

**Actual**: {What actually happened}

**Diagnostic Info**:
- {Relevant error messages}
- {State at time of failure}
- {Suggested root cause}

## Environment

| Property | Value |
|----------|-------|
| **OS** | Windows 11 |
| **OS Build** | {Build number} |
| **Screen Resolution** | {WxH} |
| **DPI Scaling** | {100% / 125% / 150% / etc.} |
| **Monitor Count** | {N} |
| **Target Monitor Index** | {N} |
| **MCP Server Version** | {Version} |
