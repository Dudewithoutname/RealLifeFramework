using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using RealLifeFramework.Players;
using UnityEngine;
namespace RealLifeFramework.Realism
{
    [EventHandler(nameof(VehicleCrash))]
    public class VehicleCrash : IEventComponent
    {
        public void HookEvents()
        {
            VehicleManager.onDamageVehicleRequested += onDamageVehicle;
        }

        private void onDamageVehicle(CSteamID instigatorSteamID, InteractableVehicle vehicle, ref ushort pendingTotalDamage, ref bool canRepair, ref bool shouldAllow, EDamageOrigin damageOrigin)
        {
            var player = RealPlayerManager.GetRealPlayer(instigatorSteamID);

            if (pendingTotalDamage <= 2)
                return;

            if (player == null)
                return;

            if (damageOrigin == EDamageOrigin.Vehicle_Collision_Self_Damage)
            {

                if (player.Player.movement.getVehicle().asset.engine == EEngine.CAR)
                {
                    if (!player.HUD.HasSeatBelt)
                    {
                        if(player.Player.life.health > 20)
                        {
                            player.Player.life.askDamage( 19, new Vector3(player.Player.transform.position.x, player.Player.transform.position.y, player.Player.transform.position.z),
                                EDeathCause.VEHICLE, ELimb.SPINE, CSteamID.Nil, out EPlayerKill kill);
                        }
                        else
                        {
                            player.Player.life.askDamage( (byte)(player.Player.life.health-1) , new Vector3(player.Player.transform.position.x, player.Player.transform.position.y, player.Player.transform.position.z),
                                EDeathCause.VEHICLE, ELimb.SPINE, CSteamID.Nil, out EPlayerKill kill);

                            player.Player.stance.stance = EPlayerStance.PRONE;
                            player.Player.stance.checkStance(EPlayerStance.PRONE);
                            player.Player.life.serverModifyHallucination(5f);
                            player.Player.life.breakLegs();

                        }

                        if (Random.Range(0, 2) == 1) // 33 %
                        {
                            VehicleManager.forceRemovePlayer(vehicle, instigatorSteamID);
                            player.Player.stance.stance = EPlayerStance.PRONE;
                            player.Player.stance.checkStance(EPlayerStance.PRONE);
                            player.Player.life.serverModifyHallucination(5f);
                            player.Player.life.breakLegs();
                        }
                    }
                }
            }
        }
    }
}
