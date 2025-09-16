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

        [Category("Bustup")]
        [DisplayName("Dialogue Portrait")]
        [Description("Select your preferred dialogue bustups.")]
        [DefaultValue(BustupRV.Default)]
        [Display(Order = 0)]
        public BustupRV BustupValue { get; set; }

        [Category("Bustup")]
        [DisplayName("Epic Partypanel In Color")]
        [Description("Colorful bustup in battle. By Zrego and Wisteria.")]
        [DefaultValue(false)]
        [Display(Order = 1)]
        public bool ColorPartyPanelRV { get; set; } = false;

        [Category("Model")]
        [DisplayName("Black Leotard Overhaul")]
        [Description("Replaces the Black Leotard phantom suit with a recoloured Leotard.")]
        [DefaultValue(PhantomSuit.Default)]
        [Display(Order = 2)]
        public PhantomSuit PhantomSuitValue { get; set; }

        [Category("Model")]
        [DisplayName("No All-Out-Attack Portrait")]
        [Description("Removes the All-Out-Attack finisher art. By lyncpk.")]
        [DefaultValue(NoAOAportrait.Default)]
        [Display(Order = 3)]
        public NoAOAportrait AOAValue { get; set; }

        [Category("Model")]
        [DisplayName("Golden Rapiers")]
        [Description("For usage with the Phantom Suit Overhaul, disable if you want regular Rapiers or other weapon model mods to work.")]
        [DefaultValue(false)]
        [Display(Order = 4)]
        public bool GoldRapiers { get; set; } = false;

        [Category("Model")]
        [DisplayName("Blue Dress over Winter Casual (R&V)")]
        [Description("Replaces the player's winter casual outfit with the blue dress from Sumire's SL.")]
        [DefaultValue(false)]
        [Display(Order = 5)]
        public bool BlueDressRV { get; set; } = false;

        [Category("Model")]
        [DisplayName("Summer Casual outfit")]
        [Description("Select your preferred Summer casual outfit (default : polka dots shirt and shorts) Fuuka's dress by MyTamagos.")]
        [DefaultValue(SummerDressRV.Default)]
        [Display(Order = 6)]
        public SummerDressRV SummerDressValue { get; set; }

        [Category("Model")]
        [DisplayName("Lawson Outfit over 777 Outfit")]
        [Description("Replaces the 777 work outfit with the Lawson outfit from the December P5 Beta.")]
        [DefaultValue(false)]
        [Display(Order = 7)]
        public bool LawsonRV { get; set; } = false;

        [Category("Model")]
        [DisplayName("Concept art tracksuit for workout")]
        [Description("Replaces the workout outfit (red tank top) with the recolored tracksuit by MyTamagos based on concept art.")]
        [DefaultValue(false)]
        [Display(Order = 8)]
        public bool WorkoutRV { get; set; } = false;

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