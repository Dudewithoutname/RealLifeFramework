using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using RealLifeFramework.RealPlayers;
using UnityEngine;
using System;

namespace RealLifeFramework.Realism
{
    [EventHandler]
    public class Realism : IEventComponent
    {
    // tu sa ani nepozeraj toto je taka sracka ze az no hanbím sa za to xddd
        public void HookEvents()
        {
            DamageTool.damagePlayerRequested += onPlayerDamage;
            VehicleManager.onDamageVehicleRequested += onDamageVehicle;
            VehicleManager.onDamageTireRequested += onDamageTireRequested;
        }

        private void onPlayerDamage(ref DamagePlayerParameters parameters, ref bool shouldAllow)
        {
            if ((object)parameters.killer == (object)CSteamID.Nil) return;
            
            var victim = parameters.player;
            var attacker = PlayerTool.getPlayer(parameters.killer);

            if (parameters.cause == EDeathCause.GRENADE)
            {
                victim.life.serverModifyHallucination(5f);
                victim.stance.stance = EPlayerStance.CROUCH;
                victim.stance.checkStance(EPlayerStance.CROUCH);
                return;
            }
            // KURVA CO TU ROBIA TIE BITWISE OPERATORY - uf dufam ze sa to tak vola :DDDDDD
            if (parameters.limb == ELimb.RIGHT_LEG | parameters.limb == ELimb.LEFT_LEG | parameters.limb == ELimb.LEFT_FOOT | parameters.limb == ELimb.RIGHT_FOOT)
            {
                if (parameters.cause == EDeathCause.GUN && UnityEngine.Random.Range(0, 3) == 1) // 25%
                {
                    victim.life.breakLegs();
                    victim.stance.stance = EPlayerStance.PRONE;
                    victim.stance.checkStance(EPlayerStance.PRONE);
                    return;
                }

                if (parameters.cause == EDeathCause.MELEE && UnityEngine.Random.Range(0, 7) == 1) // 12.5%
                {
                    victim.life.breakLegs();
                    victim.stance.stance = EPlayerStance.PRONE;
                    victim.stance.checkStance(EPlayerStance.PRONE);
                    return;
                }

                return;
            }

            if (parameters.cause == EDeathCause.GUN || parameters.cause == EDeathCause.MELEE)
            {

                switch (parameters.limb)
                {
                    case ELimb.SKULL:
                        if (parameters.cause == EDeathCause.MELEE && UnityEngine.Random.Range(0, 3) == 1)
                        {
                            victim.life.serverModifyHallucination(5f);
                            victim.stance.stance = EPlayerStance.CROUCH;
                            victim.stance.checkStance(EPlayerStance.CROUCH);
                        }
                        if (parameters.cause == EDeathCause.GUN && UnityEngine.Random.Range(0, 4) == 1)
                        {
                            victim.life.serverModifyHallucination(3f);
                        }
                        break;

                    case ELimb.LEFT_HAND when (UnityEngine.Random.Range(0, 3) == 1): 
                        victim.equipment.dequip();
                        break;

                    case ELimb.RIGHT_HAND when (UnityEngine.Random.Range(0, 3) == 1):
                        victim.equipment.dequip();
                        break;

                    case ELimb.LEFT_ARM when (UnityEngine.Random.Range(0, 4) == 1):
                        victim.life.breakLegs();
                        break;

                    case ELimb.RIGHT_ARM when (UnityEngine.Random.Range(0, 4) == 1):
                        victim.life.breakLegs();
                        break;
                }
            }
        }

        private void onDamageVehicle(CSteamID instigatorSteamID, InteractableVehicle vehicle, ref ushort pendingTotalDamage, ref bool canRepair, ref bool shouldAllow, EDamageOrigin damageOrigin)
        {
            if (instigatorSteamID != CSteamID.Nil)
            {
                shouldAllow = false;
            }

            if (pendingTotalDamage <= 2)
                return;

            if (damageOrigin == EDamageOrigin.Vehicle_Collision_Self_Damage)
            {
                foreach(var passenger in vehicle.passengers)
                {
                    if (passenger == null)
                        continue;

                    try
                    {
                        var player = RealPlayer.From(passenger.player.playerID.steamID);


                        if (vehicle.asset.engine == EEngine.CAR && player != null)
                        {
                            if (!player.HUD.HasSeatBelt)
                            {
                                player.Player.life.askDamage((byte)UnityEngine.Random.Range(25, 45), Vector3.zero, EDeathCause.VEHICLE, ELimb.SKULL, CSteamID.Nil, out EPlayerKill kill, false, ERagdollEffect.NONE, Convert.ToBoolean(UnityEngine.Random.Range(0,1)));
                                VehicleManager.forceRemovePlayer(vehicle, passenger.player.playerID.steamID);
                                player.Player.life.serverModifyHallucination(5f);
                                player.Player.stance.stance = EPlayerStance.PRONE;
                                player.Player.stance.checkStance(EPlayerStance.PRONE);
                                player.Player.life.breakLegs();
                            }
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
        }

        private static void onDamageTireRequested(CSteamID instigatorSteamID, InteractableVehicle vehicle, int tireIndex, ref bool shouldAllow, EDamageOrigin damageOrigin)
        {
            var player = RealPlayer.From(instigatorSteamID);

            // Player Prevention
            if (damageOrigin == EDamageOrigin.Bullet_Explosion | damageOrigin == EDamageOrigin.Punch | damageOrigin == EDamageOrigin.Useable_Gun | 
                damageOrigin == EDamageOrigin.Useable_Melee && !player.Player.channel.owner.isAdmin)
                shouldAllow = false;
        }

    }
}
