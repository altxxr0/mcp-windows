using Sbroenne.WindowsMcp.Models;

namespace Sbroenne.WindowsMcp.Input;

/// <summary>
/// Manages modifier key (Ctrl, Shift, Alt) state during mouse operations.
/// </summary>
public interface IModifierKeyManager
{
    /// <summary>
    /// Presses the specified modifier keys.
    /// Only presses keys that are not already held by the user.
    /// </summary>
    /// <param name="modifiers">The modifier keys to press.</param>
    /// <returns>A list of virtual key codes that were actually pressed.</returns>
    IReadOnlyList<int> PressModifiers(ModifierKey modifiers);

    /// <summary>
    /// Releases the specified modifier keys.
    /// </summary>
    /// <param name="pressedKeys">The virtual key codes to release.</param>
    void ReleaseModifiers(IReadOnlyList<int> pressedKeys);

    /// <summary>
    /// Checks if the specified modifier key is currently pressed.
    /// </summary>
    /// <param name="virtualKeyCode">The virtual key code to check.</param>
    /// <returns>True if the key is pressed; otherwise, false.</returns>
    bool IsKeyPressed(int virtualKeyCode);
}
