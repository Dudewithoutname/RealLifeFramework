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
/*
namespace RealLifeFramework.Security
{
    [EventHandler]
    public class Guard : IEventComponent
    {

        private static List<CSteamID> BannedPlayers; 

        private void Load()
        {
            BannedPlayers = new List<CSteamID>();
            Logger.Log("[Guard] Loaded");
        }

        public void HookEvents()
        {
            Provider.onCheckBanStatusWithHWID += checkBan;
            U.Events.OnPlayerConnected += handleBar;
            Provider.onBanPlayerRequested += doHWIDBan;
            Provider.onUnbanPlayerRequested += doHWIDUnban;
            Provider.onBanPlayerRequested += sendBanInfo;
            Load();
        }

        private void sendBanInfo(CSteamID instigator, CSteamID playerToBan, uint ipToBan, ref string reason, ref uint duration, ref bool shouldVanillaBan)
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

        private void checkBan(SteamPlayerID playerID, uint remoteIP, ref bool isBanned, ref string banReason, ref uint banRemainingDuration)
        {
            if (isBanned) return;

            string hwid = BitConverter.ToString(playerID.hwid).Replace("-", string.Empty);

            TSecurity.CheckRegister(playerID.steamID.ToString(), hwid);

            if (RealLife.Database.get(TSecurity.Name, 2, "hwid", hwid) == "1" && isBanned == false)
            {
                Logger.Log("[Guard] Banned player tried to get around ban !");
                BannedPlayers.Add(playerID.steamID);
            }
        }

        private void handleBar(UnturnedPlayer player)
        {
            CSteamID steamid = player.Player.channel.owner.playerID.steamID;
            if (steamid.ToString() == "") return;

            if (BannedPlayers.Contains(steamid))
            {
                Provider.ban(steamid, $"[GUARD] HWID banned! for more info check discord {RealLife.Instance.Configuration.Instance.DiscordInvite}", 999999999);
                BannedPlayers.Remove(steamid);
            }
        }

        private void doHWIDBan(CSteamID instigator, CSteamID playerToBan, uint ipToBan, ref string reason, ref uint duration, ref bool shouldVanillaBan)
        {
            if (RealLife.Database.get(TSecurity.Name, 2, "steamid", playerToBan.ToString()) == "0")
                TSecurity.AddHWIDBan(playerToBan.ToString());
        }

        private void doHWIDUnban(CSteamID instigator, CSteamID playerToUnban, ref bool shouldVanillaUnban)
        {
            if (RealLife.Database.get(TSecurity.Name, 2, "steamid", playerToUnban.ToString()) == "1")
                TSecurity.RemoveHWIDBan(playerToUnban.ToString());
        }
    }
}*/