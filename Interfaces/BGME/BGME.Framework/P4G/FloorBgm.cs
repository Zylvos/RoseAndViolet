using BGME.Framework.Music;
using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.Enums;
using Reloaded.Hooks.Definitions.X64;
using static Reloaded.Hooks.Definitions.X64.FunctionAttribute;

namespace BGME.Framework.P4G;

internal class FloorBgm : BaseFloorBgm
{
    [Function(Register.rdi, Register.rax, true)]
    private delegate int GetFloorBgmFunction(int floorId);
    private IReverseWrapper<GetFloorBgmFunction>? floorReverseWrapper;
    private IAsmHook? floorBgmHook;

    public FloorBgm(MusicService music) 
        : base(music)
    {
        ScanHooks.Add(
            "Floor BGM",
            "83 FF 05 0F 8E C1 00 00 00",
            (hooks, result) =>
            {
                // TODO: Don't hardcode address!
                var patch = new string[]
                {
                    "use64",
                    Utilities.PushCallerRegisters,
                    hooks.Utilities.GetAbsoluteCallMnemonics(this.GetFloorBgm, out this.floorReverseWrapper),
                    Utilities.PopCallerRegisters,
                    "cmp eax, -1",
                    "jng original",
                    "mov ebx, eax",
                    "mov r9, 0x14031525D",
                    "jmp r9",
                    "label original",
                };

                this.floorBgmHook = hooks.CreateAsmHook(patch, result, AsmHookBehaviour.ExecuteFirst).Activate();
            });
    }
}
