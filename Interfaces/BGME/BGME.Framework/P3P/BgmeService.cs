using BGME.Framework.Music;
using Reloaded.Hooks.Definitions;
using Reloaded.Memory.SigScan.ReloadedII.Interfaces;
using Ryo.Interfaces;

namespace BGME.Framework.P3P;

internal class BgmeService : IBgmeService
{
    private readonly BgmService soundPatcher;
    private readonly EncounterBgm encounterPatcher;
    private readonly FloorBgm floorBgm;

    public BgmeService(
        IRyoApi ryo,
        ICriAtomEx criAtomEx,
        ICriAtomRegistry criAtomRegistry,
        MusicService music)
    {
        this.soundPatcher = new(ryo, criAtomEx, criAtomRegistry, music);
        this.encounterPatcher = new(music);
        this.floorBgm = new(music);
    }

    public void Initialize(IStartupScanner scanner, IReloadedHooks hooks)
    {
    }

    public void SetVictoryDisabled(bool isDisabled)
    {
        this.soundPatcher.SetVictoryDisabled(isDisabled);
        this.encounterPatcher.SetVictoryDisabled(isDisabled);
    }
}
