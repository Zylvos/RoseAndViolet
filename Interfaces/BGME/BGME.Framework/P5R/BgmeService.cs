using BGME.Framework.Music;

namespace BGME.Framework.P5R;

internal class BgmeService : IBgmeService
{
    private readonly BgmService bgm;
    private readonly EncounterBgm encounterBgm;

    public BgmeService(MusicService music)
    {
        this.bgm = new(music);
        this.encounterBgm = new(music);
    }

    public void SetVictoryDisabled(bool isDisabled)
    {
        Log.Debug($"Disable Victory BGM: {isDisabled}");
        this.bgm.SetVictoryDisabled(isDisabled);
        this.encounterBgm.SetVictoryDisabled(isDisabled);
    }
}
