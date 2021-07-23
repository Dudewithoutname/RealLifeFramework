using SDG.Unturned;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RealLifeFramework.Taser
{
    public class UseableTasers : MonoBehaviour
    {
        private List<CSteamID> tasedPlayers;
        private uint taserId => RealLife.Instance.Configuration.Instance.TaserID;
        private float tasedTime => RealLife.Instance.Configuration.Instance.TasedTime;

        private void Awake()
        {
            tasedPlayers = new List<CSteamID>();
            DamageTool.damagePlayerRequested += onPlayerDamage;
            Provider.onEnemyDisconnected += onDisconnected;
        }

        private void onDisconnected(SteamPlayer player)
        {
            if (tasedPlayers.Contains(player.playerID.steamID))
                tasedPlayers.Remove(player.playerID.steamID);
        }

        private void onPlayerDamage(ref DamagePlayerParameters parameters, ref bool shouldAllow)
        {
            var attacker = PlayerTool.getPlayer(parameters.killer);
            var victim = parameters.player;

            if (attacker.equipment.asset != null && parameters.cause == EDeathCause.GUN)
            {
                if (attacker.equipment.asset.id == RealLife.Instance.Configuration.Instance.TaserID)
                {
                    victim.equipment.dequip();

                    victim.movement.sendPluginSpeedMultiplier(0.1f);
                    victim.movement.sendPluginJumpMultiplier(0.1f);

                    victim.stance.stance = EPlayerStance.PRONE;
                    victim.stance.checkStance(EPlayerStance.PRONE);

                    tasedPlayers.Add(victim.channel.owner.playerID.steamID);

                    StartCoroutine(nameof(RemoveFromTased), victim);
                    StartCoroutine(CheckTased(victim));

                    shouldAllow = false;
                }
            }
        }

        private IEnumerator CheckTased(Player victim)
        {
            while (tasedPlayers.Contains(victim.channel.owner.playerID.steamID))
            {
                victim.equipment.dequip();
                victim.stance.stance = EPlayerStance.PRONE;
                victim.stance.checkStance(EPlayerStance.PRONE);

                yield return new WaitForSeconds(0.3f);
            }
        }

        private IEnumerator RemoveFromTased(Player victim)
        {
            yield return new WaitForSeconds(tasedTime);

            victim.movement.sendPluginSpeedMultiplier(1f);
            victim.movement.sendPluginJumpMultiplier(1f);
            tasedPlayers.Remove(victim.channel.owner.playerID.steamID);
        }
    }
}
