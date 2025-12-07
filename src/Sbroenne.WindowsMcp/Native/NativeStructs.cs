using System.Runtime.InteropServices;

namespace Sbroenne.WindowsMcp.Native;

/// <summary>
/// Contains native structure definitions for Windows API interop.
/// </summary>
internal static class NativeStructs
{
    // Marker class - all structures defined below
}

/// <summary>
/// Represents a point in screen coordinates.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal struct POINT
{
    /// <summary>The x-coordinate of the point.</summary>
    public int X;

    /// <summary>The y-coordinate of the point.</summary>
    public int Y;

    /// <summary>
    /// Initializes a new instance of the <see cref="POINT"/> struct.
    /// </summary>
    /// <param name="x">The x-coordinate.</param>
    /// <param name="y">The y-coordinate.</param>
    public POINT(int x, int y)
    {
        X = x;
        Y = y;
    }
}

/// <summary>
/// Used by SendInput to store information for synthesizing input events.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal struct INPUT
{
    /// <summary>The type of the input event (mouse, keyboard, or hardware).</summary>
    public uint Type;

    /// <summary>The input union containing mouse, keyboard, or hardware input data.</summary>
    public INPUTUNION Data;

    /// <summary>Input type constant for mouse input.</summary>
    public const uint INPUT_MOUSE = 0;

    /// <summary>Input type constant for keyboard input.</summary>
    public const uint INPUT_KEYBOARD = 1;

    /// <summary>Gets the size of the INPUT structure for use with SendInput.</summary>
    public static int Size => Marshal.SizeOf<INPUT>();
}

/// <summary>
/// Union for INPUT structure to hold different input types.
/// </summary>
[StructLayout(LayoutKind.Explicit)]
internal struct INPUTUNION
{
    /// <summary>Mouse input data.</summary>
    [FieldOffset(0)]
    public MOUSEINPUT Mouse;

    /// <summary>Keyboard input data.</summary>
    [FieldOffset(0)]
    public KEYBDINPUT Keyboard;
}

/// <summary>
/// Contains information about a simulated mouse event.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal struct MOUSEINPUT
{
    /// <summary>Absolute position or relative motion in x-direction.</summary>
    public int Dx;

    /// <summary>Absolute position or relative motion in y-direction.</summary>
    public int Dy;

    /// <summary>Wheel movement amount for scroll operations.</summary>
    public int MouseData;

    /// <summary>Mouse event flags specifying the type of mouse event.</summary>
    public uint DwFlags;

    /// <summary>Time stamp for the event (0 = system provides).</summary>
    public uint Time;

    /// <summary>Additional value associated with the event.</summary>
    public nuint DwExtraInfo;
}

/// <summary>
/// Contains information about a simulated keyboard event.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal struct KEYBDINPUT
{
    /// <summary>Virtual-key code.</summary>
    public ushort WVk;

    /// <summary>Hardware scan code for the key.</summary>
    public ushort WScan;

    /// <summary>Keyboard event flags.</summary>
    public uint DwFlags;

    /// <summary>Time stamp for the event (0 = system provides).</summary>
    public uint Time;

    /// <summary>Additional value associated with the event.</summary>
    public nuint DwExtraInfo;
}

/// <summary>
/// Contains information about the elevation status of a process's token.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal struct TOKEN_ELEVATION
{
    /// <summary>Non-zero if the token is elevated.</summary>
    public int TokenIsElevated;
}
