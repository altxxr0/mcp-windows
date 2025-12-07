namespace Sbroenne.WindowsMcp.Automation;

/// <summary>
/// Provides detection of secure desktop states (UAC, lock screen).
/// </summary>
public interface ISecureDesktopDetector
{
    /// <summary>
    /// Checks if a secure desktop (UAC prompt, lock screen, Ctrl+Alt+Del) is currently active.
    /// </summary>
    /// <returns>True if a secure desktop is active; otherwise, false.</returns>
    bool IsSecureDesktopActive();
}
