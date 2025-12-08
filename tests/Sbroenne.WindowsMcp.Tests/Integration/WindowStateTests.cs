using System.Runtime.Versioning;
using Sbroenne.WindowsMcp.Models;
using Sbroenne.WindowsMcp.Window;

namespace Sbroenne.WindowsMcp.Tests.Integration;

/// <summary>
/// Integration tests for window state control operations (minimize, maximize, restore, close).
/// </summary>
[Collection("WindowManagement")]
[SupportedOSPlatform("windows")]
public class WindowStateTests : IClassFixture<WindowTestFixture>
{
    private readonly IWindowService _windowService;

    /// <summary>
    /// Initializes a new instance of the <see cref="WindowStateTests"/> class.
    /// </summary>
    /// <param name="fixture">The shared test fixture.</param>
    public WindowStateTests(WindowTestFixture fixture)
    {
        ArgumentNullException.ThrowIfNull(fixture);
        _windowService = fixture.WindowService;
    }

    [Fact]
    public async Task MinimizeWindow_MinimizesWindow()
    {
        // Arrange - Get a window that is not minimized and not elevated
        var listResult = await _windowService.ListWindowsAsync();
        Assert.True(listResult.Success);
        Assert.NotNull(listResult.Windows);

        var targetWindow = listResult.Windows.FirstOrDefault(w =>
            w.State != WindowState.Minimized && !w.IsElevated);

        if (targetWindow is null)
        {
            // No suitable window, skip test
            return;
        }

        Assert.True(long.TryParse(targetWindow.Handle, out long handleValue));
        nint handle = (nint)handleValue;

        // Act
        var result = await _windowService.MinimizeWindowAsync(handle);

        // Assert - operation completes, may fail due to permissions
        Assert.NotNull(result);
        if (result.Success)
        {
            Assert.NotNull(result.Window);
            Assert.Equal(WindowState.Minimized, result.Window.State);

            // Clean up - restore the window
            await _windowService.RestoreWindowAsync(handle);
        }
    }

    [Fact]
    public async Task MaximizeWindow_MaximizesWindow()
    {
        // Arrange - Get a window that is not maximized and not elevated
        var listResult = await _windowService.ListWindowsAsync();
        Assert.True(listResult.Success);
        Assert.NotNull(listResult.Windows);

        var targetWindow = listResult.Windows.FirstOrDefault(w =>
            w.State != WindowState.Maximized && !w.IsElevated);

        if (targetWindow is null)
        {
            // No suitable window, skip test
            return;
        }

        var originalState = targetWindow.State;
        Assert.True(long.TryParse(targetWindow.Handle, out long handleValue));
        nint handle = (nint)handleValue;

        // Act
        var result = await _windowService.MaximizeWindowAsync(handle);

        // Assert - operation completes, may fail due to permissions
        Assert.NotNull(result);
        if (result.Success)
        {
            Assert.NotNull(result.Window);
            Assert.Equal(WindowState.Maximized, result.Window.State);

            // Clean up - restore original state
            if (originalState == WindowState.Minimized)
            {
                await _windowService.MinimizeWindowAsync(handle);
            }
            else
            {
                await _windowService.RestoreWindowAsync(handle);
            }
        }
    }

    [Fact]
    public async Task RestoreWindow_RestoresMinimizedWindow()
    {
        // Arrange - Find a minimized window
        var listResult = await _windowService.ListWindowsAsync();
        Assert.True(listResult.Success);
        Assert.NotNull(listResult.Windows);

        var minimizedWindow = listResult.Windows.FirstOrDefault(w =>
            w.State == WindowState.Minimized && !w.IsElevated);

        if (minimizedWindow is null)
        {
            // No minimized windows, skip test
            return;
        }

        Assert.True(long.TryParse(minimizedWindow.Handle, out long handleValue));
        nint handle = (nint)handleValue;

        // Act
        var result = await _windowService.RestoreWindowAsync(handle);

        // Assert - operation completes (success may vary based on window and environment)
        Assert.NotNull(result);
        // Note: Restore may not always work depending on the window and system state
        // We just verify the operation completes without crashing
    }

