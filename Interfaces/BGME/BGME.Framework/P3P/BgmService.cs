using BGME.Framework.Music;
using PersonaMusicScript.Types.Music;
using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.Enums;
using Reloaded.Hooks.Definitions.X64;
using Ryo.Definitions.Structs;
using Ryo.Interfaces;
using SharpCompress.Common;
using System.Runtime.InteropServices;
using System.Text;
using YamlDotNet.Core.Tokens;
using static Reloaded.Hooks.Definitions.X64.FunctionAttribute;

namespace BGME.Framework.P3P;

/// <summary>
/// Patch BGM function to play any BGM audio file.
/// </summary>
internal unsafe class BgmService : BaseBgm
{
    private const int MAX_STRING_SIZE = 16;

    [Function(Register.rax, Register.rax, true)]
    private delegate byte* GetBgmString(int bgmId);
    private IReverseWrapper<GetBgmString>? bgmReverseWrapper;
    private IAsmHook? bgmHook;
    private IAsmHook? fixBgmCrashHook;

    private delegate void PlayComseOrBse(SE_TYPE seType, int param2, ushort majorId, short minorId);
    private IHook<PlayComseOrBse>? playComseOrBseHook;

    private delegate void criAtomExPlayer_SetVolume(nint player, float volume);
    private IHook<criAtomExPlayer_SetVolume>? criAtomExPlayer_SetVolumeHook;
    private float sfxVolume;

    private string? sfxFilePath;

    private delegate void PlaySePCM(int soundId);
    private IHook<PlaySePCM>? PlaySePCMHook;

    private delegate int FUN_1587a9410(short param_1, int param_2, nint* param_3);
    private IHook<FUN_1587a9410>? _FUN_1587a9410Hook;
    private int FUN_1587a9410Impl(short param_1, int param_2, nint* param_3)
    {
        //Log.Debug($"FUN_1587a9410 param_1, param_2: {param_1}, {param_2}");
        this.sfxFilePath = null;
        return _FUN_1587a9410Hook!.OriginalFunction(param_1, param_2, param_3);
    }

    private readonly IRyoApi ryo;
    private readonly ICriAtomEx criAtomEx;
    private readonly ICriAtomRegistry criAtomRegistry;
    private readonly byte* bgmStringBuffer;

    public BgmService(
        IRyoApi ryo,
        ICriAtomEx criAtomEx,
        ICriAtomRegistry criAtomRegistry,
        MusicService music)
        : base(music)
    {
        this.ryo = ryo;
        this.criAtomEx = criAtomEx;
        this.criAtomRegistry = criAtomRegistry;
        this.bgmStringBuffer = (byte*)NativeMemory.AllocZeroed(MAX_STRING_SIZE, sizeof(byte));

        ScanHooks.Add(
            "BGM Patch",
            "4E 8B 84 ?? ?? ?? ?? ?? E8 ?? ?? ?? ?? 8B 0D",
            (hooks, result) =>
            {
                var bgmPatch = new string[]
                {
                    "use64",
                    Utilities.PushCallerRegisters,
                    hooks.Utilities.GetAbsoluteCallMnemonics(this.GetBgmStringImpl, out this.bgmReverseWrapper),
                    Utilities.PopCallerRegisters,
                    $"mov r8, rax",
                };

                this.bgmHook = hooks.CreateAsmHook(bgmPatch, result, AsmHookBehaviour.DoNotExecuteOriginal).Activate();
            });

        ScanHooks.Add(
            "Fix BGM Crashes",
            "8B 0D ?? ?? ?? ?? BB 01 00 00 00 89 DA E8 ?? ?? ?? ?? 66 44 8B 05",
            (hooks, result) => this.fixBgmCrashHook = hooks.CreateAsmHook(new[] { "use64", "xor ecx, ecx" }, result + 0x7C, AsmHookBehaviour.DoNotExecuteOriginal).Activate());

        ScanHooks.Add(
            nameof(PlayComseOrBse),
            "40 53 55 56 57 41 54 41 57 48 81 EC B8 00 00 00",
            (hooks, result) => this.playComseOrBseHook = hooks.CreateHook<PlayComseOrBse>(this.PlayComseOrBseImpl, result).Activate());

        ScanHooks.Add(
            nameof(criAtomExPlayer_SetVolume),
            "48 85 C9 75 ?? 44 8D 41 ?? 48 8D 15 ?? ?? ?? ?? E9 ?? ?? ?? ?? 48 8B 89 ?? ?? ?? ?? 0F 28 D1 33 D2",
            (hooks, result) => this.criAtomExPlayer_SetVolumeHook = hooks.CreateHook<criAtomExPlayer_SetVolume>(this.criAtomExPlayer_SetVolumeImpl, result).Activate());

        ScanHooks.Add(
            nameof(PlaySePCM),
            "48 89 5C 24 ?? 57 48 83 EC 30 48 63 D9 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? FF 15 ?? ?? ?? ?? 8B D0 4C 8D 05 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 8B 0D ?? ?? ?? ?? 48 8D 3D ?? ?? ?? ?? 48 C1 E3 05 41 B9 01 00 00 00 C7 44 24 ?? 44 AC 00 00",
            (hooks, result) => this.PlaySePCMHook = hooks.CreateHook<PlaySePCM>(this.PlaySePCMImpl, result).Activate());

        ScanHooks.Add(
            nameof(FUN_1587a9410),
            "48 89 5C 24 ?? 48 89 6C 24 ?? 48 89 74 24 ?? 66 89 4C 24",
            (hooks, result) => this._FUN_1587a9410Hook = hooks.CreateHook<FUN_1587a9410>(this.FUN_1587a9410Impl, result).Activate());
    }

