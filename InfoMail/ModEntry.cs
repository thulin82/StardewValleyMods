using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.TerrainFeatures;
using System.Linq;
using System.Text;

public class CropNotifierMod : Mod
{
    public override void Entry(IModHelper helper)
    {
        helper.Events.GameLoop.DayStarted += GameLoop_DayStarted;
    }

    private void GameLoop_DayStarted(object sender, DayStartedEventArgs e)
    {
        StringBuilder letter = new StringBuilder("Dear Farmer,\n\n");

        bool hasCrops = false;
        hasCrops |= CheckLocationForCrops(letter, Game1.getFarm(), "Farm");
        hasCrops |= CheckLocationForCrops(letter, Game1.getLocationFromName("Greenhouse"), "Greenhouse");
        hasCrops |= CheckLocationForCrops(letter, Game1.getLocationFromName("IslandWest"), "Ginger Island");

        if (hasCrops)
        {
            letter.Append("\nYours,\nCrop Notifier");
            Game1.mailbox.Add(letter.ToString());
        }
    }

    private bool CheckLocationForCrops(StringBuilder letter, GameLocation location, string locationName)
    {
        if (location == null)
            return false;

        var cropsNeedWatering = location.terrainFeatures.Values.OfType<HoeDirt>()
            .Where(hd => hd.crop != null && !hd.crop.dead.Value && hd.state.Value == 0).Any();
        var cropsReadyToHarvest = location.terrainFeatures.Values.OfType<HoeDirt>()
            .Where(hd => hd.crop != null && hd.crop.currentPhase.Value >= hd.crop.phaseDays.Count - 1).Any();

        if (cropsNeedWatering || cropsReadyToHarvest)
        {
            letter.Append($"At the {locationName}:\n");
            if (cropsNeedWatering)
                letter.Append("Some of your crops need watering.\n");
            if (cropsReadyToHarvest)
                letter.Append("Some of your crops are ready to harvest.\n");
            letter.Append('\n');

            return true;
        }

        return false;
    }
}