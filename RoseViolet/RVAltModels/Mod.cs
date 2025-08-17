﻿using RVAltModels.Configuration;
using RVAltModels.Template;
using Reloaded.Hooks.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;
using CriFs.V2.Hook.Interfaces;
using SPD.File.Emulator.Interfaces;
using P5R.CostumeFramework.Interfaces;
using CriExtensions;

namespace RVAltModels
{
    public class Mod : ModBase
    {
        private readonly IModLoader _modLoader;
        private readonly IReloadedHooks? _hooks;
        private readonly ILogger _logger;
        private readonly IMod _owner;
        private Config _configuration;
        private readonly IModConfig _modConfig;

        public Mod(ModContext context)
        {
            _modLoader = context.ModLoader;
            _hooks = context.Hooks;
            _logger = context.Logger;
            _owner = context.Owner;
            _configuration = context.Configuration;
            _modConfig = context.ModConfig;

            string modDir = _modLoader.GetDirectoryForModId(_modConfig.ModId);
            string modId = _modConfig.ModId;

            // Initialize file emulator controllers
            var criFsCtl = _modLoader.GetController<ICriFsRedirectorApi>();
            var costumeCtl = _modLoader.GetController<ICostumeApi>();
            var spdEmuCtl = _modLoader.GetController<ISpdEmulator>();

            if (criFsCtl == null || !criFsCtl.TryGetTarget(out var criFsApi)) { _logger.WriteLine("CRI FS missing → cpk and binds broken.", System.Drawing.Color.Red); return; }
            if (costumeCtl == null || !costumeCtl.TryGetTarget(out var costumeApi)) { _logger.WriteLine("Costume API missing → Costumes broken.", System.Drawing.Color.Red); return; }
            if (spdEmuCtl == null || !spdEmuCtl.TryGetTarget(out var spdEmu)) { _logger.WriteLine("SPD Emu missing → SPD merges broken.", System.Drawing.Color.Red); return; }

            // Leotard Overhaul (Pure White or Red and Gold)
            if (_configuration.PhantomSuitValue == Config.PhantomSuit.PureWhite || _configuration.PhantomSuitValue == Config.PhantomSuit.RedGold)
            {
                string suitFolder = _configuration.PhantomSuitValue == Config.PhantomSuit.PureWhite ? "PureWhite" : "RedAndGold";
                costumeApi.AddCostumesFolder(modDir, Path.Combine(modDir, "OptionalModFiles", suitFolder, "CF"));
                BindAllFilesIn(Path.Combine("OptionalModFiles", suitFolder, "Bind"), modDir, criFsApi, modId);
            }

            // Gold Rapiers
            if (_configuration.GoldRapiers)
                BindAllFilesIn(Path.Combine("OptionalModFiles", "GoldenRapiers"), modDir, criFsApi, modId);

            // Blue Dress
            if (_configuration.BlueDressRV)
                BindAllFilesIn(Path.Combine("OptionalModFiles", "BlueDress"), modDir, criFsApi, modId);

            // Bustup
            if (_configuration.BustupValue == Config.BustupRV.L7M3RV)
                BindAllFilesIn(Path.Combine("OptionalModFiles", "Bustup", "L7M3"), modDir, criFsApi, modId);

            // EPIC colorful party panel
            if (_configuration.ColorPartyPanelRV)
                spdEmu.AddDirectory(Path.Combine(modDir, "OptionalModFiles", "EPICPartyPanel", "SPD"));

            // Menu artworks
            if (_configuration.HeroTexValue == Config.HeroTexRV.NeptuneRV)
                BindAllFilesIn(Path.Combine("OptionalModFiles", "Menu", "Neptune"), modDir, criFsApi, modId);

            // No AOA portrait
            if (_configuration.AOAValue == Config.NoAOAportrait.NoAOA || _configuration.AOAValue == Config.NoAOAportrait.NoAOASmug)
            {
                string AOAFolder = _configuration.AOAValue == Config.NoAOAportrait.NoAOA ? "NoAoAPortrait" : "SmugAoA";
                BindAllFilesIn(Path.Combine("OptionalModFiles", AOAFolder), modDir, criFsApi, modId);
            }
        }

        private static void BindAllFilesIn(string subPathRelativeToModDir, string modDir, ICriFsRedirectorApi criFsApi, string modId)
        {
            var absoluteFolder = Path.Combine(modDir, subPathRelativeToModDir);
            if (!Directory.Exists(absoluteFolder)) return;
            foreach (var file in Directory.EnumerateFiles(absoluteFolder, "*", SearchOption.AllDirectories))
                criFsApi.AddBind(file, Path.GetRelativePath(absoluteFolder, file).Replace(Path.DirectorySeparatorChar, '/'), modId);
        }

        public override void ConfigurationUpdated(Config configuration)
        {
            _configuration = configuration;
            _logger.WriteLine($"[{_modConfig.ModId}] Config Updated: Applying");
        }

#pragma warning disable CS8618
        public Mod() { }
#pragma warning restore CS8618
    }
}
