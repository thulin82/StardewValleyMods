﻿using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using System.Linq;

namespace CleanFarm
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
                helper.Events.GameLoop.DayStarted += GameLoop_DayStarted;
                Monitor.Log("Up and Running", LogLevel.Info);
            }
            else
            {
                Monitor.Log("Disabled.", LogLevel.Info);
            }

        }

        /// <summary>
        /// Raised after a new day is started
        /// </summary>
        /// <param name="sender">The event sender</param>
        /// <param name="e">The event data</param>
        private void GameLoop_DayStarted(object sender, DayStartedEventArgs e)
        {
            Game1.locations.Where(loc => loc.NameOrUniqueName.Equals("Farm")).ToList()
                           .ForEach(loc => loc.Objects.Pairs.Where(kvp => 
                               kvp.Value.Name.Equals("Weeds") || 
                               kvp.Value.Name.Equals("Stone") || 
                               kvp.Value.Name.Equals("Twig"))
                           .ToList()
                           .ForEach(kvp => loc.Objects.Remove(kvp.Key)));   
        }
    }
}