using System;
using System.Collections.Generic;
using SDG.Unturned;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using RealLifeFramework.RealPlayers;
using RealLifeFramework.Patches;
using Steamworks;
using System.Linq;
using Rocket.API.Extensions;
using UnityEngine;

namespace RealLifeFramework.UserInterface
{
    [EventHandler]
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
            UnturnedPlayerEvents.OnPlayerUpdateVirus += updateVirus;
            UnturnedPlayerEvents.OnPlayerDeath += onPlayerDeath;
            Patches.Time.onTimeUpdated += updateTime;
            InteractableVehicle.OnPassengerAdded_Global += (vehicle, seat) => onVehicleEnter(vehicle, seat);
            InteractableVehicle.OnPassengerRemoved_Global += (vehicle, seat, player) => onVehicleExit(vehicle, seat, player);
            VehicleManager.onExitVehicleRequested += onVehicleExitRequested;
            PlayerEquipment.OnUseableChanged_Global += (equipment) => onUsebleChanged(equipment);
            UseableGun.onChangeMagazineRequested += changeMagazine;
            UseableGun.onBulletSpawned += onShooted;
            ChangeFiremode.OnFiremodeChanged += onFiremodeChanged;
            PlayerSkills.OnExperienceChanged_Global += (instance, exp) => onExpUpdate(instance, exp);
        }

        private static void onExpUpdate(PlayerSkills skills, uint exp)
        {
            var rp = RealPlayer.From(skills.player);

            rp.HUD.UpdateComponent(HUDComponent.Credit);
        }

        private static void onPlayerDeath(UnturnedPlayer player, EDeathCause cause, ELimb limb, CSteamID murderer)
        {
            var rp = RealPlayer.From(player);

            if (rp == null)
                return;

            rp.HUD.RemoveWidget(EWidgetType.Bleeding);
            rp.HUD.RemoveWidget(EWidgetType.BrokenBone);
            rp.HUD.RemoveWidget(EWidgetType.LowVirus);
        }

        #region Weapon
        private static void onUsebleChanged(PlayerEquipment equipment)
        {
            var player = RealPlayer.From(equipment.player);

            if (equipment.asset != null && equipment.asset.type == EItemType.GUN)
            {
                ushort magId = BitConverter.ToUInt16(new byte[] { equipment.state[8], equipment.state[9] }, 0);

                if (magId != 0)
                {
                    var maxAmmo = new Item(magId, true).amount;
                    
                    if(!player.HUD.isHidden)
                        player.HUD.UpdateComponent(HUDComponent.WeaponStats, true);

                    player.HUD.UpdateComponent(HUDComponent.Ammo, equipment.state[10].ToString());
                    player.HUD.UpdateComponent(HUDComponent.FullAmmo, maxAmmo.ToString());
                    player.HUD.UpdateComponent(HUDComponent.Firemode, getFiremode(equipment.state[11]));
                }
                else
                {
                    if (!player.HUD.isHidden)
                        player.HUD.UpdateComponent(HUDComponent.WeaponStats, true);

                    player.HUD.UpdateComponent(HUDComponent.Ammo, "0");
                    player.HUD.UpdateComponent(HUDComponent.FullAmmo, "0");
                    player.HUD.UpdateComponent(HUDComponent.Firemode, getFiremode(equipment.state[11]));
                }
            }
            else
            {
                player.HUD.UpdateComponent(HUDComponent.WeaponStats, false);
            }
        }

        private static void onFiremodeChanged(Player rawPlayer, EFiremode firemode)
        {
            var player = RealPlayer.From(rawPlayer);
            player.HUD.UpdateComponent(HUDComponent.Firemode, getFiremode(firemode));
        }

        private static void changeMagazine(PlayerEquipment equipment, UseableGun gun, Item oldItem, ItemJar newItem, ref bool shouldAllow)
        {
            var player = RealPlayer.From(equipment.player);
            
            if (player == null)
                return;

            if (newItem != null)
            {
                var maxAmmo = new Item(newItem.item.id, true).amount;

                player.HUD.UpdateComponent(HUDComponent.Ammo, newItem.item.amount.ToString());
                player.HUD.UpdateComponent(HUDComponent.FullAmmo, maxAmmo.ToString());
            }
            else
            {
                player.HUD.UpdateComponent(HUDComponent.Ammo, "0");
                player.HUD.UpdateComponent(HUDComponent.FullAmmo, "0");
            }
        }

        private static void onShooted(UseableGun gun, BulletInfo bullet)
        {
            var player = RealPlayer.From(gun.player);
            player.HUD.UpdateComponent(HUDComponent.Ammo, gun.player.equipment.state[10].ToString());
        }

        private static string getFiremode(EFiremode firemode)
        {
            switch (firemode)
            {
                case EFiremode.SAFETY:
                    return "SAFE";
                case EFiremode.SEMI:
                    return "SEMI";
                case EFiremode.AUTO:
                    return "AUTO";
                case EFiremode.BURST:
                    return "BURST";
            }

            return "NEVIEM";
        }

        private static string getFiremode(byte firemode)
        {
            switch (firemode)
            {
                case 0:
                    return "SAFE";
                case 1:
                    return "SEMI";
                case 2:
                    return "AUTO";
                case 3:
                    return "BURST";
            }

            return "NEVIEM";
        }

        #endregion

        #region Seatbelt
        private static void onVehicleExitRequested(Player player, InteractableVehicle vehicle, ref bool shouldAllow, ref Vector3 pendingLocation, ref float pendingYaw)
        {
            if (vehicle.asset.engine == EEngine.CAR)
            {
                var rp = RealPlayer.From(player);

                if (rp.HUD.HasSeatBelt)
                {
                    shouldAllow = false;
                }
            }
        }

        private static void onVehicleEnter(InteractableVehicle vehicle, int seat)
        {
            if (vehicle.asset.engine == EEngine.CAR)
            {
                var rp = RealPlayer.From(vehicle.passengers[seat].player.player);

                rp.HUD.HasSeatBelt = false;
                if (!rp.HUD.isHidden)
                    rp.HUD.UpdateComponent(HUDComponent.Seatbelt[Convert.ToInt32(rp.HUD.HasSeatBelt)], true);
            }
        }

        private static void onVehicleExit(InteractableVehicle vehicle, int seat, Player player)
        {
            if (vehicle.asset.engine == EEngine.CAR)
            {
                var rp = RealPlayer.From(player);

                for (int i = 0; i < 2; i++)
                {
                    rp.HUD.UpdateComponent(HUDComponent.Seatbelt[i], false); 
                }
            }
        }
        #endregion


        #region Life
        private static void updateVirus(UnturnedPlayer player, byte virus)
        {
            var rp = RealPlayer.From(player);

            if (rp == null)
                return;

            if (virus <= 45)
                rp.HUD.SendWidget(EWidgetType.LowVirus);
            else
                rp.HUD.RemoveWidget(EWidgetType.LowVirus);

        }

        private static void updateBleeding(UnturnedPlayer player, bool isBleeding)
        {
            var rp = RealPlayer.From(player);

            if (rp == null)
                return;

            if (isBleeding)
                rp.HUD.SendWidget(EWidgetType.Bleeding);
            else
                rp.HUD.RemoveWidget(EWidgetType.Bleeding);
        }


        private static void updateBroken(UnturnedPlayer player, bool isBroken)
        {
            var rp = RealPlayer.From(player);

            if (rp == null)
                return;

            if (isBroken)
                rp.HUD.SendWidget(EWidgetType.BrokenBone);
            else
                rp.HUD.RemoveWidget(EWidgetType.BrokenBone);
        }

        private static void updateHealth(UnturnedPlayer player, byte val)
        {
            var rp = RealPlayer.From(player);

            if(rp != null)
                rp.HUD.UpdateComponent(HUDComponent.Health, val.ToString());
        }

        private static void updateFood(UnturnedPlayer player, byte val)
        {
            var rp = RealPlayer.From(player);

            if (rp != null)
                rp.HUD.UpdateComponent(HUDComponent.Food, val.ToString());
        }

        private static void updateWater(UnturnedPlayer player, byte val)
        {
            var rp = RealPlayer.From(player);

            if (rp != null)
                rp.HUD.UpdateComponent(HUDComponent.Water, val.ToString());
        }

        private static void updateStamina(UnturnedPlayer player, byte val)
        {
            var rp = RealPlayer.From(player);

            if (rp != null)
                rp.HUD.UpdateComponent(HUDComponent.Stamina, val.ToString());
        }

        private static void updateTime(ushort hours, ushort minutes)
        {
            foreach (SteamPlayer sp in Provider.clients)
            {
                RealPlayer player = RealPlayer.From(sp.playerID.steamID);

                if (player != null)
                    player.HUD.UpdateComponent(HUDComponent.Time, HUD.FormatTime(hours, minutes));
            }
        }
        #endregion
    }
}
