using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using RealLifeFramework.RealPlayers;
using UnityEngine;
using System;

namespace RealLifeFramework.Realism
{
    [EventHandler]
    public class VehicleCrash : IEventComponent
    {
        public void HookEvents()
        {
            VehicleManager.onDamageVehicleRequested += onDamageVehicle;
        }

        private void onDamageVehicle(CSteamID instigatorSteamID, InteractableVehicle vehicle, ref ushort pendingTotalDamage, ref bool canRepair, ref bool shouldAllow, EDamageOrigin damageOrigin)
        {
            if (pendingTotalDamage <= 2)
                return;

            if (damageOrigin == EDamageOrigin.Vehicle_Collision_Self_Damage)
            {
                foreach(var passenger in vehicle.passengers)
                {
                    if (passenger == null)
                        continue;

                    var player = RealPlayer.From(passenger.player.playerID.steamID);

                    if (vehicle.asset.engine == EEngine.CAR && player != null)
                    {
                        if (!player.HUD.HasSeatBelt)
                        {
                            player.Player.life.askDamage((byte)UnityEngine.Random.Range(30, 45), Vector3.zero, EDeathCause.VEHICLE, ELimb.SKULL, CSteamID.Nil, out EPlayerKill kill, false, ERagdollEffect.NONE, Convert.ToBoolean(UnityEngine.Random.Range(0,1)));
                            VehicleManager.forceRemovePlayer(vehicle, passenger.player.playerID.steamID);
                            player.Player.life.serverModifyHallucination(5f);
                            player.Player.stance.stance = EPlayerStance.PRONE;
                            player.Player.stance.checkStance(EPlayerStance.PRONE);
                            player.Player.life.breakLegs();
                        }
                    }
                    // for some reason this should fix that strange bug
                    break;
                }
            }
        }
    }
}
