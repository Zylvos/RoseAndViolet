namespace PersonaMusicScript.Types.Games;

public class GameMusic
{
    public string DefaultOutputPath { get; set; } = string.Empty;

    public string? DefaultEncoder { get; set; }

    public List<AudioTrack> Tracks { get; set; } = new();
}
