using StardewValley;
using System.Text;

namespace InfoMail
{
    public static class ModHelper
    {
        public static string GenerateCropNotifierMessage()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Dear @,^");
            sb.Append("Some of your crops need watering.^");
            sb.Append("Some of your crops are ready for harvest.^^");
            sb.Append("Yours,^");
            sb.Append("Crop Notifier^^");
            sb.Append($"PS. Today is {Game1.stats.DaysPlayed} day");
            return sb.ToString();
        }

        public static bool GingerIslandUnlocked()
        {
            return Game1.player.mailReceived.Contains("willyBoatFixed");
        }

        public static bool GreenhouseUnlocked()
        {
            return Game1.player.mailReceived.Contains("ccPantry");
        }
    }
}
