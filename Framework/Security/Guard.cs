using System;
using System.Collections.Generic;
using SDG.Unturned;
using Steamworks;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using Rocket.Unturned;
using UnityEngine;

namespace RealLifeFramework.Security
{
    [EventHandler("Security")]
    public class Guard : IEventComponent
    {

        private static List<CSteamID> BannedPlayers; 
        /* 
         * TODO
         * DISCORD EMBED SEND ETC.
        */

        private void Load()
        {
            BannedPlayers = new List<CSteamID>();
            Logger.Log("[Guard] Loaded");
        }

        public void HookEvents()
        {
            Load();
            Provider.onCheckBanStatusWithHWID += checkBan;
            U.Events.OnPlayerConnected += HandleBan;
            Provider.onBanPlayerRequested += doHWIDBan;
            Provider.onUnbanPlayerRequested += doHWIDUnban;
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

        private void HandleBan(UnturnedPlayer player)
        {
            CSteamID steamid = player.Player.channel.owner.playerID.steamID;

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
}
