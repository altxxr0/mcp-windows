using Sbroenne.WindowsMcp.Models;

namespace Sbroenne.WindowsMcp.Input;

/// <summary>
/// Provides mouse input operations using the Windows SendInput API.
/// </summary>
public interface IMouseInputService
{
    /// <summary>
    /// Moves the cursor to the specified coordinates.
    /// </summary>
    /// <param name="x">The target x-coordinate.</param>
    /// <param name="y">The target y-coordinate.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the operation result.</returns>
    Task<MouseControlResult> MoveAsync(int x, int y, CancellationToken cancellationToken = default);

    /// <summary>
    /// Performs a left mouse button click.
    /// </summary>
    /// <param name="x">Optional target x-coordinate. If null, uses current position.</param>
    /// <param name="y">Optional target y-coordinate. If null, uses current position.</param>
    /// <param name="modifiers">Modifier keys to hold during the click.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the operation result.</returns>
    Task<MouseControlResult> ClickAsync(int? x, int? y, ModifierKey modifiers = ModifierKey.None, CancellationToken cancellationToken = default);

    /// <summary>
    /// Performs a double left mouse button click.
    /// </summary>
    /// <param name="x">Optional target x-coordinate. If null, uses current position.</param>
    /// <param name="y">Optional target y-coordinate. If null, uses current position.</param>
    /// <param name="modifiers">Modifier keys to hold during the click.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the operation result.</returns>
    Task<MouseControlResult> DoubleClickAsync(int? x, int? y, ModifierKey modifiers = ModifierKey.None, CancellationToken cancellationToken = default);

    /// <summary>
    /// Performs a right mouse button click.
    /// </summary>
    /// <param name="x">Optional target x-coordinate. If null, uses current position.</param>
    /// <param name="y">Optional target y-coordinate. If null, uses current position.</param>
    /// <param name="modifiers">Modifier keys to hold during the click.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the operation result.</returns>
    Task<MouseControlResult> RightClickAsync(int? x, int? y, ModifierKey modifiers = ModifierKey.None, CancellationToken cancellationToken = default);

    /// <summary>
    /// Performs a middle mouse button click.
    /// </summary>
    /// <param name="x">Optional target x-coordinate. If null, uses current position.</param>
    /// <param name="y">Optional target y-coordinate. If null, uses current position.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the operation result.</returns>
    Task<MouseControlResult> MiddleClickAsync(int? x, int? y, CancellationToken cancellationToken = default);

    /// <summary>
    /// Performs a drag operation from start to end coordinates.
    /// </summary>
    /// <param name="startX">The starting x-coordinate.</param>
    /// <param name="startY">The starting y-coordinate.</param>
    /// <param name="endX">The ending x-coordinate.</param>
    /// <param name="endY">The ending y-coordinate.</param>
    /// <param name="button">The mouse button to use for dragging.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the operation result.</returns>
    Task<MouseControlResult> DragAsync(int startX, int startY, int endX, int endY, MouseButton button = MouseButton.Left, CancellationToken cancellationToken = default);

    /// <summary>
    /// Performs a scroll operation.
    /// </summary>
    /// <param name="direction">The scroll direction.</param>
    /// <param name="amount">The number of scroll clicks.</param>
    /// <param name="x">Optional target x-coordinate. If null, uses current position.</param>
    /// <param name="y">Optional target y-coordinate. If null, uses current position.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the operation result.</returns>
    Task<MouseControlResult> ScrollAsync(ScrollDirection direction, int amount, int? x, int? y, CancellationToken cancellationToken = default);
}
