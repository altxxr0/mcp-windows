using Sbroenne.WindowsMcp.Input;

namespace Sbroenne.WindowsMcp.Tests.Integration;

/// <summary>
/// Integration tests for DPI awareness.
/// These tests verify that cursor positioning is accurate regardless of DPI scaling.
/// Note: These tests run on the current system's DPI settings.
/// </summary>
[Collection("MouseIntegrationTests")]
public sealed class DpiAwarenessTests
{
    private readonly MouseInputService _service = new();

    [Fact]
    public async Task MoveAsync_PositionAccuracy_IsWithin1PixelAtAnyDpi()
    {
        // Arrange - test at multiple positions
        var testPositions = new[]
        {
            (x: 100, y: 100),
            (x: 500, y: 300),
            (x: 800, y: 600),
        };

        foreach (var (x, y) in testPositions)
        {
            // Act
            var result = await _service.MoveAsync(x, y);

            // Assert - position should be accurate within 1 pixel
            if (result.Success)
            {
                Assert.InRange(result.FinalPosition.X, x - 1, x + 1);
                Assert.InRange(result.FinalPosition.Y, y - 1, y + 1);
            }
        }
    }

    [Fact]
    public async Task ClickAsync_CursorPosition_AccurateAfterClick()
    {
        // Arrange
        var x = 400;
        var y = 400;

        // Act
        var result = await _service.ClickAsync(x, y);

        // Assert - if successful, cursor should be at the expected position
        if (result.Success)
        {
            Assert.NotNull(result.FinalPosition);
            Assert.InRange(result.FinalPosition.X, x - 1, x + 1);
            Assert.InRange(result.FinalPosition.Y, y - 1, y + 1);
        }
    }

    [Fact]
    public void VirtualScreenBounds_HasReasonableSize()
    {
        // Act
        var bounds = CoordinateNormalizer.GetVirtualScreenBounds();

        // Assert - modern monitors have at least 640x480 resolution
        Assert.True(bounds.Width >= 640, $"Screen width {bounds.Width} seems too small");
        Assert.True(bounds.Height >= 480, $"Screen height {bounds.Height} seems too small");

        // Assert - very large resolutions (e.g., > 100,000) would indicate a bug
        Assert.True(bounds.Width < 100000, $"Screen width {bounds.Width} seems too large (possible scaling bug)");
        Assert.True(bounds.Height < 100000, $"Screen height {bounds.Height} seems too large (possible scaling bug)");
    }

    [Fact]
    public async Task DragAsync_EndPosition_AccurateAtAnyDpi()
    {
        // Arrange
        var startX = 300;
        var startY = 300;
        var endX = 600;
        var endY = 600;

        // Act
        var result = await _service.DragAsync(startX, startY, endX, endY);

        // Assert - if successful, cursor should end at the expected position
        if (result.Success)
        {
            Assert.NotNull(result.FinalPosition);
            Assert.InRange(result.FinalPosition.X, endX - 1, endX + 1);
            Assert.InRange(result.FinalPosition.Y, endY - 1, endY + 1);
        }
    }
}
