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
            [Display(Name = "Black (Default)")]
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

            [Display(Name = "Kotomo's Shujin uniform")]
            KotomoUniformRV,            
        }

        public enum WinterUniformRV
        {
            [Display(Name = "Default")]
            Default,

            [Display(Name = "Shujin's turtleneck and suspenders")]
            WinterUnifTurtleSuspendRV,

            [Display(Name = "Kotomo's beige Shujin uniform")]
            WinterUnifKotomoBeigeRV,
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

            [Display(Name = "Comfy Hoodie")]
            ComfyHoodieRV,

        }

        public enum IncognitoRV
        {
            [Display(Name = "Default")]
            Default,

            [Display(Name = "Default winter casual")]
            WinterCasualIncognitoRV,

            [Display(Name = "Blue dress")]
            BlueDressIncognitoRV,

            [Display(Name = "Yukiko's winter casual")]
            YukikoCasualIncognitoRV,

            [Display(Name = "Yukari's winter casual")]
            YukariCasualIncognitoRV,

            [Display(Name = "Comfy Hoodie")]
            ComfyHoodieIncognitoRV,

        }

        public enum MidWinterCasualRV
        {
            [Display(Name = "Default")]
            Default,

            [Display(Name = "Rise's Midwinter casual")]
            RiseMidwinterCasualRV,

            [Display(Name = "Fancy fur coat casual")]
            FurCoatCasualRV,

            [Display(Name = "Micaiah's Midwinter casual")]
            MicaiahMWCasualRV,
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

        public enum PajamasRV
        {
            [Display(Name = "Default")]
            Default,

            [Display(Name = "Star pajamas")]
            StarPajamasRV,
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

        public enum PTBustupRV
        {
            [Display(Name = "Default")]
            Default,

            [Display(Name = "L7M3 (Version 1)")]
            PTL7M3RV,

            [Display(Name = "L7M3 (Version 2)")]
            PTL7M3V2RV,

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
            [Display(Name = "Default")]
            Default,

            [Display(Name = "Black Tracksuit")]
            BlackTracksuit,

            [Display(Name = "Concept Art Tracksuit")]
            ConceptArtTracksuit,

            [Display(Name = "Tamayo's Gym Outfit")]
            TamayoGym,            
        }

        public enum StudentIDRV
        {
            [Display(Name = "Default")]
            Default,

            [Display(Name = "Ponytail with ribbon")]
            PonytailRibbon,

            [Display(Name = "Ponytail with hair tie")]
            PonytailHairTie,

            [Display(Name = "Hair down")]
            PonytailHairDown,            
        }

        public enum CutinRV
        {
            [Display(Name = "Default")]
            Default,

            [Display(Name = "L7M3")]
            CutinL7M3,
        }

        public enum AOAShardRV
        {
            [Display(Name = "Default")]
            Default,

            [Display(Name = "L7M3")]
            AOAShardL7M3,
        }

        [Category("Bustups")]
        [DisplayName("Overworld dialogue portrait")]
        [Description("Select your preferred overworld dialogue bustups.")]
        [DefaultValue(BustupRV.Default)]
        [Display(Order = 0)]
        public BustupRV BustupValue { get; set; }

        [Category("Bustups")]
        [DisplayName("Phantom Thief dialogue portrait")]
        [Description("Select your preferred Phantom Thief dialogue bustups.")]
        [DefaultValue(PTBustupRV.Default)]
        [Display(Order = 1)]
        public PTBustupRV PTBustupValue { get; set; }

        [Category("Bustups")]
        [DisplayName("Student ID")]
        [Description("Choose the photo displayed on the student ID. By shadows0.")]
        [DefaultValue(StudentIDRV.Default)]
        [Display(Order = 2)]
        public StudentIDRV StudentIDValue { get; set; }

        [Category("Bustups")]
        [DisplayName("AOA Shard")]
        [Description("Select your preferred AOA shard.")]
        [DefaultValue(AOAShardRV.Default)]
        [Display(Order = 3)]
        public AOAShardRV AOAShardRVValue { get; set; }

        [Category("Bustups")]
        [DisplayName("Battle cutin")]
        [Description("Select your preferred battle cutin.")]
        [DefaultValue(CutinRV.Default)]
        [Display(Order = 4)]
        public CutinRV CutinRVValue { get; set; }

        [Category("Bustups")]
        [DisplayName("Menu artworks")]
        [Description("Select your preferred menu artworks.")]
        [DefaultValue(HeroTexRV.Default)]
        [Display(Order = 5)]
        public HeroTexRV HeroTexValue { get; set; }

        [Category("Bustups")]
        [DisplayName("Epic Partypanel In Color")]
        [Description("Colorful bustup in battle. By Zrego and Wisteria.")]
        [DefaultValue(false)]
        [Display(Order = 6)]
        public bool ColorPartyPanelRV { get; set; } = false;

        [Category("Metaverse models")]
        [DisplayName("Phantom Thief outfit")]
        [Description("Select your preferred color for the Phantom Thief outfit. Bustups and AOA finisher will be changed accordingly.")]
        [DefaultValue(PhantomSuit.Default)]
        [Display(Order = 7)]
        public PhantomSuit PhantomSuitValue { get; set; }

        [Category("Metaverse models")]
        [DisplayName("No All-Out-Attack Portrait")]
        [Description("Removes the All-Out-Attack finisher art. By lyncpk.")]
        [DefaultValue(NoAOAportrait.Default)]
        [Display(Order = 8)]
        public NoAOAportrait AOAValue { get; set; }

        [Category("Metaverse models")]
        [DisplayName("Golden Rapiers")]
        [Description("For usage with the Phantom Suit Overhaul, disable if you want regular Rapiers or other weapon model mods to work.")]
        [DefaultValue(false)]
        [Display(Order = 9)]
        public bool GoldRapiers { get; set; } = false;

        [Category("Overworld outfits")]
        [DisplayName("Winter Casual outfit")]
        [Description("Select your preferred Winter Casual outfit.")]
        [DefaultValue(WinterCasualRV.Default)]
        [Display(Order = 10)]
        public WinterCasualRV WinterCasualValue { get; set; }

        [Category("Overworld outfits")]
        [DisplayName("Summer Casual outfit")]
        [Description("Select your preferred Summer casual outfit.")]
        [DefaultValue(SummerDressRV.Default)]
        [Display(Order = 11)]
        public SummerDressRV SummerDressValue { get; set; }

        [Category("Overworld outfits")]
        [DisplayName("Midwinter Casual outfit")]
        [Description("Select your preferred Midwinter Casual outfit.")]
        [DefaultValue(MidWinterCasualRV.Default)]
        [Display(Order = 12)]
        public MidWinterCasualRV MidWinterCasualValue { get; set; }

        [Category("Overworld outfits")]
        [DisplayName("Winter Uniform")]
        [Description("Select your preferred Winter uniform.")]
        [DefaultValue(WinterUniformRV.Default)]
        [Display(Order = 13)]
        public WinterUniformRV WinterUniformValue { get; set; }

        [Category("Overworld outfits")]
        [DisplayName("Summer Uniform")]
        [Description("Select your preferred Summer uniform.")]
        [DefaultValue(SummerUniformRV.Default)]
        [Display(Order = 14)]
        public SummerUniformRV SummerUniformValue { get; set; }

        [Category("Overworld outfits")]
        [DisplayName("Midwinter Uniform")]
        [Description("Select your preferred Midwinter uniform.")]
        [DefaultValue(MidWinterUniformRV.Default)]
        [Display(Order = 15)]
        public MidWinterUniformRV MidWinterUniformValue { get; set; }

        [Category("Overworld outfits")]
        [DisplayName("Incognito outfit")]
        [Description("Select your preferred incognito outfit.")]
        [DefaultValue(IncognitoRV.Default)]
        [Display(Order = 16)]
        public IncognitoRV IncognitoValue { get; set; }

        [Category("Overworld outfits")]
        [DisplayName("Workout outfit")]
        [Description("Choose your preferred workout outfit.")]
        [DefaultValue(TracksuitRV.Default)]
        [Display(Order = 17)]
        public TracksuitRV TracksuitValue { get; set; }

        [Category("Overworld outfits")]
        [DisplayName("Loungewear")]
        [Description("Choose your preferred night time outfit.")]
        [DefaultValue(PajamasRV.Default)]
        [Display(Order = 18)]
        public PajamasRV PajamasValue { get; set; }

        [Category("Overworld outfits")]
        [DisplayName("Lawson Outfit over 777 Outfit")]
        [Description("Replaces the 777 work outfit with the Lawson outfit from the December P5 Beta.")]
        [DefaultValue(false)]
        [Display(Order = 19)]
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