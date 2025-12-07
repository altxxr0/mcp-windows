using Sbroenne.WindowsMcp.Configuration;

namespace Sbroenne.WindowsMcp.Tests.Unit;

/// <summary>
/// Unit tests for <see cref="MouseConfiguration"/>.
/// </summary>
public sealed class MouseConfigurationTests
{
    [Fact]
    public void DefaultTimeout_ShouldBe5000Ms()
    {
        // Arrange & Act
        var config = new MouseConfiguration();

        // Assert
        Assert.Equal(5000, config.TimeoutMs);
    }

    [Fact]
    public void DefaultTimeoutMs_ConstantShouldBe5000()
    {
        // Assert
        Assert.Equal(5000, MouseConfiguration.DefaultTimeoutMs);
    }

    [Fact]
    public void TimeoutEnvironmentVariable_ShouldBeCorrectName()
    {
        // Assert
        Assert.Equal("MCP_MOUSE_TIMEOUT_MS", MouseConfiguration.TimeoutEnvironmentVariable);
    }

    [Fact]
    public void FromEnvironment_WithNoEnvVar_ShouldReturnDefaultTimeout()
    {
        // Arrange - Ensure env var is not set
        var previousValue = Environment.GetEnvironmentVariable("MCP_MOUSE_TIMEOUT_MS");
        try
        {
            Environment.SetEnvironmentVariable("MCP_MOUSE_TIMEOUT_MS", null);

            // Act
            var config = MouseConfiguration.FromEnvironment();

            // Assert
            Assert.Equal(5000, config.TimeoutMs);
        }
        finally
        {
            // Restore previous value
            Environment.SetEnvironmentVariable("MCP_MOUSE_TIMEOUT_MS", previousValue);
        }
    }

    [Fact]
    public void FromEnvironment_WithValidEnvVar_ShouldReturnConfiguredTimeout()
    {
        // Arrange
        var previousValue = Environment.GetEnvironmentVariable("MCP_MOUSE_TIMEOUT_MS");
        try
        {
            Environment.SetEnvironmentVariable("MCP_MOUSE_TIMEOUT_MS", "10000");

            // Act
            var config = MouseConfiguration.FromEnvironment();

            // Assert
            Assert.Equal(10000, config.TimeoutMs);
        }
        finally
        {
            // Restore previous value
            Environment.SetEnvironmentVariable("MCP_MOUSE_TIMEOUT_MS", previousValue);
        }
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("")]
    [InlineData("-1")]
    [InlineData("0")]
    public void FromEnvironment_WithInvalidEnvVar_ShouldReturnDefaultTimeout(string invalidValue)
    {
        // Arrange
        var previousValue = Environment.GetEnvironmentVariable("MCP_MOUSE_TIMEOUT_MS");
        try
        {
            Environment.SetEnvironmentVariable("MCP_MOUSE_TIMEOUT_MS", invalidValue);

            // Act
            var config = MouseConfiguration.FromEnvironment();

            // Assert
            Assert.Equal(5000, config.TimeoutMs);
        }
        finally
        {
            // Restore previous value
            Environment.SetEnvironmentVariable("MCP_MOUSE_TIMEOUT_MS", previousValue);
        }
    }
}
