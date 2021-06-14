using Steamworks;
using Rocket.Unturned.Player;
using Rocket.API;
using SDG.Unturned;
using RealLifeFramework.UserInterface;
using System.Collections.Generic;
using RealLifeFramework.Chatting;

namespace RealLifeFramework.RealPlayers
{
    public static class RealPlayerManager
    {
        public static void InitializePlayer(UnturnedPlayer player)
        {
            DBPlayerResult PlayerResult = TPlayerInfo.GetPlayer(player.CSteamID);
            bool isNew = (PlayerResult == null) ? true : false;

            if (isNew)
            {

                if(RealLife.Debugging)
                    RealLife.Instance.RealPlayers.Add(player.CSteamID, new RealPlayer(player, "Matthew Creampie", 20, 0));
                else
                    firstJoin(player);

            }
            else
            {
                RealLife.Instance.RealPlayers.Add(player.CSteamID, new RealPlayer(player, PlayerResult));
            }

            VoiceChat.Subscribe(RealLife.Instance.RealPlayers[player.CSteamID]);
        }

        public static void HandleDisconnect(UnturnedPlayer player)
        {
            if (RealLife.Instance.RealPlayers.ContainsKey(player.CSteamID))
            {
                VoiceChat.UnSubscribe(RealLife.Instance.RealPlayers[player.CSteamID]);
                RealLife.Instance.RealPlayers[player.CSteamID].Keyboard.Stop();
                RealLife.Instance.RealPlayers.Remove(player.CSteamID);
            }
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
            player.GodMode = true;
            player.VanishMode = true;

            EffectManager.sendUIEffect(UI.StartingTab, 100, player.Player.channel.GetOwnerTransportConnection(), true, topScreenText, midScreenText, "Create Character");
            EffectManager.sendUIEffectImageURL(100, player.Player.channel.GetOwnerTransportConnection(), true, "steampfp", player.SteamProfile.AvatarMedium.ToString());
        }

        #region GetRealPlayer
        public static RealPlayer GetRealPlayer(CSteamID csteamid)
        {
            if (RealLife.Instance.RealPlayers.ContainsKey(csteamid))
                return RealLife.Instance.RealPlayers[csteamid];
            else
                return null;
        }

        public static RealPlayer GetRealPlayer(UnturnedPlayer player)
        {
            if (RealLife.Instance.RealPlayers.ContainsKey(player.CSteamID))
                return RealLife.Instance.RealPlayers[player.CSteamID];
            else
                return null;
        }

        public static RealPlayer GetRealPlayer(IRocketPlayer player)
        {
            var p = (UnturnedPlayer)player;

            if (RealLife.Instance.RealPlayers.ContainsKey(p.CSteamID))
                return RealLife.Instance.RealPlayers[p.CSteamID];
            else
                return null;

        }

        public static RealPlayer GetRealPlayer(Player player)
        {
            CSteamID p = player.channel.owner.playerID.steamID;

            if (RealLife.Instance.RealPlayers.ContainsKey(p))
                return RealLife.Instance.RealPlayers[p];
            else
                return null;
        }
        #endregion
    }

}