    private nint _customSePlayer = IntPtr.Zero;

    private nint CustomeSePlayer
    {
        get
        {
            if (this._customSePlayer == IntPtr.Zero)
            {
                var config = (CriAtomExPlayerConfigTag*)Marshal.AllocHGlobal(Marshal.SizeOf<CriAtomExPlayerConfigTag>());
                config->maxPathStrings = 8;
                config->maxPath = 256;
                config->voiceAllocationMethod = 1;

                this._customSePlayer = this.criAtomEx.Player_Create(config, (void*)0, 0);
            }

            return this._customSePlayer;
        }
    }

    /// <summary>
    /// Play COMSE or BSE SFX.
    /// </summary>
    /// <param name="seType">SE type: 0 = COMSE, 1 = BSE</param>
    /// <param name="param2">Unknown/unused?</param>
    /// <param name="majorId">First two digits of file.</param>
    /// <param name="minorId">Last two digits of file.</param>
    private void PlayComseOrBseImpl(SE_TYPE seType, int param2, ushort majorId, short minorId)
    {
        //Log.Debug($"seType, param2, majorId, minorId: {seType}, {param2}, {majorId}, {minorId}");
        var seFilePath = $"sound/se/{(seType == SE_TYPE.COMSE ? "comse.pak/" : "bse.pak/b")}{majorId:00}{minorId:00}.vag";
        if (Enum.IsDefined(seType) && this.ryo.HasFileContainer(seFilePath))
        {
            Log.Debug($"{nameof(PlayComseOrBse)}: {seFilePath}");
            this.sfxFilePath = seFilePath;
        }
        else
        {
            this.sfxFilePath = null;
            if (Enum.IsDefined(seType) == false)
            {
                Log.Debug($"Unknown SE value: {seType}");
            }
        }
        this.playComseOrBseHook!.OriginalFunction(seType, param2, majorId, minorId);
    }

    /// <summary>
    /// Control playback volume for COMSE or BSE SFX.
    /// </summary>
    /// <param name="player">Player used to play the sound category. 0 = SE, 1 = Music, 2 = Voice</param>
    /// <param name="volume">Controls the volume of the sound played</param>
    private void criAtomExPlayer_SetVolumeImpl(nint player, float volume)
    {
        var currentplayer = criAtomRegistry.GetPlayerByHn(player);
        if (currentplayer!.Id == 0)
        {
            this.criAtomExPlayer_SetVolumeImpl(CustomeSePlayer, volume);
            this.sfxVolume = volume;
            //Log.Debug($"criAtomExPlayer Player {currentplayer.Id}: {volume}");
        }
        this.criAtomExPlayer_SetVolumeHook!.OriginalFunction(player, volume);
    }

    /// <summary>
    /// Play PCM SFX.
    /// </summary>
    /// <param name="soundId">Sound ID.</param>
    private void PlaySePCMImpl(int soundId)
    {
        //Log.Debug($"PlaySePCM soundId: {soundId}");
        if (this.sfxFilePath != null)
        {
            this.criAtomEx.Player_SetFile(this.CustomeSePlayer, 0, (byte*)StringsCache.GetStringPtr(sfxFilePath));
            this.criAtomEx.Player_Start(this.CustomeSePlayer);
        }
        else
        {
            this.PlaySePCMHook!.OriginalFunction(soundId);
        }
    }

    protected override int VictoryBgmId { get; } = 60;

    protected override void PlayBgm(int bgmId)
    {
        Log.Debug("Play BGM not supported.");
    }

    private byte* GetBgmStringImpl(int bgmId)
    {
        // Handle Mass Destruction being played
        // through ID 2 for some reason.
        int? currentBgmId = bgmId == 2 ? 26 : bgmId;
        currentBgmId = this.GetGlobalBgmId((int)currentBgmId);

        if (currentBgmId == null)
        {
            Log.Warning("Music disabling not supported.");
            currentBgmId = 1;
        }

        var bgmString = string.Format("{0:00}.ADX\0", (int)currentBgmId);
        if (bgmString.Length > MAX_STRING_SIZE)
        {
            bgmString = "01.ADX\0";
            Log.Error($"BGM value too large. Value: {bgmId}");
        }

        if (bgmId < 1)
        {
            bgmString = "01.ADX\0";
            Log.Error("Negative BGM value, previous file probably does not exist.");
        }

        var bgmStringBytes = Encoding.ASCII.GetBytes(bgmString);

        var handle = GCHandle.Alloc(bgmStringBytes, GCHandleType.Pinned);
        NativeMemory.Copy((void*)handle.AddrOfPinnedObject(), this.bgmStringBuffer, (nuint)bgmStringBytes.Length);
        handle.Free();

        Log.Debug($"Playing BGM: {bgmString.Trim('\0')}");
        return this.bgmStringBuffer;
    }

    private enum SE_TYPE
        : short
    {
        COMSE,
        BSE,
    }
}
