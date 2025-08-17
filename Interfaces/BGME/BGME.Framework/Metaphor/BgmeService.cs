using BGME.Framework.Music;

namespace BGME.Framework.Metaphor;

internal class BgmeService : IBgmeService
{
    private readonly BgmService _bgm;
    private readonly EncounterBgm _encounterBgm;

    public BgmeService(MusicService music)
    {
        _bgm = new(music);
        _encounterBgm = new(music);
    }

    public void SetVictoryDisabled(bool isDisabled)
    {
        _bgm.SetVictoryDisabled(isDisabled);
        _encounterBgm.SetVictoryDisabled(isDisabled);
    }
}
