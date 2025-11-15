using RVAltModels.Template.Configuration;
using Reloaded.Mod.Interfaces.Structs;
using System.ComponentModel;
using CriFs.V2.Hook;
using CriFs.V2.Hook.Interfaces;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace RVAltModels.Configuration
{
	public class Config : Configurable<Config>
	{
        /*
            User Properties:
                - Please put all of your configurable properties here.

            By default, configuration saves as "Config.json" in mod user config folder.    
            Need more config files/classes? See Configuration.cs

            Available Attributes:
            - Category
            - DisplayName
            - Description
            - DefaultValue

            // Technically Supported but not Useful
            - Browsable
            - Localizable

            The `DefaultValue` attribute is used as part of the `Reset` button in Reloaded-Launcher.
        */
        public enum PhantomSuit
        {
            [Display(Name = "Default")]
            Default,

            [Display(Name = "Pure White")]
            PureWhite,

            [Display(Name = "Red and White")]
            RedGold,
        }

        public enum SummerDressRV
        {
            [Display(Name = "Default")]
            Default,

            [Display(Name = "Fuuka's dress (Blue Ribbon)")]
            FuukaBlue,

            [Display(Name = "Fuuka's dress (White Ribbon)")]
            FuukaWhite,

            [Display(Name = "Eiko's casual outfit")]
            EikoCasualRV,

            [Display(Name = "Mitsuru's summer uniform")]
            MitsuruUniformCasual,
        }

        public enum SummerUniformRV
        {
            [Display(Name = "Default")]
            Default,

            [Display(Name = "Shujin's white shirt")]
            SummerUnifWhiteShirtRV,
        }

        public enum WinterUniformRV
        {
            [Display(Name = "Default")]
            Default,

            [Display(Name = "Shujin's turtleneck and suspenders")]
            WinterUnifTurtleSuspendRV,
        }

        public enum WinterCasualRV
        {
            [Display(Name = "Default")]
            Default,

            [Display(Name = "Blue dress")]
            BlueDressRV,

            [Display(Name = "Yukiko's winter casual")]
            YukikoWinterCasualRV,

            [Display(Name = "Yukiko's winter casual (Red ribbon)")]
            YukikoWinterCasualRedRV,

            [Display(Name = "Yukari's winter casual")]
            YukariWinterCasualRV,

        }

        public enum MidWinterCasualRV
        {
            [Display(Name = "Default")]
            Default,

            [Display(Name = "Rise's Midwinter casual")]
            RiseMidwinterCasualRV,

            [Display(Name = "Fancy fur coat casual")]
            FurCoatCasualRV,
        }
        
        public enum MidWinterUniformRV
        {
            [Display(Name = "Default")]
            Default,

            [Display(Name = "Rise's Midwinter coat")]
            RiseMidwinterUniformRV,

            [Display(Name = "Fancy fur coat uniform")]
            FurCoatUniformRV,
        }                       

        public enum BustupRV
        {
            [Display(Name = "Default")]
            Default,

            [Display(Name = "L7M3 (Version 1)")]
            L7M3RV,

            [Display(Name = "L7M3 (Version 2)")]
            L7M3V2RV,

            [Display(Name = "Legacy (neutral)")]
            LegacyV1,

            [Display(Name = "Legacy (smiling)")]
            LegacyV2,

        }

        public enum HeroTexRV
        {
            [Display(Name = "L7M3 (Default)")]
            Default,

            [Display(Name = "Neptune")]
            NeptuneRV,
        }

        public enum NoAOAportrait
        {
            [Display(Name = "Disabled")]
            Default,

            [Display(Name = "Enabled")]
            NoAOA,

            [Display(Name = "Enabled + Smug")]
            NoAOASmug,
        }

        public enum TracksuitRV
        {
            [Display(Name = "Red tank top (Default)")]
            Default,

            [Display(Name = "Black Tracksuit")]
            BlackTracksuit,

            [Display(Name = "Concept Art Tracksuit")]
            ConceptArtTracksuit,
        }

        [Category("Bustup")]
        [DisplayName("Dialogue Portrait")]
        [Description("Select your preferred dialogue bustups.")]
        [DefaultValue(BustupRV.Default)]
        [Display(Order = 0)]
        public BustupRV BustupValue { get; set; }

        [Category("Bustup")]
        [DisplayName("Menu artworks")]
        [Description("Select your preferred menu artworks.")]
        [DefaultValue(HeroTexRV.Default)]
        [Display(Order = 1)]
        public HeroTexRV HeroTexValue { get; set; }

        [Category("Bustup")]
        [DisplayName("Epic Partypanel In Color")]
        [Description("Colorful bustup in battle. By Zrego and Wisteria.")]
        [DefaultValue(false)]
        [Display(Order = 2)]
        public bool ColorPartyPanelRV { get; set; } = false;

        [Category("Model")]
        [DisplayName("Black Leotard Overhaul")]
        [Description("Replaces the Black Leotard phantom suit with a recoloured Leotard.")]
        [DefaultValue(PhantomSuit.Default)]
        [Display(Order = 3)]
        public PhantomSuit PhantomSuitValue { get; set; }

        [Category("Model")]
        [DisplayName("No All-Out-Attack Portrait")]
        [Description("Removes the All-Out-Attack finisher art. By lyncpk.")]
        [DefaultValue(NoAOAportrait.Default)]
        [Display(Order = 4)]
        public NoAOAportrait AOAValue { get; set; }

        [Category("Model")]
        [DisplayName("Golden Rapiers")]
        [Description("For usage with the Phantom Suit Overhaul, disable if you want regular Rapiers or other weapon model mods to work.")]
        [DefaultValue(false)]
        [Display(Order = 5)]
        public bool GoldRapiers { get; set; } = false;

        [Category("Model")]
        [DisplayName("Winter Casual outfit")]
        [Description("Select your preferred Winter Casual outfit. Blue dress from Sumire's SL. Yukiko's and Yukari's outfits by Mugikomachi.")]
        [DefaultValue(WinterCasualRV.Default)]
        [Display(Order = 6)]
        public WinterCasualRV WinterCasualValue { get; set; }

        [Category("Model")]
        [DisplayName("Summer Casual outfit")]
        [Description("Select your preferred Summer casual outfit (default : polka dots shirt and shorts) Fuuka's dress (MyTamagos), Eiko's outfit (Zylvos), Mitsuru's uniform (Bester).")]
        [DefaultValue(SummerDressRV.Default)]
        [Display(Order = 7)]
        public SummerDressRV SummerDressValue { get; set; }

        [Category("Model")]
        [DisplayName("Midwinter Casual outfit")]
        [Description("Select your preferred Midwinter Casual outfit. Rise's outfit (Mugikomachi). Fancy Fur Coat (Bester).")]
        [DefaultValue(MidWinterCasualRV.Default)]
        [Display(Order = 8)]
        public MidWinterCasualRV MidWinterCasualValue { get; set; }

        [Category("Model")]
        [DisplayName("Winter Uniform")]
        [Description("Select your preferred Winter uniform. Turtleneck with suspenders by Bester.")]
        [DefaultValue(WinterUniformRV.Default)]
        [Display(Order = 9)]
        public WinterUniformRV WinterUniformValue { get; set; }

        [Category("Model")]
        [DisplayName("Summer Uniform")]
        [Description("Select your preferred Summer uniform. White Shirt uniform by Zylvos.")]
        [DefaultValue(SummerUniformRV.Default)]
        [Display(Order = 10)]
        public SummerUniformRV SummerUniformValue { get; set; }

        [Category("Model")]
        [DisplayName("Midwinter Uniform")]
        [Description("Select your preferred Midwinter uniform. Rise's coat (Mugikomachi). Fancy Fur Coat (Bester).")]
        [DefaultValue(MidWinterUniformRV.Default)]
        [Display(Order = 11)]
        public MidWinterUniformRV MidWinterUniformValue { get; set; }

        [Category("Model")]
        [DisplayName("Workout outfit")]
        [Description("Choose your preferred workout outfit. Default, recolored Shujin tracksuit or Concept Art Tracksuit by MyTamagos.")]
        [DefaultValue(TracksuitRV.Default)]
        [Display(Order = 12)]
        public TracksuitRV TracksuitValue { get; set; }

        [Category("Model")]
        [DisplayName("Lawson Outfit over 777 Outfit")]
        [Description("Replaces the 777 work outfit with the Lawson outfit from the December P5 Beta.")]
        [DefaultValue(false)]
        [Display(Order = 13)]
        public bool LawsonRV { get; set; } = false;

    }

    /// <summary>
    /// Allows you to override certain aspects of the configuration creation process (e.g. create multiple configurations).
    /// Override elements in <see cref="ConfiguratorMixinBase"/> for finer control.
    /// </summary>
	public class ConfiguratorMixin : ConfiguratorMixinBase
	{
		// 
	}
}