using Sbroenne.WindowsMcp.Native;

namespace Sbroenne.WindowsMcp.Automation;

/// <summary>
/// Detects whether a secure desktop (UAC, lock screen) is currently active.
/// </summary>
public class SecureDesktopDetector : ISecureDesktopDetector
{
    /// <inheritdoc/>
    public bool IsSecureDesktopActive()
    {
        // Attempt to open the input desktop
        // If we're on a secure desktop (UAC, lock screen, Ctrl+Alt+Del),
        // this will return NULL because we can't access the secure desktop
        var hDesktop = NativeMethods.OpenInputDesktop(
            0,
            false,
            NativeConstants.DESKTOP_SWITCHDESKTOP);

        if (hDesktop == IntPtr.Zero)
        {
            // Secure desktop is active - we cannot access the input desktop
            return true;
        }

        // We got a handle - close it and return false (no secure desktop)
        _ = NativeMethods.CloseDesktop(hDesktop);
        return false;
    }
}
