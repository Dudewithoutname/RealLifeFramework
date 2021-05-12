﻿using SDG.Unturned;
using Rocket.Unturned.Player;
using Rocket.Unturned.Events;
using RealLifeFramework.Players;
using RealLifeFramework.Skills;
using RealLifeFramework.Items;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;
using Rocket.Unturned;
using System;

namespace RealLifeFramework.Skills
{
    [EventHandler("SkillManager")]
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
            UseableConsumeable.onConsumePerformed += HandleConsume;
        }

        public static void SendLevelUp(RealPlayer player, int skillId)
        {
            var skill = player.SkillUser.Skills[skillId];
            
            player.AddExp(5);

            if (RealLife.Debugging)
                Logger.Log($"Debug: levelUp {skill.Name} , {skill.Level} , {skill.Exp}");
        }

        public static void HandleStatIncremented(Player player, EPlayerStat stat)
        {
            var rplayer = RealPlayerManager.GetRealPlayer(player);

            switch (stat)
            {
                case EPlayerStat.FOUND_FISHES:
                    rplayer.SkillUser.AddExp(Fishing.Id, 3);
                    break;
                case EPlayerStat.FOUND_PLANTS:
                    rplayer.SkillUser.AddExp(Farming.Id, 3);
                    break;
                case EPlayerStat.FOUND_RESOURCES:
                    rplayer.SkillUser.AddExp(Dexterity.Id, 3);
                    break;
            }
        }

        public static void HandleConsume(Player player, ItemConsumeableAsset consumeableAsset)
        {
            var rplayer = RealPlayerManager.GetRealPlayer(player);

            if (MedicalItems.Ids.Contains(consumeableAsset.id))
                rplayer.SkillUser.AddExp(Endurance.Id, 3);
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

                    var rplayer = RealPlayerManager.GetRealPlayer(player);

                    var distance = (int)Math.Round(Vector3.Distance(prevPos[player.CSteamID], player.Position));
                    uint exp;

                    if (distance > 500)
                    {
                        exp = 50;
                        rplayer.SkillUser.AddExp(Agitily.Id, exp);
                    }
                    if (distance > 30 && distance < 500)
                    {
                        exp = (uint)Math.Floor((decimal)(distance / 10));
                        rplayer.SkillUser.AddExp(Agitily.Id, exp);
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