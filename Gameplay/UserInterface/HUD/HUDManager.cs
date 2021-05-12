using System;
using System.Collections.Generic;
using SDG.Unturned;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using RealLifeFramework.Players;
using RealLifeFramework.Patches;

namespace RealLifeFramework.UserInterface
{
    [EventHandler("HudManager")]
    public class HUDManager : IEventComponent
    {
        public void HookEvents()
        {
            UnturnedPlayerEvents.OnPlayerUpdateHealth += updateHealth;
            UnturnedPlayerEvents.OnPlayerUpdateFood += updateFood;
            UnturnedPlayerEvents.OnPlayerUpdateWater += updateWater;
            UnturnedPlayerEvents.OnPlayerUpdateStamina += updateStamina;
            UnturnedPlayerEvents.OnPlayerUpdateExperience += updateExperience;
            Time.onTimeUpdated += updateTime;
        }

        private static void updateHealth(UnturnedPlayer player, byte val)
        {
            var rplayer = RealPlayerManager.GetRealPlayer(player);

            if(rplayer != null)
                rplayer.HUD.UpdateHealth(val);
        }

        private static void updateFood(UnturnedPlayer player, byte val)
        {
            var rplayer = RealPlayerManager.GetRealPlayer(player);

            if (rplayer != null)
                rplayer.HUD.UpdateFood(val);
        }

        private static void updateWater(UnturnedPlayer player, byte val)
        {
            var rplayer = RealPlayerManager.GetRealPlayer(player);

            if (rplayer != null)
                rplayer.HUD.UpdateWater(val);
        }

        private static void updateStamina(UnturnedPlayer player, byte val)
        {
            var rplayer = RealPlayerManager.GetRealPlayer(player);

            if (rplayer != null)
                rplayer.HUD.UpdateStamina(val);
        }

        private static void updateExperience(UnturnedPlayer player, uint val)
        {
            var rplayer = RealPlayerManager.GetRealPlayer(player);

            if (rplayer != null)
                rplayer.HUD.UpdateMoney(val);
        }

        private static void updateTime(ushort hours, ushort minutes)
        {
            foreach (SteamPlayer sp in Provider.clients)
            {
                RealPlayer player = RealPlayerManager.GetRealPlayer(sp.playerID.steamID);

                if (player != null)
                    player.HUD.UpdateTime(hours, minutes);
            }
        }
    }
}
