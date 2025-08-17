namespace PersonaMusicScript.Types.Games;

public class AudioTrack
{
    public string Name { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string[] Tags { get; set; } = Array.Empty<string>();

    public int CueId { get; set; }

    public string? OutputPath { get; set; }

    public string? Encoder { get; set; }
}
