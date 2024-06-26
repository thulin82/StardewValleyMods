﻿using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace StaminaHelper
{
    public class ModEntry : Mod
    {
        private ModConfig Config;


        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            Config = Helper.ReadConfig<ModConfig>();
            if (Config.Enabled)
            {
                helper.Events.Input.ButtonPressed += Input_ButtonPressed;
                Monitor.Log("Up and Running", LogLevel.Info);
            }
            else
            {
                Monitor.Log("Disabled.", LogLevel.Info);
            }

        }

        /*********
        ** Private methods
        *********/
        /// <summary>
        /// Raised after the player presses a button on the keyboard, controller, or mouse.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void Input_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            // ignore if player hasn't loaded a save yet
            if (!Context.IsWorldReady)
                return;

            if (e.Button == SButton.F10)
            {
                Monitor.Log("Current MaxHealth is " + Game1.player.maxHealth, LogLevel.Info);
                Monitor.Log("Current MaxStamina is " + Game1.player.MaxStamina, LogLevel.Info);
                Monitor.Log("Current Health is " + Game1.player.health, LogLevel.Info);
                Monitor.Log("Current Stamina is " + Game1.player.Stamina, LogLevel.Info);
            }

            if (e.Button == SButton.F11)
            {
                Game1.player.Stamina += Config.StaminaIncrease;
            }
        }
    }
}