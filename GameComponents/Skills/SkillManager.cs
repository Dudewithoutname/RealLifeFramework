using SDG.Unturned;
using Rocket.Unturned.Player;
using Rocket.Unturned.Events;
using RealLifeFramework.RealPlayers;
using RealLifeFramework.Skills;
using RealLifeFramework.Items;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;
using Rocket.Unturned;
using System;
using RealLifeFramework.Threadding;

namespace RealLifeFramework.Skills
{
    [EventHandler]
    public class SkillManager : IEventComponent
    {

        private static Dictionary<CSteamID, Vector3> prevPos;
        private static Dictionary<CSteamID, bool> wasSprinting;

        public void HookEvents()
        {
            prevPos = new Dictionary<CSteamID, Vector3>();
            wasSprinting = new Dictionary<CSteamID, bool>();

            U.Events.OnPlayerConnected += AddPrevPos;
            U.Events.OnPlayerDisconnected += RemovePrevPos;

            Player.onPlayerStatIncremented += HandleStatIncremented;
            DamageTool.damagePlayerRequested += IncrementEndurance;
            UseableConsumeable.onConsumePerformed += HandleConsume;
            UseableGun.onBulletSpawned += DefenseShooted;
            
            // Prevention
            PlayerSkills.OnSkillUpgraded_Global += (player, speciality, index, level) => player.ServerSetSkillLevel(speciality, index, (level - 1));
        }

        private static void DefenseShooted(UseableGun gun, BulletInfo bullet) => RealPlayer.From(gun.player).SkillUser.AddExp(Defense.Id, 1);

        private static void IncrementEndurance(ref DamagePlayerParameters parameters, ref bool shouldAllow)
        {
            if ((object)parameters.killer != null)
            {
                if (parameters.cause == EDeathCause.MELEE | parameters.cause == EDeathCause.GUN)
                {
                    if (parameters.player.stance.stance != EPlayerStance.PRONE)
                    {
                        RealPlayer.From(parameters.killer).SkillUser.AddExp(Defense.Id, 2); // bonus for hit
                    }
                }
            }

            if (parameters.damage >= 10)
            {
                if (parameters.player.stance.stance != EPlayerStance.PRONE)
                {
                    RealPlayer.From(parameters.player).SkillUser.AddExp(Endurance.Id, 2);
                }
            }
        }

        public static void SendLevelUp(RealPlayer player, int skillId)
        {
            var skill = player.SkillUser.Skills[skillId];

            if (player.RankUser.Vip == null)
            {
                player.AddExp(5);
            }
            else
            {
                switch (player.RankUser.Vip.Value.Level)
                {
                    case 0:
                        player.AddExp(7);
                        break;
                    case 1:
                        player.AddExp(7);
                        break;
                    case 2:
                        player.AddExp(10);
                        break;
                    case 3:
                        player.AddExp(10);
                        break;
                }
            }

            if (skill.Level == skill.MaxLevel)
            {
                ChatManager.serverSendMessage($"<b><color=#FFD846>Skills</color></b> | Dosiahol si <color=#FFD846>MAX level</color> v <color=#FFD846>{skill.Name}</color>", Color.white, null, player.Player.channel.owner, EChatMode.SAY, RealLife.Instance.Configuration.Instance.SkillIconURL, true);
            }
            else
            {
                ChatManager.serverSendMessage($"<b><color=#FFD846>Skills</color></b> | Dosiahol si <color=#FFD846>level {skill.Level}</color> v <color=#FFD846>{skill.Name}</color>", Color.white, null, player.Player.channel.owner, EChatMode.SAY, RealLife.Instance.Configuration.Instance.SkillIconURL, true);
            }

            if (skill.Exp >= skill.GetExpToNextLevel())
            {
                skill.Exp -= skill.GetExpToNextLevel();
                skill.Upgrade();
            }
        }

        private static void HandleStatIncremented(Player player, EPlayerStat stat)
        {
            var rp = RealPlayer.From(player);

            switch (stat)
            {
                case EPlayerStat.FOUND_FISHES:
                    rp.SkillUser.AddExp(Fishing.Id, 3);
                    break;
                case EPlayerStat.FOUND_PLANTS:
                    rp.SkillUser.AddExp(Farming.Id, 3);
                    break;
                case EPlayerStat.FOUND_RESOURCES:
                    rp.SkillUser.AddExp(Dexterity.Id, 3);
                    break;
            }
        }

        private static void HandleConsume(Player player, ItemConsumeableAsset consumeableAsset)
        {
            var rp = RealPlayer.From(player);

            if (MedicalItems.Ids.Contains(consumeableAsset.id))
                rp.SkillUser.AddExp(Endurance.Id, 3);
        }


        private static void CheckRunning(UnturnedPlayer player)
        {
            if (player.Player.stance.stance == EPlayerStance.SPRINT) // 2
            {
                prevPos[player.CSteamID] = player.Position;
                wasSprinting[player.CSteamID] = true;
            }
            else
            {
                if (wasSprinting[player.CSteamID] == true)
                {
                    wasSprinting[player.CSteamID] = false;

                    var rp = RealPlayer.From(player);

                    var distance = (int)Math.Round(Vector3.Distance(prevPos[player.CSteamID], player.Position));
                    uint exp;

                    if (distance > 500)
                    {
                        exp = 50;
                        rp.SkillUser.AddExp(Agitily.Id, exp);
                    }
                    if (distance > 20 && distance < 500)
                    {
                        exp = (uint)Math.Floor((decimal)(distance / 10));
                        rp.SkillUser.AddExp(Agitily.Id, exp);
                    }
                }
            }
        }

        private static void AddPrevPos(UnturnedPlayer player)
        {

            if(!prevPos.ContainsKey(player.CSteamID))
                prevPos.Add(player.CSteamID, player.Position);

            if(!wasSprinting.ContainsKey(player.CSteamID))
                wasSprinting.Add(player.CSteamID, false);

            player.Player.stance.onStanceUpdated += () => CheckRunning(player);

        }

        private static void RemovePrevPos(UnturnedPlayer player)
        {
            if (prevPos.ContainsKey(player.CSteamID))
                prevPos.Remove(player.CSteamID);

            if (wasSprinting.ContainsKey(player.CSteamID))
                wasSprinting.Remove(player.CSteamID);
        }
    }
}
