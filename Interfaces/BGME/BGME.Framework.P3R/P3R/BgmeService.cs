using BGME.Framework.Music;
using Ryo.Interfaces;

namespace BGME.Framework.P3R.P3R;

internal class BgmeService : IBgmeService
{
    private readonly EncounterBgm encounterBgm;
    private readonly BgmService bgm;

    public BgmeService(ICriAtomEx criAtomEx, MusicService music)
    {
        this.bgm = new(criAtomEx, music);
        this.encounterBgm = new(music);
    }

    public void SetVictoryDisabled(bool isDisabled)
    {
        this.bgm.SetVictoryDisabled(isDisabled);
        this.encounterBgm.SetVictoryDisabled(isDisabled);
    }
}
