using BGME.Framework.Models;
using BGME.Framework.Music;
using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.Enums;
using Reloaded.Hooks.Definitions.X64;
using static Reloaded.Hooks.Definitions.X64.FunctionAttribute;

namespace BGME.Framework.P4G;

internal unsafe class EncounterBgm : BaseEncounterBgm
{
    [Function([Register.r8, Register.rcx], Register.rax, true)]
    private delegate int GetEncounterBgm(nint encounterPtr, int encounterId);
    private IReverseWrapper<GetEncounterBgm>? encounterReverseWrapper;
    private IAsmHook? encounterBgmHook;

    [Function([Register.rdx], Register.rax, true)]
    private delegate int GetVictoryBgm(int defaultMusicId);
    private IReverseWrapper<GetVictoryBgm>? victoryReverseWrapper;
    private IAsmHook? victoryBgmHook;

    public EncounterBgm(MusicService music)
        : base(music)
    {
        ScanHooks.Add(
            "Encounter BGM",
            "0F B7 4C D0 16",
            (hooks, result) =>
            {
                // TODO: Don't hardcode address!
                var encounterPatch = new string[]
                {
                    "use64",
                    "mov r12, rax",
                    Utilities.PushCallerRegisters,
                    hooks.Utilities.GetAbsoluteCallMnemonics(this.GetEncounterBgmImpl, out this.encounterReverseWrapper),
                    Utilities.PopCallerRegisters,
                    "cmp eax, -1",
                    "jng original",
                    "mov rdx, rax",
                    "mov r9, 0x1400bc6fd",
                    "jmp r9",
                    "label original",
                    "mov rax, r12",
                };

                this.encounterBgmHook = hooks.CreateAsmHook(encounterPatch, result).Activate();
            });

        ScanHooks.Add(
            "Victory BGM",
            "BA 07 00 00 00 E8 ?? ?? ?? ?? BA 07 00 00 00",
            (hooks, result) =>
            {
                var victoryPatch = new string[]
                {
                    "use64",
                    Utilities.PushCallerRegisters,
                    hooks.Utilities.GetAbsoluteCallMnemonics(this.GetVictoryBgmImpl, out this.victoryReverseWrapper),
                    Utilities.PopCallerRegisters,
                    "mov rdx, rax"
                };

                this.victoryBgmHook = hooks.CreateAsmHook(victoryPatch, result + 10, AsmHookBehaviour.ExecuteAfter).Activate();
            });
    }

    private int GetVictoryBgmImpl(int defaultMusicId)
    {
        var victoryMusicId = this.GetVictoryMusic();
        if (victoryMusicId != -1)
        {
            return victoryMusicId;
        }

        return defaultMusicId;
    }

    private int GetEncounterBgmImpl(nint encounterPtr, int encounterId)
    {
        var context = (EncounterContext)(*(ushort*)(encounterPtr + 0x1e));
        return this.GetBattleMusic(encounterId, context);
    }
}
