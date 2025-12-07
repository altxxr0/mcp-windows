namespace Sbroenne.WindowsMcp.Native;

/// <summary>
/// Contains native constants for Windows API interop.
/// </summary>
internal static class NativeConstants
{
    #region Input Types

    /// <summary>The input is mouse event data.</summary>
    public const uint INPUT_MOUSE = 0;

    /// <summary>The input is keyboard event data.</summary>
    public const uint INPUT_KEYBOARD = 1;

    /// <summary>The input is hardware event data.</summary>
    public const uint INPUT_HARDWARE = 2;

    #endregion

    #region Mouse Event Flags (MOUSEEVENTF_*)

    /// <summary>Movement occurred.</summary>
    public const uint MOUSEEVENTF_MOVE = 0x0001;

    /// <summary>The left button was pressed.</summary>
    public const uint MOUSEEVENTF_LEFTDOWN = 0x0002;

    /// <summary>The left button was released.</summary>
    public const uint MOUSEEVENTF_LEFTUP = 0x0004;

    /// <summary>The right button was pressed.</summary>
    public const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;

    /// <summary>The right button was released.</summary>
    public const uint MOUSEEVENTF_RIGHTUP = 0x0010;

    /// <summary>The middle button was pressed.</summary>
    public const uint MOUSEEVENTF_MIDDLEDOWN = 0x0020;

    /// <summary>The middle button was released.</summary>
    public const uint MOUSEEVENTF_MIDDLEUP = 0x0040;

    /// <summary>An X button was pressed.</summary>
    public const uint MOUSEEVENTF_XDOWN = 0x0080;

    /// <summary>An X button was released.</summary>
    public const uint MOUSEEVENTF_XUP = 0x0100;

    /// <summary>Wheel rotation (vertical scrolling).</summary>
    public const uint MOUSEEVENTF_WHEEL = 0x0800;

    /// <summary>Wheel rotation (horizontal scrolling).</summary>
    public const uint MOUSEEVENTF_HWHEEL = 0x1000;

    /// <summary>Coordinates are mapped to entire virtual desktop.</summary>
    public const uint MOUSEEVENTF_VIRTUALDESK = 0x4000;

    /// <summary>The dx and dy values contain absolute coordinates.</summary>
    public const uint MOUSEEVENTF_ABSOLUTE = 0x8000;

    #endregion

    #region Keyboard Event Flags (KEYEVENTF_*)

    /// <summary>If specified, key is being released; otherwise pressed.</summary>
    public const uint KEYEVENTF_KEYUP = 0x0002;

    #endregion

    #region Virtual Key Codes (VK_*)

    /// <summary>Control key.</summary>
    public const int VK_CONTROL = 0x11;

    /// <summary>Shift key.</summary>
    public const int VK_SHIFT = 0x10;

    /// <summary>Alt key (Menu).</summary>
    public const int VK_MENU = 0x12;

    /// <summary>Left Control key.</summary>
    public const int VK_LCONTROL = 0xA2;

    /// <summary>Right Control key.</summary>
    public const int VK_RCONTROL = 0xA3;

    /// <summary>Left Shift key.</summary>
    public const int VK_LSHIFT = 0xA0;

    /// <summary>Right Shift key.</summary>
    public const int VK_RSHIFT = 0xA1;

    /// <summary>Left Alt key.</summary>
    public const int VK_LMENU = 0xA4;

    /// <summary>Right Alt key.</summary>
    public const int VK_RMENU = 0xA5;

    #endregion

    #region System Metrics (SM_*)

    /// <summary>Left side of the virtual screen.</summary>
    public const int SM_XVIRTUALSCREEN = 76;

    /// <summary>Top of the virtual screen.</summary>
    public const int SM_YVIRTUALSCREEN = 77;

    /// <summary>Width of the virtual screen.</summary>
    public const int SM_CXVIRTUALSCREEN = 78;

    /// <summary>Height of the virtual screen.</summary>
    public const int SM_CYVIRTUALSCREEN = 79;

    /// <summary>Number of display monitors.</summary>
    public const int SM_CMONITORS = 80;

    #endregion

    #region Process Access Rights

    /// <summary>Required to retrieve certain information about a process.</summary>
    public const uint PROCESS_QUERY_LIMITED_INFORMATION = 0x1000;

    #endregion

    #region Token Access Rights

    /// <summary>Required to query an access token.</summary>
    public const uint TOKEN_QUERY = 0x0008;

    #endregion

    #region Token Information Classes

    /// <summary>Token elevation information class.</summary>
    public const int TokenElevation = 20;

    #endregion

    #region Desktop Access Rights

    /// <summary>Required to use the SwitchDesktop function on a desktop.</summary>
    public const uint DESKTOP_SWITCHDESKTOP = 0x0100;

    #endregion

    #region Mouse Wheel

    /// <summary>Standard wheel delta value (120 units = one notch).</summary>
    public const int WHEEL_DELTA = 120;

    #endregion
}
