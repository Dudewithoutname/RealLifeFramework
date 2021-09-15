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
        //private static List<CSteamID> playersToBan;

        public void HookEvents()
        {
            /* 
            playersToBan = new List<CSteamID>();
            Provider.onEnemyConnected += onEnemyConnected;
            Provider.onCheckBanStatusWithHWID += checkBan;
            */

            Provider.onBanPlayerRequested += onPlayerBanned;
        }

        private void onPlayerBanned(CSteamID instigator, CSteamID playerToBan, uint ipToBan, ref string reason, ref uint duration, ref bool shouldVanillaBan)
        {
            var player = DataManager.LoadPlayer(playerToBan);
            var admin = UnturnedPlayer.FromCSteamID(instigator);
            string name = (admin != null)? admin.DisplayName : "Console";

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

        // HWID BANS
        /*
        private void onEnemyConnected(SteamPlayer player)
        {
            if (playersToBan.Contains(player.playerID.steamID))
            {
                Provider.ban(player.playerID.steamID, $"[GUARD] HWID banned! for more info check discord {RealLife.Instance.Configuration.Instance.DiscordInvite}", 999999999);
                playersToBan.Remove(player.playerID.steamID);
            }
        }

        private void checkBan(SteamPlayerID playerID, uint remoteIP, ref bool isBanned, ref string banReason, ref uint banRemainingDuration)
        {
            Helper.ExecuteAsync(async () =>
            {
                var hwid = BitConverter.ToString(playerID.hwid).Replace("-", string.Empty);
                var isHwidBanned = await GuardTable.Singleton.CheckHWIDBan(playerID.steamID.ToString(), hwid);

                if (isHwidBanned == EGuardBanResult.True) 
                {
                    if (isBanned) return;

                    Logger.Log($"[Guard] The banned player {playerID.steamID} tried to get around ban !");
                    playersToBan.Add(playerID.steamID);
                }
                else if(isHwidBanned == EGuardBanResult.ToUnban)
                {
                    isBanned = false;
                }
            });
        }
        */
    }
}