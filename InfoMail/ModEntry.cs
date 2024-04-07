using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.TerrainFeatures;

namespace InfoMail
{
    public class CropNotifierMod : Mod
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
                        editor.Data["cropNotifier"] = $"Dear @^^Some of your crops need watering.^Some of your crops are ready for harvest^^Yours,^Crop Notifier^^PS. Today is {Game1.stats.DaysPlayed} day";
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
        }

        private bool HasCrops()
        {
            return CheckLocationForCrops(Game1.getFarm(), "Farm") ||
                   CheckLocationForCrops(Game1.getLocationFromName("Greenhouse"), "Greenhouse") ||
                   CheckLocationForCrops(Game1.getLocationFromName("IslandWest"), "Ginger Island");
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