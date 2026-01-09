namespace Sbroenne.WindowsMcp.Models;

/// <summary>
/// Defines the screenshot operation to perform.
/// </summary>
public enum ScreenshotAction
{
    /// <summary>
    /// Capture screen, monitor, window, or region (default).
    /// </summary>
    Capture = 0,

    /// <summary>
    /// List available monitors with metadata.
    /// </summary>
    ListMonitors = 1,

    /// <summary>
    /// Record a sequence of screenshots.
    /// </summary>
    Record = 2
}
