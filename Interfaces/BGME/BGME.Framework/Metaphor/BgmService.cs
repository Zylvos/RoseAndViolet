using BGME.Framework.Music;
using Reloaded.Hooks.Definitions;

namespace BGME.Framework.Metaphor;

internal class BgmService : BaseBgm
{
    private delegate void SndPlayBgm(int bgmCueId);
    private IHook<SndPlayBgm>? _playBgmHook;

    public BgmService(MusicService music) : base(music)
    {
        ScanHooks.Add(
            nameof(SndPlayBgm),
            "48 89 5C 24 ?? 57 48 83 EC 70 48 8B 05 ?? ?? ?? ?? 8B F9",
            (hooks, result) => _playBgmHook = hooks.CreateHook<SndPlayBgm>(this.SndPlayBgmImpl, result).Activate());
    }

    private void SndPlayBgmImpl(int cueId)
    {
        var currentBgmId = this.GetGlobalBgmId(cueId);
        if (currentBgmId == null)
        {
            return;
        }

        Log.Debug($"Playing BGM ID: {currentBgmId}");
        _playBgmHook!.OriginalFunction((int)currentBgmId);
    }

    protected override int VictoryBgmId => 1090;

    protected override void PlayBgm(int bgmId) => SndPlayBgmImpl(bgmId);
}
