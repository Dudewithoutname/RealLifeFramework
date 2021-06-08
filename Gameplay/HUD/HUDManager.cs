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
            UnturnedPlayerEvents.OnPlayerUpdateBroken += updateBroken;
            UnturnedPlayerEvents.OnPlayerUpdateBleeding += updateBleeding;
            Provider.onEnemyConnected += subscribe;
            Provider.onEnemyDisconnected += unSubscribe;
            Time.onTimeUpdated += updateTime;
        }
        private static void subscribe(SteamPlayer player)
        {
           // player.player.movement.onVehicleUpdated += onVehicleUpdate;
        }

        private static void unSubscribe(SteamPlayer player)
        {
            //player.player.movement.onVehicleUpdated -= onVehicleUpdate;
        }

        private static void onVehicleUpdate(bool isDriveable, ushort newFuel, ushort maxFuel, float newSpeed, float minSpeed, float maxSpeed, ushort newHeath, ushort maxHealth, ushort newBatteryCharge)
        {
            CommandWindow.Log($"update {isDriveable}");
        }

        private static void updateBleeding(UnturnedPlayer player, bool isBleeding)
        {
            var rplayer = RealPlayerManager.GetRealPlayer(player);

            if (rplayer != null)
                return;

            if (isBleeding)
                rplayer.HUD.SendWidget(EWidgetType.Bleeding);
            else
                rplayer.HUD.RemoveWidget(EWidgetType.Bleeding);
        }


        private static void updateBroken(UnturnedPlayer player, bool isBroken)
        {
            var rplayer = RealPlayerManager.GetRealPlayer(player);

            if (rplayer != null)
                return;

            if (isBroken)
                rplayer.HUD.SendWidget(EWidgetType.BrokenBone);
            else
                rplayer.HUD.RemoveWidget(EWidgetType.BrokenBone);
        }

        private static void updateHealth(UnturnedPlayer player, byte val)
        {
            var rplayer = RealPlayerManager.GetRealPlayer(player);

            if(rplayer != null)
                rplayer.HUD.UpdateComponent(HUDComponent.Health, val.ToString());
        }

        private static void updateFood(UnturnedPlayer player, byte val)
        {
            var rplayer = RealPlayerManager.GetRealPlayer(player);

            if (rplayer != null)
                rplayer.HUD.UpdateComponent(HUDComponent.Food, val.ToString());
        }

        private static void updateWater(UnturnedPlayer player, byte val)
        {
            var rplayer = RealPlayerManager.GetRealPlayer(player);

            if (rplayer != null)
                rplayer.HUD.UpdateComponent(HUDComponent.Water, val.ToString());
        }

        private static void updateStamina(UnturnedPlayer player, byte val)
        {
            var rplayer = RealPlayerManager.GetRealPlayer(player);

            if (rplayer != null)
                rplayer.HUD.UpdateComponent(HUDComponent.Stamina, val.ToString());
        }

        private static void updateTime(ushort hours, ushort minutes)
        {
            foreach (SteamPlayer sp in Provider.clients)
            {
                RealPlayer player = RealPlayerManager.GetRealPlayer(sp.playerID.steamID);

                if (player != null)
                    player.HUD.UpdateComponent(HUDComponent.Time, HUD.FormatTime(hours, minutes));
            }
        }
    }
}
