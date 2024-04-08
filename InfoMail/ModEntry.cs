using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.TerrainFeatures;

namespace InfoMail
{
    public class ModEntry : Mod
    {
        public override void Entry(IModHelper helper)
        {
            helper.Events.Content.AssetRequested += Content_AssetRequested;
            helper.Events.GameLoop.DayStarted += GameLoop_DayStarted;
        }

        private void Content_AssetRequested(object sender, AssetRequestedEventArgs e)
        {
            if (e.NameWithoutLocale.IsEquivalentTo("Data/Mail"))
            {
                if (HasCrops())
                {
                    e.Edit(asset =>
                    {
                        var editor = asset.AsDictionary<string, string>();
                        editor.Data["cropNotifier"] = ModHelper.GenerateCropNotifierMessage();
                    });
                }
            }
        }

        private void GameLoop_DayStarted(object sender, DayStartedEventArgs e)
        {
            if (HasCrops())
            {
                Game1.mailbox.Add("cropNotifier");
            }
            Helper.GameContent.InvalidateCache("Data/Mail");
        }

        private bool HasCrops()
        {
            var greenHouseHasCrops = false;
            var gingerIslandHasCrops = false;
            var farmHasCrops = CheckLocationForCrops(Game1.getFarm(), "Farm");

            if (ModHelper.GreenhouseUnlocked())
            {
                greenHouseHasCrops = CheckLocationForCrops(Game1.getLocationFromName("Greenhouse"), "Greenhouse");
            }

            if (ModHelper.GingerIslandUnlocked())
            {
                gingerIslandHasCrops = CheckLocationForCrops(Game1.getLocationFromName("IslandWest"), "Ginger Island");
            }
            return farmHasCrops || greenHouseHasCrops || gingerIslandHasCrops;
        }

        private bool CheckLocationForCrops(GameLocation location, string locationName)
        {
            if (location == null)
                return false;

            var cropsNeedWatering = location.terrainFeatures.Values.OfType<HoeDirt>()
                .Where(hd => hd.crop != null && !hd.crop.dead.Value && hd.state.Value == 0).Any();
            var cropsReadyToHarvest = location.terrainFeatures.Values.OfType<HoeDirt>()
                .Where(hd => hd.crop != null && hd.crop.currentPhase.Value >= hd.crop.phaseDays.Count - 1).Any();

            return cropsNeedWatering || cropsReadyToHarvest;

        }
    }
}