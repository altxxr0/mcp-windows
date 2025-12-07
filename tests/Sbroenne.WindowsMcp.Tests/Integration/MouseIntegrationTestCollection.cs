namespace Sbroenne.WindowsMcp.Tests.Integration;

/// <summary>
/// Test collection for mouse integration tests that require exclusive access to the mouse cursor.
/// Tests in this collection will run sequentially to avoid cursor position interference.
/// </summary>
[Xunit.CollectionDefinition("MouseIntegrationTests", DisableParallelization = true)]
#pragma warning disable CA1711 // Identifiers should not have incorrect suffix - required by xUnit naming convention
public class MouseIntegrationTestCollection : Xunit.ICollectionFixture<MouseIntegrationTestFixture>
#pragma warning restore CA1711
{
}

/// <summary>
/// Fixture for mouse integration tests. Can be used for shared setup/teardown if needed.
/// </summary>
public class MouseIntegrationTestFixture : IDisposable
{
    public MouseIntegrationTestFixture()
    {
        // Any shared setup can go here
    }

    public void Dispose()
    {
        // Any shared cleanup can go here
        GC.SuppressFinalize(this);
    }
}
