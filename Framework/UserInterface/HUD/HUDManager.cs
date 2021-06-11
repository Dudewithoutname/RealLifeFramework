using System;
using System.Collections.Generic;
using SDG.Unturned;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using RealLifeFramework.Players;
using RealLifeFramework.Patches;
using Steamworks;
using System.Linq;
using Rocket.API.Extensions;
using UnityEngine;

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
            UnturnedPlayerEvents.OnPlayerUpdateVirus += checkVirus;
            UnturnedPlayerEvents.OnPlayerDeath += onPlayerDeath;
            Patches.Time.onTimeUpdated += updateTime;
            InteractableVehicle.OnPassengerAdded_Global += (vehicle, seat) => onVehicleEnter(vehicle, seat);
            InteractableVehicle.OnPassengerRemoved_Global += (vehicle, seat, player) => onVehicleExit(vehicle, seat, player);
            VehicleManager.onExitVehicleRequested += onVehicleExitRequested;
        }

        private static void onVehicleExitRequested(Player player, InteractableVehicle vehicle, ref bool shouldAllow, ref Vector3 pendingLocation, ref float pendingYaw)
        {
            if (vehicle.asset.engine == EEngine.CAR)
            {
                var rplayer = RealPlayerManager.GetRealPlayer(player);

                if (rplayer.HUD.HasSeatBelt)
                {
                    shouldAllow = false;
                }
            }
        }

        private static void onVehicleEnter(InteractableVehicle vehicle, int seat)
        {
            if (vehicle.asset.engine == EEngine.CAR)
            {
                var rplayer = RealPlayerManager.GetRealPlayer(vehicle.passengers[seat].player.player);

                rplayer.HUD.HasSeatBelt = false;
                rplayer.HUD.UpdateComponent(HUDComponent.Seatbelt[Convert.ToInt32(rplayer.HUD.HasSeatBelt)], true);
            }
        }

        private static void onVehicleExit(InteractableVehicle vehicle, int seat, Player player)
        {
            if (vehicle.asset.engine == EEngine.CAR)
            {
                var rplayer = RealPlayerManager.GetRealPlayer(player);

                for (int i = 0; i < 2; i++)
                {
                    rplayer.HUD.UpdateComponent(HUDComponent.Seatbelt[i], false); 
                }
            }
        }

        private static void onPlayerDeath(UnturnedPlayer player, EDeathCause cause, ELimb limb, CSteamID murderer)
        {
            var rplayer = RealPlayerManager.GetRealPlayer(player);

            if (rplayer == null)
                return;

            rplayer.HUD.RemoveWidget(EWidgetType.Bleeding);
            rplayer.HUD.RemoveWidget(EWidgetType.BrokenBone);
            rplayer.HUD.RemoveWidget(EWidgetType.LowVirus);
        }
        private static void checkVirus(UnturnedPlayer player, byte virus)
        {
            var rplayer = RealPlayerManager.GetRealPlayer(player);

            if (rplayer == null)
                return;

            if (virus < 40)
                rplayer.HUD.SendWidget(EWidgetType.LowVirus);
            else
                rplayer.HUD.RemoveWidget(EWidgetType.LowVirus);

        }

        private static void updateBleeding(UnturnedPlayer player, bool isBleeding)
        {
            var rplayer = RealPlayerManager.GetRealPlayer(player);

            if (rplayer == null)
                return;

            if (isBleeding)
                rplayer.HUD.SendWidget(EWidgetType.Bleeding);
            else
                rplayer.HUD.RemoveWidget(EWidgetType.Bleeding);
        }


        private static void updateBroken(UnturnedPlayer player, bool isBroken)
        {
            var rplayer = RealPlayerManager.GetRealPlayer(player);

            if (rplayer == null)
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
