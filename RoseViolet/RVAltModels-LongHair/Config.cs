using RVAltModelsLongHair.Template.Configuration;
using Reloaded.Mod.Interfaces.Structs;
using System.ComponentModel;
using CriFs.V2.Hook;
using CriFs.V2.Hook.Interfaces;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace RVAltModelsLongHair.Configuration
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
        }

        public enum WinterCasualRV
        {
            [Display(Name = "Default")]
            Default,

            [Display(Name = "Blue dress")]
            BlueDressRV,

            [Display(Name = "Yukiko's winter casual")]
            YukikoWinterCasualRV,

            [Display(Name = "Yukari's winter casual")]
            YukariWinterCasualRV,

            [Display(Name = "Yukari's winter casual (Black ribbon)")]
            YukariWinterCasualBlackRV,            

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

            [Display(Name = "Legacy (neutral)")]
            LegacyV1,

            [Display(Name = "Legacy (smiling)")]
            LegacyV2,
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

            [Display(Name = "Tamayo's Gym Outfit")]
            TamayoGym, 
        }

        public enum AOACutinRV
        {
            [Display(Name = "Default")]
            Default,

            [Display(Name = "Johesy")]
            Johesy,
        }        

        [Category("Bustups")]
        [DisplayName("Dialogue Portrait")]
        [Description("Select your preferred dialogue bustups.")]
        [DefaultValue(BustupRV.Default)]
        [Display(Order = 0)]
        public BustupRV BustupValue { get; set; }

        [Category("Bustups")]
        [DisplayName("Epic Partypanel In Color")]
        [Description("Colorful bustup in battle. By Zrego and Wisteria.")]
        [DefaultValue(false)]
        [Display(Order = 1)]
        public bool ColorPartyPanelRV { get; set; } = false;

        [Category("Bustups")]
        [DisplayName("AOA Cutin")]
        [Description("Choose your prefered AOA. Default by MyTamagos.")]
        [DefaultValue(AOACutinRV.Default)]
        [Display(Order = 2)]
        public AOACutinRV AOACutinValue { get; set; }

        [Category("Metaverse models")]
        [DisplayName("Phantom Thief outfit")]
        [Description("Select your preferred color for the Phantom Thief outfit. Bustups and AOA finisher will be changed accordingly.")]
        [DefaultValue(PhantomSuit.Default)]
        [Display(Order = 3)]
        public PhantomSuit PhantomSuitValue { get; set; }

        [Category("Metaverse models")]
        [DisplayName("No All-Out-Attack Portrait")]
        [Description("Removes the All-Out-Attack finisher art. By lyncpk.")]
        [DefaultValue(NoAOAportrait.Default)]
        [Display(Order = 4)]
        public NoAOAportrait AOAValue { get; set; }

        [Category("Metaverse models")]
        [DisplayName("Golden Rapiers")]
        [Description("For usage with the Phantom Suit Overhaul, disable if you want regular Rapiers or other weapon model mods to work.")]
        [DefaultValue(false)]
        [Display(Order = 5)]
        public bool GoldRapiers { get; set; } = false;

        [Category("Overworld outfits")]
        [DisplayName("Winter Casual outfit")]
        [Description("Select your preferred Winter Casual outfit.")]
        [DefaultValue(WinterCasualRV.Default)]
        [Display(Order = 6)]
        public WinterCasualRV WinterCasualValue { get; set; }

        [Category("Overworld outfits")]
        [DisplayName("Summer Casual outfit")]
        [Description("Select your preferred Summer casual outfit.")]
        [DefaultValue(SummerDressRV.Default)]
        [Display(Order = 7)]
        public SummerDressRV SummerDressValue { get; set; }

        [Category("Overworld outfits")]
        [DisplayName("Midwinter Casual outfit")]
        [Description("Select your preferred Midwinter Casual outfit.")]
        [DefaultValue(MidWinterCasualRV.Default)]
        [Display(Order = 8)]
        public MidWinterCasualRV MidWinterCasualValue { get; set; }

        [Category("Overworld outfits")]
        [DisplayName("Winter Uniform")]
        [Description("Select your preferred Winter uniform.")]
        [DefaultValue(WinterUniformRV.Default)]
        [Display(Order = 9)]
        public WinterUniformRV WinterUniformValue { get; set; }

        [Category("Overworld outfits")]
        [DisplayName("Summer Uniform")]
        [Description("Select your preferred Summer uniform.")]
        [DefaultValue(SummerUniformRV.Default)]
        [Display(Order = 10)]
        public SummerUniformRV SummerUniformValue { get; set; }

        [Category("Overworld outfits")]
        [DisplayName("Midwinter Uniform")]
        [Description("Select your preferred Midwinter uniform.")]
        [DefaultValue(MidWinterUniformRV.Default)]
        [Display(Order = 11)]
        public MidWinterUniformRV MidWinterUniformValue { get; set; }

        [Category("Overworld outfits")]
        [DisplayName("Incognito outfit")]
        [Description("Select your preferred incognito outfit.")]
        [DefaultValue(IncognitoRV.Default)]
        [Display(Order = 12)]
        public IncognitoRV IncognitoValue { get; set; }

        [Category("Overworld outfits")]
        [DisplayName("Workout outfit")]
        [Description("Choose your preferred workout outfit.")]
        [DefaultValue(TracksuitRV.Default)]
        [Display(Order = 13)]
        public TracksuitRV TracksuitValue { get; set; }

        [Category("Overworld outfits")]
        [DisplayName("Loungewear")]
        [Description("Choose your preferred night time outfit.")]
        [DefaultValue(PajamasRV.Default)]
        [Display(Order = 14)]
        public PajamasRV PajamasValue { get; set; }

        [Category("Overworld outfits")]
        [DisplayName("Lawson Outfit over 777 Outfit")]
        [Description("Replaces the 777 work outfit with the Lawson outfit from the December P5 Beta.")]
        [DefaultValue(false)]
        [Display(Order = 15)]
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