namespace Sbroenne.WindowsMcp.Automation;

/// <summary>
/// Provides detection of elevated (admin) processes for UIPI compliance.
/// </summary>
public interface IElevationDetector
{
    /// <summary>
    /// Checks if the window at the specified coordinates belongs to an elevated process.
    /// </summary>
    /// <param name="x">The x-coordinate to check.</param>
    /// <param name="y">The y-coordinate to check.</param>
    /// <returns>True if the target window is elevated; otherwise, false.</returns>
    bool IsTargetElevated(int x, int y);
}
