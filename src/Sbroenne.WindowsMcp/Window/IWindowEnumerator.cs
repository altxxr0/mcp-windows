namespace Sbroenne.WindowsMcp.Window;

/// <summary>
/// Interface for enumerating windows on the system.
/// </summary>
public interface IWindowEnumerator
{
    /// <summary>
    /// Enumerates all visible top-level windows on the system.
    /// </summary>
    /// <param name="filter">Optional filter string for title or process name matching.</param>
    /// <param name="useRegex">Whether to use regex matching for the filter.</param>
    /// <param name="includeAllDesktops">Whether to include windows on other virtual desktops.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Collection of window information records.</returns>
    Task<IReadOnlyList<Models.WindowInfo>> EnumerateWindowsAsync(
        string? filter = null,
        bool useRegex = false,
        bool includeAllDesktops = false,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets information about a specific window by handle.
    /// </summary>
    /// <param name="handle">The window handle.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Window information, or null if the window doesn't exist.</returns>
    Task<Models.WindowInfo?> GetWindowInfoAsync(
        nint handle,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds windows matching the specified criteria.
    /// </summary>
    /// <param name="title">Title to search for (substring or regex).</param>
    /// <param name="useRegex">Whether to use regex matching.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Collection of matching window information records.</returns>
    Task<IReadOnlyList<Models.WindowInfo>> FindWindowsAsync(
        string title,
        bool useRegex = false,
        CancellationToken cancellationToken = default);
}
