namespace Sbroenne.WindowsMcp.Window;

/// <summary>
/// Interface for activating windows and managing foreground state.
/// </summary>
public interface IWindowActivator
{
    /// <summary>
    /// Activates a window and brings it to the foreground.
    /// </summary>
    /// <param name="handle">The window handle to activate.</param>
    /// <param name="useFallbackStrategies">Whether to try fallback strategies if initial attempt fails.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if activation was successful, false otherwise.</returns>
    Task<bool> ActivateWindowAsync(
        nint handle,
        bool useFallbackStrategies = true,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the handle of the current foreground window.
    /// </summary>
    /// <returns>Handle to the foreground window, or IntPtr.Zero if none.</returns>
    nint GetForegroundWindow();

    /// <summary>
    /// Verifies that a window is currently the foreground window.
    /// </summary>
    /// <param name="handle">The window handle to verify.</param>
    /// <returns>True if the window is the foreground window.</returns>
    bool IsForegroundWindow(nint handle);
}
