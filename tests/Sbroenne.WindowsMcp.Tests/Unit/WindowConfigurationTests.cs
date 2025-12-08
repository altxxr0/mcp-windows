using Sbroenne.WindowsMcp.Configuration;

namespace Sbroenne.WindowsMcp.Tests.Unit;

/// <summary>
/// Unit tests for <see cref="WindowConfiguration"/>.
/// </summary>
public class WindowConfigurationTests
{
    [Fact]
    public void Constructor_WithDefaults_HasExpectedValues()
    {
        // Act
        var config = new WindowConfiguration();

        // Assert
        Assert.Equal(5000, config.OperationTimeoutMs);
        Assert.Equal(30000, config.WaitForTimeoutMs);
        Assert.Equal(100, config.PropertyQueryTimeoutMs);
    }

    [Fact]
    public void Constructor_WithCustomValues_RetainsValues()
    {
        // Arrange & Act
        var config = new WindowConfiguration
        {
            OperationTimeoutMs = 10000,
            WaitForTimeoutMs = 60000,
            PropertyQueryTimeoutMs = 250
        };

        // Assert
        Assert.Equal(10000, config.OperationTimeoutMs);
        Assert.Equal(60000, config.WaitForTimeoutMs);
        Assert.Equal(250, config.PropertyQueryTimeoutMs);
    }

    [Fact]
    public void FromEnvironment_WithNoVariables_ReturnsDefaults()
    {
        // Arrange - Clear any existing environment variables
        Environment.SetEnvironmentVariable("MCP_WINDOW_TIMEOUT_MS", null);
        Environment.SetEnvironmentVariable("MCP_WINDOW_WAITFOR_TIMEOUT_MS", null);
        Environment.SetEnvironmentVariable("MCP_WINDOW_PROPERTY_TIMEOUT_MS", null);

        // Act
        var config = WindowConfiguration.FromEnvironment();

        // Assert
        Assert.Equal(5000, config.OperationTimeoutMs);
        Assert.Equal(30000, config.WaitForTimeoutMs);
        Assert.Equal(100, config.PropertyQueryTimeoutMs);
    }

    [Fact]
    public void FromEnvironment_WithVariables_ParsesValues()
    {
        // Arrange
        Environment.SetEnvironmentVariable("MCP_WINDOW_TIMEOUT_MS", "7500");
        Environment.SetEnvironmentVariable("MCP_WINDOW_WAITFOR_TIMEOUT_MS", "45000");
        Environment.SetEnvironmentVariable("MCP_WINDOW_PROPERTY_TIMEOUT_MS", "200");

        try
        {
            // Act
            var config = WindowConfiguration.FromEnvironment();

            // Assert
            Assert.Equal(7500, config.OperationTimeoutMs);
            Assert.Equal(45000, config.WaitForTimeoutMs);
            Assert.Equal(200, config.PropertyQueryTimeoutMs);
        }
        finally
        {
            // Cleanup
            Environment.SetEnvironmentVariable("MCP_WINDOW_TIMEOUT_MS", null);
            Environment.SetEnvironmentVariable("MCP_WINDOW_WAITFOR_TIMEOUT_MS", null);
            Environment.SetEnvironmentVariable("MCP_WINDOW_PROPERTY_TIMEOUT_MS", null);
        }
    }

    [Fact]
    public void FromEnvironment_WithInvalidValue_ReturnsDefault()
    {
        // Arrange
        Environment.SetEnvironmentVariable("MCP_WINDOW_TIMEOUT_MS", "not-a-number");

        try
        {
            // Act
            var config = WindowConfiguration.FromEnvironment();

            // Assert - should use default when parsing fails
            Assert.Equal(5000, config.OperationTimeoutMs);
        }
        finally
        {
            // Cleanup
            Environment.SetEnvironmentVariable("MCP_WINDOW_TIMEOUT_MS", null);
        }
    }

    [Fact]
    public void WithImmutability_RecordWorksCorrectly()
    {
        // Arrange
        var original = new WindowConfiguration
        {
            OperationTimeoutMs = 5000,
            WaitForTimeoutMs = 30000,
            PropertyQueryTimeoutMs = 100
        };

        // Act
        var modified = original with { OperationTimeoutMs = 10000 };

        // Assert
        Assert.Equal(5000, original.OperationTimeoutMs); // Original unchanged
        Assert.Equal(10000, modified.OperationTimeoutMs); // New value
        Assert.Equal(30000, modified.WaitForTimeoutMs); // Copied from original
    }
}
