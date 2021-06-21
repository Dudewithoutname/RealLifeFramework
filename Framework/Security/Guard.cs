using System;
using System.Collections.Generic;
using SDG.Unturned;
using Steamworks;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using Rocket.Unturned;
using UnityEngine;
using RealLifeFramework.Data;
using RealLifeFramework.Framework.Data;
using RealLifeFramework.SecondThread;
using MySql.Data.MySqlClient;

namespace RealLifeFramework.Security
{
    [EventHandler]
    public class Guard : IEventComponent
    {

        private static List<CSteamID> bannedPlayers;
        /* 
         * TODO
         * DISCORD EMBED SEND ETC.
        */
        public void HookEvents()
        {
            bannedPlayers = new List<CSteamID>();
            Provider.onCheckBanStatusWithHWID += checkBan;
            U.Events.OnPlayerConnected += handleBans;
            Provider.onBanPlayerRequested += doHWIDBan;
            Provider.onUnbanPlayerRequested += doHWIDUnban;
            Logger.Log("[Guard] Loaded");
        }

        private void checkBan(SteamPlayerID playerID, uint remoteIP, ref bool isBanned, ref string banReason, ref uint banRemainingDuration)
        {
            bool banned = isBanned;

            SecondaryThread.Execute(() =>
            {
                if (banned) return;

                string hwid = BitConverter.ToString(playerID.hwid).Replace("-", string.Empty);
                var get = Database.Instance.get("security", 2, "hwid", hwid);

                if (get == "1" && !banned)
                {
                    Logger.Log("[Guard] Banned player tried to get around ban !");
                    bannedPlayers.Add(playerID.steamID);
                    return;
                }

                if(get == null && !banned)
                {
                    new MySqlCommand($"INSERT INTO security (hwid, steamid) VALUES ('{hwid}','{playerID.steamID}')", Database.Instance.Connection).ExecuteNonQuery();
                }
            });
        }

        private void handleBans(UnturnedPlayer player)
        {
            SecondaryThread.Execute(() =>
            {
                CSteamID steamid = player.Player.channel.owner.playerID.steamID;

                if (bannedPlayers.Contains(steamid))
                {
                    Provider.ban(steamid, $"[GUARD] HWID banned! for more info check discord {RealLife.Instance.Configuration.Instance.DiscordInvite}", 999999999);
                    bannedPlayers.Remove(steamid);
                }
            });
        }

        private void doHWIDBan(CSteamID instigator, CSteamID playerToBan, uint ipToBan, ref string reason, ref uint duration, ref bool shouldVanillaBan)
        {
            SecondaryThread.Execute(() =>
            {
                if (Database.Instance.get("security", 2, "steamid", playerToBan.ToString()) == "0")
                {
                    Database.Instance.set("security", "steamid", playerToBan.ToString(), "ban", "1");
                }
            });
        }

        private void doHWIDUnban(CSteamID instigator, CSteamID playerToUnban, ref bool shouldVanillaUnban)
        {
            SecondaryThread.Execute(() =>
            {
                if (Database.Instance.get("security", 2, "steamid", playerToUnban.ToString()) == "1")
                {
                    Database.Instance.set("security", "steamid", playerToUnban.ToString(), "ban", "0");
                }
            });
        }
    }
}
