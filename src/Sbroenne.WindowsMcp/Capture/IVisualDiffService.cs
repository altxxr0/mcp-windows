using Sbroenne.WindowsMcp.Models;

namespace Sbroenne.WindowsMcp.Capture;

/// <summary>
/// Service for computing visual differences between screenshots.
/// </summary>
public interface IVisualDiffService
{
    /// <summary>
    /// Computes the visual difference between two base64-encoded PNG images.
    /// </summary>
    /// <param name="beforeImageBase64">Base64-encoded PNG of the "before" image.</param>
    /// <param name="afterImageBase64">Base64-encoded PNG of the "after" image.</param>
    /// <param name="beforeImageName">Name/identifier for the before image (for result metadata).</param>
    /// <param name="afterImageName">Name/identifier for the after image (for result metadata).</param>
    /// <param name="options">Options controlling diff computation (threshold, tolerance, etc.).</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>A result containing diff metrics, and optionally a diff image highlighting changes.</returns>
    Task<VisualDiffResult> ComputeDiffAsync(
        string beforeImageBase64,
        string afterImageBase64,
        string beforeImageName,
        string afterImageName,
        VisualDiffOptions? options = null,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Options for visual diff computation.
/// </summary>
public sealed record VisualDiffOptions
{
    /// <summary>
    /// Gets the percentage threshold for determining if changes are significant (0-100).
    /// Default is 1.0 (1% of pixels changed is considered significant).
    /// </summary>
    public double Threshold { get; init; } = 1.0;

    /// <summary>
    /// Gets the per-channel tolerance for pixel comparison (0-255).
    /// Pixels are considered different if any RGB channel differs by more than this value.
    /// Default is 10 to account for minor rendering variations.
    /// </summary>
    public int PixelTolerance { get; init; } = 10;

    /// <summary>
    /// Gets whether to generate a diff image highlighting changed pixels.
    /// Default is true.
    /// </summary>
    public bool GenerateDiffImage { get; init; } = true;

    /// <summary>
    /// Gets the color to use for highlighting changed pixels in the diff image (ARGB).
    /// Default is semi-transparent red (0x80FF0000).
    /// </summary>
    public uint HighlightColor { get; init; } = 0x80FF0000;
}
