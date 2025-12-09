using System.Text.Json.Serialization;

namespace Sbroenne.WindowsMcp.Models;

/// <summary>
/// Result of a visual diff computation between two images.
/// </summary>
public sealed record VisualDiffResult
{
    /// <summary>
    /// Gets the filename of the before screenshot.
    /// </summary>
    [JsonPropertyName("before_image")]
    public required string BeforeImage { get; init; }

    /// <summary>
    /// Gets the filename of the after screenshot.
    /// </summary>
    [JsonPropertyName("after_image")]
    public required string AfterImage { get; init; }

    /// <summary>
    /// Gets the filename of the generated diff image (if changes detected).
    /// </summary>
    [JsonPropertyName("diff_image")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? DiffImage { get; init; }

    /// <summary>
    /// Gets the base64-encoded PNG diff image data (if changes detected).
    /// </summary>
    [JsonPropertyName("diff_image_data")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? DiffImageData { get; init; }

    /// <summary>
    /// Gets the number of pixels that differ between images.
    /// </summary>
    [JsonPropertyName("changed_pixels")]
    public required int ChangedPixels { get; init; }

    /// <summary>
    /// Gets the total number of pixels compared.
    /// </summary>
    [JsonPropertyName("total_pixels")]
    public required int TotalPixels { get; init; }

    /// <summary>
    /// Gets the percentage of pixels that changed (0-100).
    /// </summary>
    [JsonPropertyName("change_percentage")]
    public required double ChangePercentage { get; init; }

    /// <summary>
    /// Gets the threshold percentage used to determine significance.
    /// </summary>
    [JsonPropertyName("threshold")]
    public required double Threshold { get; init; }

    /// <summary>
    /// Gets whether the change percentage exceeds the threshold.
    /// </summary>
    [JsonPropertyName("is_significant_change")]
    public required bool IsSignificantChange { get; init; }

    /// <summary>
    /// Gets the per-channel tolerance used for pixel comparison (0-255).
    /// </summary>
    [JsonPropertyName("pixel_tolerance")]
    public required int PixelTolerance { get; init; }

    /// <summary>
    /// Gets the time taken to compute the diff in milliseconds.
    /// </summary>
    [JsonPropertyName("computation_time_ms")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? ComputationTimeMs { get; init; }

    /// <summary>
    /// Gets bounding boxes of contiguous changed regions (optional).
    /// </summary>
    [JsonPropertyName("changed_regions")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<ChangedRegion>? ChangedRegions { get; init; }

    /// <summary>
    /// Gets whether before and after images have different dimensions.
    /// </summary>
    [JsonPropertyName("dimensions_mismatch")]
    public bool DimensionsMismatch { get; init; }

    /// <summary>
    /// Gets the error message if diff computation failed.
    /// </summary>
    [JsonPropertyName("error")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Error { get; init; }

    /// <summary>
    /// Gets a value indicating whether the diff computation succeeded.
    /// </summary>
    [JsonPropertyName("success")]
    public bool Success => Error is null;
}

/// <summary>
/// Represents a bounding box of a changed region within the image.
/// </summary>
public sealed record ChangedRegion
{
    /// <summary>
    /// Gets the left edge of the changed region.
    /// </summary>
    [JsonPropertyName("x")]
    public required int X { get; init; }

    /// <summary>
    /// Gets the top edge of the changed region.
    /// </summary>
    [JsonPropertyName("y")]
    public required int Y { get; init; }

    /// <summary>
    /// Gets the width of the changed region.
    /// </summary>
    [JsonPropertyName("width")]
    public required int Width { get; init; }

    /// <summary>
    /// Gets the height of the changed region.
    /// </summary>
    [JsonPropertyName("height")]
    public required int Height { get; init; }
}
