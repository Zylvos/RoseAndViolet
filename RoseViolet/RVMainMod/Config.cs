using RVMainMod.Template.Configuration;
using Reloaded.Mod.Interfaces.Structs;
using System.ComponentModel;
using CriFs.V2.Hook;
using CriFs.V2.Hook.Interfaces;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace RVMainMod.Configuration
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

        [Category("Cheat Sheets")]
        [DisplayName("Classroom Cheat Sheet")]
        [Description("Shows the correct answers for classroom questions. Disable any other standalone cheat sheet mods.")]
        [DefaultValue(false)]
        [Display(Order = 1)]
        public bool ClassroomCheat { get; set; } = false;

        [Category("Cheat Sheets")]
        [DisplayName("Confidant Cheat Sheet")]
        [Description("Shows how many points you get for each confidant option. Disable any other standalone cheat sheet mods.")]
        [DefaultValue(false)]
        [Display(Order = 2)]
        public bool ConfidantCheat { get; set; } = false;

        [Category("Cheat Sheets")]
        [DisplayName("Crosswords Cheat Sheet")]
        [Description("Shows the answer in the crosswords questions. Disable any other standalone cheat sheet mods.")]
        [DefaultValue(false)]
        [Display(Order = 3)]
        public bool CrosswordsCheat { get; set; } = false;

        [Category("Cheat Sheets")]
        [DisplayName("TV Quiz Cheat Sheet")]
        [Description("Shows the correct answers to the TV quizzes. Disable any other standalone cheat sheet mods.")]
        [DefaultValue(false)]
        [Display(Order = 4)]
        public bool TVQuizCheat { get; set; } = false;

        [Category("Cheat Sheets")]
        [DisplayName("Correct Darts Options")]
        [Description("Highlights the dialogue options to have party members get the 0 score at darts. Disable any other standalone cheat sheet mods.")]
        [DefaultValue(false)]
        [Display(Order = 5)]
        public bool DartsCheat { get; set; } = false;

        [Category("Gameplay")]
        [DisplayName("Weapons and Equipment Patch")]
        [Description("Weapon Equipment patch for Rose Violet. Guns with 10 rounds and rebalanced damage.")]
        [DefaultValue(false)]
        [Display(Order = 6)]
        public bool WeaponsPatch { get; set; } = false;

        [Category("Optional")]
        [DisplayName("Ryuji Palace Scene (Rose is angry)")]
        [Description("Modified Ryuji Palace Scene. Disable it if you use the CBT option.")]
        [DefaultValue(false)]
        [Display(Order = 7)]
        public bool RyujiOuch { get; set; } = false;

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