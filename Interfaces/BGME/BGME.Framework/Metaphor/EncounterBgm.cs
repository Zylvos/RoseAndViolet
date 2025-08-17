using BGME.Framework.Models;
using BGME.Framework.Music;
using Reloaded.Hooks.Definitions;
using System.Runtime.InteropServices;

namespace BGME.Framework.Metaphor;

internal unsafe class EncounterBgm : BaseEncounterBgm
{
    private delegate void ExecBattle(BattleWrapper* btlWrapper, nint param2);
    private IHook<ExecBattle>? _execBattle;

    private delegate EncountEntry* GetEncountEntryByBtlObj(BattleObject* btlObj);
    private GetEncountEntryByBtlObj? _getEncEntry;

    private delegate void SetBattleBgm(Battle* btl, int bgmId);
    private IHook<SetBattleBgm>? _setBgmSpecHook;

    public EncounterBgm(MusicService music) : base(music)
    {
        ScanHooks.Add(
            nameof(ExecBattle),
            "48 8B C4 55 53 48 8D 68 ?? 48 81 EC 18 01 00 00 83 79 ?? 00",
            (hooks, result) => _execBattle = hooks.CreateHook<ExecBattle>(ExecBattleImpl, result).Activate());

        ScanHooks.Add(
            nameof(GetEncountEntryByBtlObj),
            "0F B7 89 ?? ?? ?? ?? E9",
            (hooks, result) => _getEncEntry = hooks.CreateWrapper<GetEncountEntryByBtlObj>(result, out _));

        ScanHooks.Add(
            nameof(SetBattleBgm),
            "48 89 5C 24 ?? 57 48 83 EC 20 8B DA 48 8B F9 E8 ?? ?? ?? ?? 3B C3 74",
            (hooks, result) => _setBgmSpecHook = hooks.CreateHook<SetBattleBgm>(SetBattleBgmImpl, result).Activate());
    }

    private void SetBattleBgmImpl(Battle* btl, int bgmId)
    {
        var encounterId = btl->EncounterId;
        var newBgmId = bgmId == 1090 ? this.GetVictoryMusic() : this.GetBattleMusic(encounterId, EncounterContext.Normal);
        if (newBgmId != -1)
        {
            _setBgmSpecHook!.OriginalFunction(btl, newBgmId);
        }
        else
        {
            _setBgmSpecHook!.OriginalFunction(btl, bgmId);
        }
    }

    private void ExecBattleImpl(BattleWrapper* btlWrapper, nint param2)
    {
        var encounterId = btlWrapper->battleObj->EncounterId;
        var context = ToBgmeContext(btlWrapper->Context);

        var newBgmId = this.GetBattleMusic(encounterId, context);
        if (newBgmId != -1)
        {
            var encounter = _getEncEntry!(btlWrapper->battleObj);
            encounter->BgmId = (ushort)newBgmId;
        }

        _execBattle!.OriginalFunction(btlWrapper, param2);
    }

    private static EncounterContext ToBgmeContext(EncounterContextMF ctx)
        => ctx switch
        {
            EncounterContextMF.Disadvantage => EncounterContext.Disadvantage,
            EncounterContextMF.Normal => EncounterContext.Normal,
            EncounterContextMF.Advantage => EncounterContext.Advantage,
            _ => EncounterContext.Unknown,
        };

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    private unsafe struct BattleWrapper
    {
        [FieldOffset(0x10)]
        public EncounterContextMF Context;

        [FieldOffset(0x18)]
        public BattleObject* battleObj;

        [FieldOffset(0x20)]
        public bool IsAdvantage;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    private unsafe struct BattleObject
    {
        [FieldOffset(0x3634)]
        public ushort EncounterId;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    private unsafe struct EncountEntry
    {
        [FieldOffset(0x18)]
        public ushort BgmId;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    private unsafe struct Battle
    {
        [FieldOffset(0x25C)]
        public ushort EncounterId;
    }

    private enum EncounterContextMF
    {
        Disadvantage,
        Normal,
        Advantage,
    }
}
