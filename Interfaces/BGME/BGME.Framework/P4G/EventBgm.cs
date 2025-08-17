using BGME.Framework.Music;
using LibellusLibrary.Event.Types.Frame;
using PersonaMusicScript.Types.Music;
using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.X64;
using System.Runtime.InteropServices;

namespace BGME.Framework.P4G;

/// <summary>
/// Based on p4g64.EventLogger by Secre-C.
/// </summary>
internal unsafe class EventBgm
{
    [Function(CallingConventions.Microsoft)]
    private delegate void* RunCommandFunction(nint commandPtr, nint param2);
    private IHook<RunCommandFunction>? runCommandHook;
    private IFunction<RunCommandFunction>? runCommandFunction;

    [Function(CallingConventions.Microsoft)]
    private delegate void RunEventCommandsFunction(int frame, nint param2, int pass, nint param4);
    private IHook<RunEventCommandsFunction>? runEventCommandsHook;

    private IAsmHook? setCurrentEventHook;

    private readonly BgmService sound;
    private readonly MusicService music;

    private readonly int* currentMajorId = (int*)NativeMemory.AllocZeroed(sizeof(int));
    private readonly int* currentMinorId = (int*)NativeMemory.AllocZeroed(sizeof(int));
    private int currentFrame = -1;

    public EventBgm(BgmService sound, MusicService music)
    {
        this.sound = sound;
        this.music = music;

        ScanHooks.Add(
            nameof(RunEventCommandsFunction),
            "48 89 6C 24 ?? 56 57 41 56 48 83 EC 30 89 CD",
            (hooks, result) => this.runEventCommandsHook = hooks.CreateHook<RunEventCommandsFunction>(this.RunFrameCommands, result).Activate());

        ScanHooks.Add(
            nameof(RunCommandFunction),
            "40 53 48 83 EC 20 48 8B D9 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 0F BE 53 ??",
            (hooks, result) => this.runCommandHook = hooks.CreateHook<RunCommandFunction>(this.RunCommand, result).Activate());

        ScanHooks.Add(
            "Event PMD File Path",
            "F7 EE 89 7C 24 20",
            (hooks, result) =>
            {
                var patch = new string[]
                {
                    "use64",
                    $"push rcx",
                    $"mov rcx, {(nint)this.currentMajorId}",
                    $"mov [rcx], rsi",
                    $"mov rcx, {(nint)this.currentMinorId}",
                    $"mov [rcx], rdi",
                    $"pop rcx"
                };

                this.setCurrentEventHook = hooks.CreateAsmHook(patch, result, Reloaded.Hooks.Definitions.Enums.AsmHookBehaviour.ExecuteFirst).Activate();
            });
    }

    private void RunFrameCommands(int frame, nint param2, int pass, nint param4)
    {
        this.currentFrame = frame;

        // Log current event IDs.
        if (this.currentFrame == 0 && pass == 1)
        {
            Log.Debug($"Event || Major ID: {*this.currentMajorId} || Minor ID: {*this.currentMinorId}");
        }

        // BGM added through script.
        // Only check on first pass(?)
        if (pass == 1
            && this.music.GetEventFrame(*this.currentMajorId, *this.currentMinorId, PmdType.PM3) != null)
        {
            Log.Debug($"Frame: {this.currentFrame}");
        }

        this.runEventCommandsHook?.OriginalFunction(frame, param2, pass, param4);
    }

    private void* RunCommand(nint commandPtr, nint param2)
    {
        int commandTypeId = *(int*)(commandPtr + 0x38);
        PmdTargetTypeID commandType = (PmdTargetTypeID)commandTypeId;

        int commandId = *(ushort*)commandPtr;
        if (this.currentFrame == commandId)
        {
            Log.Verbose($"Frame: {commandId} || Command: {commandType} || Address: {commandPtr:X} || param2: {param2:X}");
        }

        return this.runCommandHook!.OriginalFunction(commandPtr, param2);
    }
}
