using BGME.Framework.Music;
using Ryo.Interfaces;
using SharedScans.Interfaces;

namespace BGME.Framework.P4G;

internal class BgmeService : IBgmeService
{
    private readonly MusicService music;

    private readonly BgmService sound;
    private readonly EncounterBgm encounterPatcher;
    private readonly FloorBgm floorPatcher;
    private readonly EventBgm eventBgm;

    public BgmeService(
        ISharedScans scans,
        ICriAtomRegistry criAtomRegistry,
        MusicService music)
    {
        this.music = music;

        this.sound = new(scans, criAtomRegistry, this.music);
        this.encounterPatcher = new(music);
        this.floorPatcher = new(music);
        this.eventBgm = new(this.sound, music);
    }

    public void SetVictoryDisabled(bool isDisabled)
    {
        this.sound.SetVictoryDisabled(isDisabled);
        this.encounterPatcher.SetVictoryDisabled(isDisabled);
    }
}
