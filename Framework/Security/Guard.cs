using System;
using System.Collections.Generic;
using SDG.Unturned;
using Steamworks;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using Rocket.Unturned;
using UnityEngine;
using SDG.Framework.IO.Deserialization;
using RealLifeFramework.API.Models;
using Newtonsoft.Json;
using RealLifeFramework.Data;
using RealLifeFramework.Data.Models;
using RealLifeFramework.Threadding;

namespace RealLifeFramework.Security
{
    [EventHandler]
    public class Guard : IEventComponent
    {
        private static List<CSteamID> playersToBan;

        public void HookEvents()
        {
             
            playersToBan = new List<CSteamID>();
            Provider.onEnemyConnected += onEnemyConnected;
            Provider.onCheckBanStatusWithHWID += checkBan;

            Provider.onBanPlayerRequested += onPlayerBanned;
            Provider.onUnbanPlayerRequested += onPlayerUnbanned;
        }

        private void onPlayerBanned(CSteamID instigator, CSteamID playerToBan, uint ipToBan, ref string reason, ref uint duration, ref bool shouldVanillaBan)
        {
            var player = DataManager.LoadPlayer(playerToBan);
            var admin = UnturnedPlayer.FromCSteamID(instigator);
            string name = (admin != null)? admin.DisplayName : "Console";
            GuardTable.Singleton.AddHWIDBan(playerToBan.ToString(), (int)duration);

            Api.Send("/info/bans", JsonConvert.SerializeObject(
                new Ban() 
                {
                     characterName = (player != null)? player.name : string.Empty,
                     provider = name,
                     reason = reason,
                     time = (int)duration,
                     steamId = playerToBan.ToString(),
                }
            ));;
        }

        private void onPlayerUnbanned(CSteamID instigator, CSteamID playerToUnban, ref bool shouldVanillaUnban)
        {
            GuardTable.Singleton.RemoveHWIDBan(playerToUnban.ToString());
            Logger.Log($"[Guard] Successfuly unbaned player {playerToUnban}!");
        }

        private void onEnemyConnected(SteamPlayer player)
        {
            if (playersToBan.Contains(player.playerID.steamID))
            {
                Provider.ban(player.playerID.steamID, $"[GUARD] HWID banned! for more info check discord {RealLife.Instance.Configuration.Instance.DiscordInvite}", 2678400);
                playersToBan.Remove(player.playerID.steamID);
            }
        }

        private void checkBan(SteamPlayerID playerID, uint remoteIP, ref bool isBanned, ref string banReason, ref uint banRemainingDuration)
        {
            if (isBanned) return;

            Helper.ExecuteAsync(async () =>
            {
                var hwid = BitConverter.ToString(playerID.hwid).Replace("-", string.Empty);
                var hwidBanResult = await GuardTable.Singleton.CheckHWIDBan(playerID.steamID.ToString(), hwid);                

                if (hwidBanResult == EGuardBanResult.True) 
                {
                    Logger.Log($"[Guard] a banned player {playerID.steamID} tried to get around ban !");
                    playersToBan.Add(playerID.steamID);
                }
                else if (hwidBanResult == EGuardBanResult.ToUnban)
                {
                    Logger.Log($"[Guard] Successfuly unbaned player {playerID.steamID}!");
                }
            });
        }
    }
}