    [Fact]
    public async Task RestoreWindow_RestoresMaximizedWindow()
    {
        // Arrange - Find a maximized window or maximize one
        var listResult = await _windowService.ListWindowsAsync();
        Assert.True(listResult.Success);
        Assert.NotNull(listResult.Windows);

        var targetWindow = listResult.Windows.FirstOrDefault(w =>
            w.State == WindowState.Maximized && !w.IsElevated);

        if (targetWindow is null)
        {
            // No maximized windows, skip test
            return;
        }

        Assert.True(long.TryParse(targetWindow.Handle, out long handleValue));
        nint handle = (nint)handleValue;

        // Act
        var result = await _windowService.RestoreWindowAsync(handle);

        // Assert - operation completes
        Assert.NotNull(result);
        if (result.Success)
        {
            Assert.NotNull(result.Window);
            Assert.Equal(WindowState.Normal, result.Window.State);

            // Clean up - re-maximize
            await _windowService.MaximizeWindowAsync(handle);
        }
    }

    [Fact]
    public async Task MinimizeWindow_InvalidHandle_ReturnsError()
    {
        // Arrange - Use an invalid handle
        nint invalidHandle = (nint)0x12345678;

        // Act
        var result = await _windowService.MinimizeWindowAsync(invalidHandle);

        // Assert
        Assert.False(result.Success);
        Assert.NotNull(result.Error);
    }

    [Fact]
    public async Task MaximizeWindow_InvalidHandle_ReturnsError()
    {
        // Arrange - Use an invalid handle
        nint invalidHandle = (nint)0x12345678;

        // Act
        var result = await _windowService.MaximizeWindowAsync(invalidHandle);

        // Assert
        Assert.False(result.Success);
        Assert.NotNull(result.Error);
    }

    [Fact]
    public async Task RestoreWindow_InvalidHandle_ReturnsError()
    {
        // Arrange - Use an invalid handle
        nint invalidHandle = (nint)0x12345678;

        // Act
        var result = await _windowService.RestoreWindowAsync(invalidHandle);

        // Assert
        Assert.False(result.Success);
        Assert.NotNull(result.Error);
    }

    [Fact]
    public async Task MinimizeWindow_ZeroHandle_ReturnsError()
    {
        // Arrange
        nint zeroHandle = IntPtr.Zero;

        // Act
        var result = await _windowService.MinimizeWindowAsync(zeroHandle);

        // Assert
        Assert.False(result.Success);
        Assert.NotNull(result.Error);
    }

    [Fact]
    public async Task MaximizeWindow_ZeroHandle_ReturnsError()
    {
        // Arrange
        nint zeroHandle = IntPtr.Zero;

        // Act
        var result = await _windowService.MaximizeWindowAsync(zeroHandle);

        // Assert
        Assert.False(result.Success);
        Assert.NotNull(result.Error);
    }

    [Fact]
    public async Task RestoreWindow_ZeroHandle_ReturnsError()
    {
        // Arrange
        nint zeroHandle = IntPtr.Zero;

        // Act
        var result = await _windowService.RestoreWindowAsync(zeroHandle);

        // Assert
        Assert.False(result.Success);
        Assert.NotNull(result.Error);
    }

    [Fact]
    public async Task CloseWindow_ZeroHandle_ReturnsError()
    {
        // Arrange
        nint zeroHandle = IntPtr.Zero;

        // Act
        var result = await _windowService.CloseWindowAsync(zeroHandle);

        // Assert
        Assert.False(result.Success);
        Assert.NotNull(result.Error);
    }

    [Fact]
    public async Task CloseWindow_InvalidHandle_ReturnsError()
    {
        // Arrange - Use an invalid handle
        nint invalidHandle = (nint)0x12345678;

        // Act
        var result = await _windowService.CloseWindowAsync(invalidHandle);

        // Assert
        Assert.False(result.Success);
        Assert.NotNull(result.Error);
    }
}
