using System;
using System.Collections.Generic;
using SDG.Unturned;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using RealLifeFramework.Players;

namespace RealLifeFramework.UserInterface
{
    public class HUDManager : IEventComponent
    {
        public void HookEvents()
        {
            UnturnedPlayerEvents.OnPlayerUpdateHealth += UpdateHealth;
            UnturnedPlayerEvents.OnPlayerUpdateFood += UpdateFood;
            UnturnedPlayerEvents.OnPlayerUpdateWater += UpdateWater;
            UnturnedPlayerEvents.OnPlayerUpdateStamina += UpdateStamina;
            UnturnedPlayerEvents.OnPlayerUpdateExperience += UpdateExperience;
        }

        private static void UpdateHealth(UnturnedPlayer player, byte val)
        {
            var rplayer = RealPlayerManager.GetRealPlayer(player);

            if(rplayer != null)
                rplayer.HUD.UpdateHealthUI(val);
        }

        private static void UpdateFood(UnturnedPlayer player, byte val)
        {
            var rplayer = RealPlayerManager.GetRealPlayer(player);

            if (rplayer != null)
                rplayer.HUD.UpdateFoodUI(val);
        }

        private static void UpdateWater(UnturnedPlayer player, byte val)
        {
            var rplayer = RealPlayerManager.GetRealPlayer(player);

            if (rplayer != null)
                rplayer.HUD.UpdateWaterUI(val);
        }

        private static void UpdateStamina(UnturnedPlayer player, byte val)
        {
            var rplayer = RealPlayerManager.GetRealPlayer(player);

            if (rplayer != null)
                rplayer.HUD.UpdateStaminaUI(val);
        }

        private static void UpdateExperience(UnturnedPlayer player, uint val)
        {
            var rplayer = RealPlayerManager.GetRealPlayer(player);

            if (rplayer != null)
                rplayer.HUD.UpdateMoneyUI(val);
        }

        public static void UpdateTime(ushort hours, ushort minutes)
        {
            foreach (SteamPlayer sp in Provider.clients)
            {
                RealPlayer player = RealPlayerManager.GetRealPlayer(sp.playerID.steamID);

                if (player != null)
                    player.HUD.UpdateTimeUI(hours, minutes);
            }
        }
    }
}
