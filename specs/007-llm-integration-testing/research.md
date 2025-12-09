# Research: LLM-Based Integration Testing Framework

**Feature**: 007-llm-integration-testing | **Date**: 2025-12-08 | **Version**: 1.1

## Executive Summary

This research document captures findings for the LLM-based integration testing framework. The key insight is that **no external testing framework is needed**—GitHub Copilot already has direct access to MCP tools and can execute test scenarios through natural language prompts.

**Update v1.1**: Added research for all-monitors composite screenshot capture and visual diff computation.

---

## 1. Existing Testing Frameworks

### Research Task
Evaluate existing LLM/MCP testing frameworks for potential adoption.

### Findings

| Framework | Purpose | Verdict |
|-----------|---------|---------|
| [MCP Inspector](https://github.com/modelcontextprotocol/inspector) | Official MCP debugging/testing tool | **Not needed** - Tests individual tool calls, not LLM-driven flows |
| [langwatch/scenario](https://github.com/langwatch/scenario) | Agent-tests-agent pattern | **Overkill** - Designed to connect TO an LLM; we ARE the LLM |
| [golf-mcp/golf-testing](https://github.com/golf-mcp/golf-testing) | MCP-specific test harness | **Not needed** - Adds orchestration layer we don't need |
| [pixelmatch](https://github.com/mapbox/pixelmatch) | Visual diff for screenshots | **Defer** - LLM can visually analyze; add if human review needed |
| [contextcheck](https://github.com/contextcheck/contextcheck) | YAML test scenarios | **Not needed** - Natural language scenarios are more flexible |

### Decision
**Keep framework-agnostic.** Test scenarios are markdown files with natural language instructions. GitHub Copilot executes them directly via MCP tools.

### Rationale
1. **We ARE the LLM** - Those frameworks are designed to connect TO an LLM, but Copilot already IS the LLM with direct MCP access
2. **Adding orchestration defeats the purpose** - We want to test "real LLM-to-MCP interactions," not orchestrated simulations
3. **Zero dependencies** - Aligns with Constitution Principle XXII (Open Source Dependencies)

### Alternatives Rejected
- External test runners add complexity without value
- YAML/JSON test definitions are less flexible than natural language
- Automated assertion libraries can't match LLM visual analysis capability

---

## 2. Screenshot Verification Strategy

### Research Task
Determine best approach for visual verification of test results.

### Findings

**Current Capability**:
- `mcp_windows_mcp_s_screenshot_control` returns base64-encoded PNG data
- GitHub Copilot can view images inline via VS Code image preview
- LLM can describe image contents and identify visual elements

**Verification Approaches**:

| Approach | Implementation | Verdict |
|----------|----------------|---------|
| LLM Visual Analysis | LLM describes what it sees in screenshot | **Primary** - Already works, no code needed |
| Pixel Diff (pixelmatch) | Compare before/after at pixel level | **Defer** - Add later if false positives become issue |
| OCR Text Extraction | Extract text from screenshots | **Defer** - Add later if text verification needed |
| Reference Image Comparison | Compare against known-good screenshots | **Defer** - Brittle; resolution/theme dependent |

### Decision
**LLM Visual Analysis** as primary verification method. The LLM takes a screenshot, describes what it sees, and determines if the expected state is present.

### Rationale
1. **Already works** - No additional tooling required
2. **Flexible** - LLM can interpret partial matches, handle minor variations
3. **Self-documenting** - LLM explains what it sees, creating readable test results

---

## 3. Secondary Monitor Targeting

### Research Task
Determine how to consistently target secondary monitor for test execution.

### Findings

**Available MCP Tool Parameters**:
- `screenshot_control`: `target="monitor"` with `monitorIndex` parameter (0-based)
- `screenshot_control`: `action="list_monitors"` returns available monitors
- `mouse_control`: `x`, `y` coordinates can target any monitor (coordinates extend beyond primary)
- `window_management`: `move` action with `x`, `y` can position windows on any monitor

**Multi-Monitor Coordinate System**:
- Windows uses virtual screen coordinates
- Primary monitor typically starts at (0, 0)
- Secondary monitor coordinates depend on arrangement (e.g., left of primary = negative X)

### Decision
**Test scenarios MUST call `list_monitors` first**, then use secondary monitor coordinates for all operations.

### Rationale
1. Aligns with Constitution v2.3.0, Principle XIV (Secondary Monitor Preference)
2. Prevents test interference with VS Code on primary monitor
3. Provides consistent test environment

### Implementation Pattern
```text
1. Call screenshot_control with action="list_monitors"
2. Parse response to get secondary monitor bounds
3. Use those bounds for all window positioning and click coordinates
4. Take screenshots targeting that monitorIndex
```

---

## 4. Test Result Storage

### Research Task
Define storage format and location for test results.

### Findings

**Requirements from Spec**:
- Visual evidence for each test step (FR-007)
- Test execution history (FR-009)
- Batch reports (FR-012)

**Storage Options**:

| Option | Format | Verdict |
|--------|--------|---------|
| Filesystem (flat) | PNG + MD files | **Too simple** - No organization |
| Filesystem (dated) | `results/YYYY-MM-DD/TC-XXX/` | **Selected** - Simple, browsable, git-friendly |
| SQLite database | Structured data + blob storage | **Overkill** - Not needed for this scale |
| JSON files | Structured test results | **Defer** - Add if machine parsing needed |

### Decision
**Filesystem with dated directories**:
```text
specs/007-llm-integration-testing/results/
└── 2025-12-08/
    └── TC-MOUSE-001/
        ├── before.png
        ├── after.png
        └── result.md
```

### Rationale
1. Human-browsable without tooling
2. Git-friendly (can .gitignore if desired)
3. Easy to archive or clean up by date

---

## 5. Test Scenario Format

### Research Task
Define structured format for test scenario definitions.

### Findings

**Format Options**:

| Format | Example | Verdict |
|--------|---------|---------|
| YAML | Structured fields, parsing required | **Not needed** - Adds parsing complexity |
| JSON | Machine-readable, verbose | **Not needed** - Less readable for humans |
| Markdown (structured) | Human-readable, natural language | **Selected** - Aligns with LLM execution model |
| Plain text | Unstructured instructions | **Too loose** - Need some structure for consistency |

### Decision
**Markdown with structured sections**:
```markdown
# Test Case: TC-MOUSE-001

## Objective
Verify basic mouse movement to specific coordinates.

## Preconditions
- Secondary monitor available
- No windows in target area

## Steps
1. Call `list_monitors` to get secondary monitor bounds
2. Take "before" screenshot of secondary monitor
3. Move mouse to center of secondary monitor
4. Take "after" screenshot
5. Verify mouse cursor is visible at expected location

## Expected Result
- Mouse cursor visible at center of secondary monitor
- No errors returned from MCP tools

## Pass Criteria
- [ ] Screenshot shows cursor at target location
- [ ] No tool invocation errors
```

### Rationale
1. Readable by both humans and LLM
2. Structured enough for consistency
3. Flexible enough for complex scenarios
4. No parsing/tooling required

---

## 6. Clarifications Resolved

| Topic | Resolution | Source |
|-------|------------|--------|
| Multi-monitor handling | Explicitly target secondary monitor | User clarification, spec updated |
| External frameworks | Not needed; use Copilot directly | Research above |
| Visual verification | LLM describes screenshots | Research above |
| Result storage | Filesystem with dated directories | Research above |
| Scenario format | Structured markdown | Research above |

---

## Summary of Decisions

1. **No external testing framework** - GitHub Copilot executes scenarios directly
2. **LLM visual analysis** - Primary verification method (no pixelmatch/OCR)
3. **Secondary monitor targeting** - Call `list_monitors` first, use those bounds
4. **Filesystem storage** - `results/YYYY-MM-DD/TC-XXX/` structure
5. **Structured markdown scenarios** - Human-readable, LLM-executable

**All NEEDS CLARIFICATION items have been resolved. Proceed to Phase 1.**

---

## 7. All-Monitors Composite Screenshot Capture

### Research Task
Determine how to capture all connected monitors into a single composite image for before/after comparison.

### Findings

**Windows Virtual Screen Concept**:
- Windows treats all monitors as one "virtual screen" with coordinates that can be negative
- `SystemInformation.VirtualScreen` returns the bounding rectangle of all monitors
- Primary monitor is typically at (0,0) but secondary monitors can have negative X or Y

**Available APIs**:

| API | Purpose | Notes |
|-----|---------|-------|
| `Screen.AllScreens` | Enumerate monitors | Already used in `MonitorService.cs` |
| `SystemInformation.VirtualScreen` | Get bounding rect of all monitors | Gives total width/height |
| `Graphics.CopyFromScreen` | Capture screen region | Can capture across monitors |
| `Bitmap.Clone(Rectangle)` | Extract region from composite | For per-monitor metadata |

**Existing Code Analysis**:
- `MonitorService.cs` already enumerates monitors with positions
- `ScreenshotService.cs` already captures using `CopyFromScreen`
- Only need to add `all_monitors` target to capture the full virtual screen

### Decision
Add `all_monitors` as a new `CaptureTarget` enum value that captures the entire virtual screen.

### Implementation Approach
```csharp
// Get virtual screen bounds
var virtualScreen = SystemInformation.VirtualScreen;

// Capture entire virtual screen
using var bitmap = new Bitmap(virtualScreen.Width, virtualScreen.Height);
using var graphics = Graphics.FromImage(bitmap);
graphics.CopyFromScreen(
    virtualScreen.X, virtualScreen.Y,  // Can be negative!
    0, 0,
    new Size(virtualScreen.Width, virtualScreen.Height));

// Return with monitors metadata
return ScreenshotControlResult.CompositeSuccess(
    base64,
    virtualScreen.Width,
    virtualScreen.Height,
    _monitorService.GetMonitors()  // Include monitor regions
);
```

### Alternatives Rejected

| Alternative | Reason Rejected |
|-------------|-----------------|
| Capture each monitor separately | More files; harder to see complete state |
| Use DXGI Desktop Duplication | Overkill; GDI+ is sufficient for test automation |
| Stitch monitors manually | Unnecessary; `CopyFromScreen` handles virtual screen natively |

### API References
- [SystemInformation.VirtualScreen](https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.systeminformation.virtualscreen)
- [Graphics.CopyFromScreen](https://learn.microsoft.com/en-us/dotnet/api/system.drawing.graphics.copyfromscreen)

---

## 8. Visual Diff Computation

### Research Task
Determine algorithm for detecting changes between before/after screenshots.

### Findings

**Requirement from Spec**:
- Configurable threshold (default 1% of pixels changed)
- Must work across multi-monitor composites
- Ignore minor variations (anti-aliasing, clock, cursor blink)

**Approaches Evaluated**:

| Approach | Pros | Cons | Verdict |
|----------|------|------|---------|
| Pixel-by-pixel comparison | Simple, no dependencies | Slow for large images | **Selected** |
| SSIM (Structural Similarity) | Perceptually accurate | Requires library (ImageSharp) | Defer |
| Perceptual hash (pHash) | Fast, handles slight variations | Doesn't highlight changed regions | Defer |

### Decision
Use pixel-by-pixel comparison with configurable tolerance.

### Algorithm
```text
1. Load before.png and after.png as Bitmap
2. Verify dimensions match (error if not)
3. For each pixel (x, y):
   a. Get RGB values from both images
   b. If |R1-R2| > tolerance OR |G1-G2| > tolerance OR |B1-B2| > tolerance:
      - Increment diffCount
      - Mark pixel as changed (for diff image)
4. Calculate diffPercent = (diffCount / totalPixels) * 100
5. Return changed = (diffPercent > threshold)
```

### Configuration
```json
{
  "pixelTolerance": 10,      // Per-channel difference tolerance (0-255)
  "changeThreshold": 1.0,    // Percentage of pixels changed to flag as different
  "generateDiffImage": true  // Whether to create visual diff overlay
}
```

### Rationale
1. **No external dependencies** - Aligns with Constitution XXII
2. **Configurable** - Allows tuning for different test scenarios
3. **Visual output** - Can generate diff image highlighting changed pixels

---

## 9. Result Storage Format Updates

### Research Task
Define storage format for composite screenshots and monitor metadata.

### Updated Decision
Store results as composite PNG + JSON metadata file:

```text
specs/007-llm-integration-testing/results/
└── 2025-12-08T14-30-00/
    ├── summary.json                    # Run summary
    └── TC-MOUSE-001/
        ├── result.json                 # Test result
        ├── step-1-before.png           # Composite screenshot
        ├── step-1-before-meta.json     # Monitor regions
        ├── step-1-after.png            # Composite screenshot  
        ├── step-1-after-meta.json      # Monitor regions
        ├── step-1-diff.png             # Visual diff (if changes)
        └── step-1-response.json        # MCP tool response
```

### Monitor Metadata Schema
```json
{
  "captureTime": "2025-12-08T14:30:01.234Z",
  "virtualScreen": {
    "x": -1920,
    "y": 0,
    "width": 5760,
    "height": 1440
  },
  "monitors": [
    {
      "index": 0,
      "x": 0,
      "y": 0,
      "width": 1920,
      "height": 1080,
      "isPrimary": true
    },
    {
      "index": 1,
      "x": -1920,
      "y": 0,
      "width": 1920,
      "height": 1080,
      "isPrimary": false
    }
  ]
}
```

### Rationale
- Single composite image shows complete system state
- JSON metadata enables programmatic analysis of per-monitor regions
- Structure supports both human browsing and automated processing

---

## Summary of Decisions

1. **No external testing framework** - GitHub Copilot executes scenarios directly
2. **LLM visual analysis** - Primary verification method (no pixelmatch/OCR)
3. **Secondary monitor targeting** - Call `list_monitors` first, use those bounds
4. **Filesystem storage** - `results/YYYY-MM-DDTHH-MM-SS/TC-XXX/` structure
5. **Structured markdown scenarios** - Human-readable, LLM-executable
6. **All-monitors composite capture** - Single image using `CopyFromScreen` with virtual screen bounds
7. **Pixel-based visual diff** - Configurable threshold (default 1%), no external dependencies
8. **PNG + JSON metadata** - Composite image with accompanying monitor region metadata

**All NEEDS CLARIFICATION items have been resolved. Proceed to Phase 1.**
