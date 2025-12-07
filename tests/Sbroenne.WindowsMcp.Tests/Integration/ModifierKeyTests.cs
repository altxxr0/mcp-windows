using Sbroenne.WindowsMcp.Input;
using Sbroenne.WindowsMcp.Models;

namespace Sbroenne.WindowsMcp.Tests.Integration;

/// <summary>
/// Integration tests for modifier key support (Ctrl, Shift, Alt) during click operations.
/// </summary>
[Collection("MouseIntegrationTests")]
public sealed class ModifierKeyTests
{
    private readonly MouseInputService _service = new();

    [Fact]
    public async Task ClickAsync_WithControlModifier_Succeeds()
    {
        // Arrange - use center of primary screen
        var x = 500;
        var y = 500;

        // Act
        var result = await _service.ClickAsync(x, y, ModifierKey.Ctrl);

        // Assert - operation should succeed even if target is elevated
        Assert.True(
            result.ErrorCode == MouseControlErrorCode.Success ||
            result.ErrorCode == MouseControlErrorCode.ElevatedProcessTarget,
            $"Expected success or elevated process target, got {result.ErrorCode}: {result.ErrorMessage}");
    }

    [Fact]
    public async Task ClickAsync_WithShiftModifier_Succeeds()
    {
        // Arrange - use center of primary screen
        var x = 500;
        var y = 500;

        // Act
        var result = await _service.ClickAsync(x, y, ModifierKey.Shift);

        // Assert - operation should succeed even if target is elevated
        Assert.True(
            result.ErrorCode == MouseControlErrorCode.Success ||
            result.ErrorCode == MouseControlErrorCode.ElevatedProcessTarget,
            $"Expected success or elevated process target, got {result.ErrorCode}: {result.ErrorMessage}");
    }

    [Fact]
    public async Task ClickAsync_WithAltModifier_Succeeds()
    {
        // Arrange - use center of primary screen
        var x = 500;
        var y = 500;

        // Act
        var result = await _service.ClickAsync(x, y, ModifierKey.Alt);

        // Assert - operation should succeed even if target is elevated
        Assert.True(
            result.ErrorCode == MouseControlErrorCode.Success ||
            result.ErrorCode == MouseControlErrorCode.ElevatedProcessTarget,
            $"Expected success or elevated process target, got {result.ErrorCode}: {result.ErrorMessage}");
    }

    [Fact]
    public async Task ClickAsync_WithMultipleModifiers_Succeeds()
    {
        // Arrange - use center of primary screen
        var x = 500;
        var y = 500;
        var modifiers = ModifierKey.Ctrl | ModifierKey.Shift;

        // Act
        var result = await _service.ClickAsync(x, y, modifiers);

        // Assert - operation should succeed even if target is elevated
        Assert.True(
            result.ErrorCode == MouseControlErrorCode.Success ||
            result.ErrorCode == MouseControlErrorCode.ElevatedProcessTarget,
            $"Expected success or elevated process target, got {result.ErrorCode}: {result.ErrorMessage}");
    }

    [Fact]
    public async Task ClickAsync_WithAllModifiers_Succeeds()
    {
        // Arrange - use center of primary screen
        var x = 500;
        var y = 500;
        var modifiers = ModifierKey.Ctrl | ModifierKey.Shift | ModifierKey.Alt;

        // Act
        var result = await _service.ClickAsync(x, y, modifiers);

        // Assert - operation should succeed even if target is elevated
        Assert.True(
            result.ErrorCode == MouseControlErrorCode.Success ||
            result.ErrorCode == MouseControlErrorCode.ElevatedProcessTarget,
            $"Expected success or elevated process target, got {result.ErrorCode}: {result.ErrorMessage}");
    }

    [Fact]
    public async Task DoubleClickAsync_WithControlModifier_Succeeds()
    {
        // Arrange - use center of primary screen
        var x = 500;
        var y = 500;

        // Act
        var result = await _service.DoubleClickAsync(x, y, ModifierKey.Ctrl);

        // Assert - operation should succeed even if target is elevated
        Assert.True(
            result.ErrorCode == MouseControlErrorCode.Success ||
            result.ErrorCode == MouseControlErrorCode.ElevatedProcessTarget,
            $"Expected success or elevated process target, got {result.ErrorCode}: {result.ErrorMessage}");
    }

    [Fact]
    public async Task RightClickAsync_WithShiftModifier_Succeeds()
    {
        // Arrange - use center of primary screen
        var x = 500;
        var y = 500;

        // Act
        var result = await _service.RightClickAsync(x, y, ModifierKey.Shift);

        // Assert - operation should succeed even if target is elevated
        Assert.True(
            result.ErrorCode == MouseControlErrorCode.Success ||
            result.ErrorCode == MouseControlErrorCode.ElevatedProcessTarget,
            $"Expected success or elevated process target, got {result.ErrorCode}: {result.ErrorMessage}");
    }

    [Fact]
    public async Task ClickAsync_AtCurrentPosition_WithModifier_Succeeds()
    {
        // Arrange - no coordinates, click at current position with modifier
        var modifiers = ModifierKey.Shift;

        // Act
        var result = await _service.ClickAsync(null, null, modifiers);

        // Assert - operation should succeed even if target is elevated
        Assert.True(
            result.ErrorCode == MouseControlErrorCode.Success ||
            result.ErrorCode == MouseControlErrorCode.ElevatedProcessTarget,
            $"Expected success or elevated process target, got {result.ErrorCode}: {result.ErrorMessage}");
    }
}
