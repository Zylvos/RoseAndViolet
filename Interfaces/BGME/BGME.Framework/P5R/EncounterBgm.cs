using BGME.Framework.Models;
using BGME.Framework.Music;
using BGME.Framework.P5R.Rhythm;
using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.Enums;
using Reloaded.Hooks.Definitions.X64;
using static Reloaded.Hooks.Definitions.X64.FunctionAttribute;

namespace BGME.Framework.P5R;

internal unsafe class EncounterBgm : BaseEncounterBgm
{
    [Function(new Register[] { Register.rbx, Register.rax }, Register.rax, true)]
    private delegate void GetEncounterBgmId(nint encounterPtr, int originalBgmId);
    private IReverseWrapper<GetEncounterBgmId>? getEncounterBgmWrapper;
    private IAsmHook? getEncounterBgmHook;

    [Function(Register.rdx, Register.rax, true)]
    private delegate int GetVictoryBgmFunction(int defaultBgmId);
    private IReverseWrapper<GetVictoryBgmFunction>? victoryBgmWrapper;

    [Function(CallingConventions.Microsoft)]
    private delegate byte BIT_CHK(uint flag);
    private BIT_CHK? bitChk;

    private IAsmHook? victoryBgmHook;
    private IAsmHook? victoryBgmHook2;
    private string? victoryBgmCall;
    private readonly RhythmGame? rhythmGame;

    public EncounterBgm(MusicService music, RhythmGame? rhythmGame = null)
        : base(music)
    {
        this.rhythmGame = rhythmGame;

        ScanHooks.Add(
            "Encounter BGM",
            "8B 83 ?? ?? ?? ?? 3D 81 02 00 00",
            (hooks, result) =>
            {
                var patch = new string[]
                {
                    "use64",
                    Utilities.PushCallerRegisters,
                    hooks.Utilities.GetAbsoluteCallMnemonics(this.GetEncounterBgmIdImpl, out this.getEncounterBgmWrapper),
                    Utilities.PopCallerRegisters,
                };

                this.getEncounterBgmHook = hooks.CreateAsmHook(patch, result, AsmHookBehaviour.ExecuteFirst).Activate();
            });

        ScanHooks.Add(
            "Victory BGM",
            "BA 54 01 00 00 49 8B CC",
            (hooks, result) =>
            {
                this.victoryBgmCall ??= hooks.Utilities.GetAbsoluteCallMnemonics(this.GetVictoryBgm, out this.victoryBgmWrapper);

                var patch = new string[]
                {
                    "use64",
                    Utilities.PushCallerRegisters,
                    this.victoryBgmCall,
                    Utilities.PopCallerRegisters,
                    "cmp eax, -1",
                    "jng original",
                    "mov edx, eax",
                    "label original",
                };

                this.victoryBgmHook = hooks.CreateAsmHook(patch, result + 5, AsmHookBehaviour.ExecuteFirst).Activate();
            });

        ScanHooks.Add(
            "Victory BGM on Victory",
            "BA 54 01 00 00 48 8B CE E8 ?? ?? ?? ?? 33 D2",
            (hooks, result) =>
            {
                this.victoryBgmCall ??= hooks.Utilities.GetAbsoluteCallMnemonics(this.GetVictoryBgm, out this.victoryBgmWrapper);

                var patch = new string[]
                {
                    "use64",
                    Utilities.PushCallerRegisters,
                    this.victoryBgmCall,
                    Utilities.PopCallerRegisters,
                    "cmp eax, -1",
                    "jng original",
                    "mov edx, eax",
                    "label original",
                };

                this.victoryBgmHook2 = hooks.CreateAsmHook(patch, result + 5, AsmHookBehaviour.ExecuteFirst).Activate();
            });

        ScanHooks.Add(
            nameof(BIT_CHK),
            "4C 8D 05 ?? ?? ?? ?? 33 C0 49 8B D0 0F 1F 40 00 39 0A 74 ?? FF C0 48 83 C2 08 83 F8 10 72 ?? 8B D1",
            (hooks, result) => this.bitChk = hooks.CreateWrapper<BIT_CHK>(result, out _));
    }

    private int GetVictoryBgm(int defaultBgmId)
    {
        var victoryMusicId = this.GetVictoryMusic();
        if (victoryMusicId != -1)
        {
            return victoryMusicId;
        }

        return defaultBgmId;
    }

    private void GetEncounterBgmIdImpl(nint encounterPtr, int originalBgmId)
    {
        this.rhythmGame?.StartBattleBgm();
        var encounterId = (int*)(encounterPtr + 0x278);
        var context = (EncounterContext) (*(int*)(encounterPtr + 0x28c));

        // P5R swaps encounter context values.
        if (context == EncounterContext.Advantage)
        {
            context = EncounterContext.Disadvantage;
        }
        else if (context == EncounterContext.Disadvantage)
        {
            context = EncounterContext.Advantage;
        }

        var battleMusicId = this.GetBattleMusic(*encounterId, context);
        if (battleMusicId == -1)
        {
            return;
        }

        // Write bgm id to encounter bgm var.
        var encounterBgm = (int*)(encounterPtr + 0x9ac);
        var inHeist = this.bitChk!(0x20000050) == 1;
        var isSpecialBattle = originalBgmId != -1;  // EAX register equals BGM value from ENC TBL minus 1.
                                                    // Normal battles have BGM value 0 in TBL.

        Log.Debug($"Encounter BGM ID: {originalBgmId} || Heist: {inHeist}");
        if (isSpecialBattle || inHeist == false)
        {
            *encounterBgm = battleMusicId;
            Log.Debug($"Encounter BGM ID written: {battleMusicId}");
        }
    }
}
