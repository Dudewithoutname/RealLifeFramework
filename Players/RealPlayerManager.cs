﻿using Steamworks;
using Rocket.Unturned.Player;
using Rocket.API;
using SDG.Unturned;

namespace RealLifeFramework.Players
{
    public static class RealPlayerManager
    {

        public static void InitializePlayer(UnturnedPlayer player)
        {
            DBPlayerResult PlayerResult = RealLife.Database.GetPlayer(player.CSteamID);
            bool isNew = (PlayerResult == null) ? true : false;

            if (isNew)
            {
                firstJoin(player);
                // just add debug
                //RealLife.Instance.RealPlayers.Add(player.CSteamID, new RealPlayer(player, "cigi", 20, 0));
            }
            else
            {
                RealLife.Instance.RealPlayers.Add(player.CSteamID, new RealPlayer(player, PlayerResult));
            }

        }

        public static void HandleDisconnect(UnturnedPlayer player)
        {
            if (RealLife.Instance.RealPlayers.ContainsKey(player.CSteamID))
                RealLife.Instance.RealPlayers.Remove(player.CSteamID);

            if (RealPlayerCreation.PrePlayers.ContainsKey(player.CSteamID))
                RealPlayerCreation.PrePlayers.Remove(player.CSteamID);
        }


        private static void firstJoin(UnturnedPlayer player)
        {
            string topScreenText = $"Welcome <color=#FFA92A>{player.SteamName}</color> to the Dudeturned roleplay";
            string midScreenText = "\n" +
                "Welcome and thanks for joining our server,\n" +
                "Before you start it's necessary to join our discord for rules, essential information and better experience overall.\n" +
                "\n" +
                "Also for more starting tips and late-game information\n" +
                "visit #wiki channel that you can find on our discord aswell.\n";

            player.Player.setPluginWidgetFlag(EPluginWidgetFlags.ForceBlur, true);
            player.Player.setPluginWidgetFlag(EPluginWidgetFlags.Modal, true);

            EffectManager.sendUIEffect(UI.StartingTab, 100, player.Player.channel.GetOwnerTransportConnection(), true, topScreenText, midScreenText, "Create Character");
            EffectManager.sendUIEffectImageURL(100, player.Player.channel.GetOwnerTransportConnection(), true, "steampfp", player.SteamProfile.AvatarMedium.ToString());
        }

        #region GetRealPlayer
        public static RealPlayer GetRealPlayer(CSteamID csteamid)
        {
            return RealLife.Instance.RealPlayers[csteamid];
        }

        public static RealPlayer GetRealPlayer(UnturnedPlayer player)
        {
            return RealLife.Instance.RealPlayers[player.CSteamID];
        }

        public static RealPlayer GetRealPlayer(IRocketPlayer player)
        {
            var p = (UnturnedPlayer)player;
            return RealLife.Instance.RealPlayers[p.CSteamID];
        }

        public static RealPlayer GetRealPlayer(Player player)
        {
            CSteamID p = player.channel.owner.playerID.steamID;
            return RealLife.Instance.RealPlayers[p];
        }
        #endregion
    }

}