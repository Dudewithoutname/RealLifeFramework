using System;
using System.Collections.Generic;
using SDG.Unturned;
using Steamworks;
using UnityEngine;

namespace RealLifeFramework.Security
{
    [EventHandler("Security")]
    public class Guard : IEventComponent
    {
        public static Guard Instance;
        /* 
         * TODO
         * DISCORD EMBED SEND ETC.
        */

        private void Load()
        {
            Instance = this;
            Logger.Log("[Guard] Loaded");
        }

        public void HookEvents()
        {
            Load();
            Provider.onCheckBanStatusWithHWID += checkBan;
            Provider.onBanPlayerRequested += doHWIDBan;
            Provider.onUnbanPlayerRequested += doHWIDUnban;
        }

        private void checkBan(SteamPlayerID playerID, uint remoteIP, ref bool isBanned, ref string banReason, ref uint banRemainingDuration)
        {
            string hwid = BitConverter.ToString(playerID.hwid).Replace("-", string.Empty);

            if (RealLife.Database.get(TSecurity.Name, 0, "hwid", hwid) == hwid && isBanned == false)
                Provider.ban(playerID.steamID, $"[GUARD] HWID banned! for more info check discord {RealLife.Instance.Configuration.Instance.DiscordInvite}", banRemainingDuration);
        }

        private void doHWIDBan(CSteamID instigator, CSteamID playerToBan, uint ipToBan, ref string reason, ref uint duration, ref bool shouldVanillaBan)
        {
            Player player = PlayerTool.getPlayer(playerToBan);

            if(player != null)
                TSecurity.AddHWIDBan(BitConverter.ToString(player.channel.owner.playerID.hwid).Replace("-", string.Empty));
            else
                ChatManager.say(instigator, "[GUARD] Warning! since player is offline ban isn't HWID", Color.red, false);
        }

        private void doHWIDUnban(CSteamID instigator, CSteamID playerToUnban, ref bool shouldVanillaUnban)
        {
            Player player = PlayerTool.getPlayer(playerToUnban);
            if(player != null) 
            { 
                string hwid = BitConverter.ToString(player.channel.owner.playerID.hwid).Replace("-", string.Empty);

                if (RealLife.Database.get(TSecurity.Name, 0, "hwid", hwid) == hwid)
                    TSecurity.RemoveHWIDBan(hwid);
            }
        }
    }
}
