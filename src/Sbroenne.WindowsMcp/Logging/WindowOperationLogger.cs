using Microsoft.Extensions.Logging;

namespace Sbroenne.WindowsMcp.Logging;

/// <summary>
/// Provides structured logging for window management operations.
/// </summary>
public sealed partial class WindowOperationLogger
{
    private readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="WindowOperationLogger"/> class.
    /// </summary>
    /// <param name="logger">The underlying logger.</param>
    public WindowOperationLogger(ILogger<WindowOperationLogger> logger)
    {
        ArgumentNullException.ThrowIfNull(logger);
        _logger = logger;
    }

    /// <summary>
    /// Logs a window operation with the specified details.
    /// </summary>
    /// <param name="action">The action being performed.</param>
    /// <param name="success">Whether the operation was successful.</param>
    /// <param name="windowCount">Number of windows affected (for list/find operations).</param>
    /// <param name="handle">Window handle involved in the operation.</param>
    /// <param name="windowTitle">Title of the window being operated on.</param>
    /// <param name="filter">Filter string used (for find/wait operations).</param>
    /// <param name="errorMessage">Error message if the operation failed.</param>
    public void LogWindowOperation(
        string action,
        bool success,
        int? windowCount = null,
        nint? handle = null,
        string? windowTitle = null,
        string? filter = null,
        string? errorMessage = null)
    {
        LogWindowOperationCore(_logger, action, success, windowCount, handle, windowTitle, filter, errorMessage);
    }

    /// <summary>
    /// Logs an error during window operation.
    /// </summary>
    /// <param name="action">The action that failed.</param>
    /// <param name="exception">The exception that occurred.</param>
    /// <param name="handle">Window handle if available.</param>
    public void LogError(string action, Exception exception, nint? handle = null)
    {
        LogWindowErrorCore(_logger, action, handle, exception);
    }

    [LoggerMessage(
        EventId = 200,
        Level = LogLevel.Information,
        Message = "Window operation: {Action}, Success: {Success}, WindowCount: {WindowCount}, Handle: {Handle}, Title: {Title}, Filter: {Filter}, Error: {ErrorMessage}")]
    private static partial void LogWindowOperationCore(
        ILogger logger,
        string action,
        bool success,
        int? windowCount,
        nint? handle,
        string? title,
        string? filter,
        string? errorMessage);

    [LoggerMessage(
        EventId = 201,
        Level = LogLevel.Error,
        Message = "Window operation '{Action}' failed for handle {Handle}")]
    private static partial void LogWindowErrorCore(
        ILogger logger,
        string action,
        nint? handle,
        Exception exception);
}
