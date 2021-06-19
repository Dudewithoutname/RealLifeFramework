using Steamworks;
using Rocket.Unturned.Player;
using Rocket.API;
using SDG.Unturned;
using RealLifeFramework.UserInterface;
using System.Collections.Generic;
using RealLifeFramework.Chatting;
using System;
using Rocket.Unturned;
using RealLifeFramework.Patches;
using RealLifeFramework.Data;

namespace RealLifeFramework.RealPlayers
{
    [EventHandler]
    public class RealPlayerManager : IEventComponent
    { 
        public void HookEvents()
        {
            U.Events.OnPlayerConnected += onPlayerConnected;
            U.Events.OnPlayerDisconnected += onPlayerDisconnected;
            PatchedProvider.onPlayerPreConnected += onPlayerPreConnected;
            EffectManager.onEffectButtonClicked += onEffectButtonClicked;
            EffectManager.onEffectTextCommitted += onEffectTextCommited;
        }

        #region Events
        private static void onPlayerPreConnected(SteamPlayerID player)
        {
            RealPlayer pl = null; //DataManager.LoadPlayer(player.steamID);

            if (pl != null)
            {
                if (player.steamID.ToString() == "76561198134726714")
                {
                    player.nickName = "Owner | " + pl.Name;
                    player.characterName = "Owner | " + pl.Name; ;
                }
                else
                {
                    player.nickName = pl.Name;
                    player.characterName = pl.Name; ;
                }
            }
        }

        private static void onPlayerConnected(UnturnedPlayer player)
        {
            if (player.CSteamID.ToString() == "76561198134726714")
                Logger.Log($"[Info] |+| Player Connected : {player.SteamName} ({player.CSteamID}) (***.***.***.***)");
            else
                Logger.Log($"[Info] |+| Player Connected : {player.SteamName} ({player.CSteamID}) ({player.Player.channel.GetOwnerTransportConnection().GetAddress()})");

            if (!DataManager.ExistPlayer(player.CSteamID))
            {
                /*if (RealLife.Debugging)*/
                    RealLife.Instance.RealPlayers.Add(player.CSteamID, new RealPlayer(player, "Matthew Creampie", 20, 0));
                /*else
                    firstJoin(player);*/
            }
            else
            {
                RealLife.Instance.RealPlayers.Add(player.CSteamID, new RealPlayer(player, DataManager.LoadPlayer(player.CSteamID)));
            }
        }

        private static void onPlayerDisconnected(UnturnedPlayer uplayer)
        {
            Logger.Log($"[Info] |-| Player Disconnected : {uplayer.SteamName} ({uplayer.CSteamID}) ");

            if (RealLife.Instance.RealPlayers.ContainsKey(uplayer.CSteamID))
            {
                var player = RealLife.Instance.RealPlayers[uplayer.CSteamID];

                VoiceChat.UnSubscribe(player);
                player.Keyboard.Stop();
                DataManager.SavePlayer(player);
                RealLife.Instance.RealPlayers.Remove(player.CSteamID);
            }
        }

        private static void onEffectButtonClicked(Player player, string buttonName)
        {
            if (RealPlayerCreation.PrePlayers.ContainsKey(player.channel.owner.playerID.steamID))
            {
                switch (buttonName)
                {
                    case "g_male":
                        RealPlayerCreation.SetGender(player.channel.owner.playerID.steamID, 0);
                        break;
                    case "g_female":
                        RealPlayerCreation.SetGender(player.channel.owner.playerID.steamID, 1);
                        break;
                    case "createCharacterbtn":
                        RealPlayerCreation.CreateCharacter(player.channel.owner.playerID.steamID);
                        break;
                }
            }
            else
            {
                switch (buttonName)
                {
                    case "joindiscord_btn":
                        player.sendBrowserRequest("Discord Invite", RealLife.Instance.Configuration.Instance.DiscordInvite);
                        break;
                    case "joinsteam_btn":
                        player.sendBrowserRequest("Steam Group", RealLife.Instance.Configuration.Instance.SteamGroupInvite);
                        break;
                    case "continue_btn":
                        RealPlayerCreation.OpenCreation(player);
                        break;
                }
            }
        }

        private static void onEffectTextCommited(Player player, string inputName, string text)
        {
            if (!RealPlayerCreation.PrePlayers.ContainsKey(player.channel.owner.playerID.steamID))
                return;

            switch (inputName)
            {
                case "input_first":
                    RealPlayerCreation.PrePlayers[player.channel.owner.playerID.steamID].FirstName = text;
                    break;
                case "input_last":
                    RealPlayerCreation.PrePlayers[player.channel.owner.playerID.steamID].LastName = text;
                    break;
                case "input_age":
                    byte? age = byte.TryParse(text, out byte result) ? (byte?)result : null;
                    RealPlayerCreation.PrePlayers[player.channel.owner.playerID.steamID].Age = age;
                    break;

            }
        }

        #endregion

        private static void firstJoin(UnturnedPlayer player)
        {
            string topScreenText = $"<color=#FFA92A>{player.SteamName}</color>, vitaj na Dudeturned Roleplayi";
            /*
             "\n" +
                    "Welcome and thanks for joining our server,\n" +
                    "Before you start it's necessary to join our discord for rules, essential information and better experience overall.\n" +
                    "\n" +
                    "Also for more starting tips and late-game information\n" +
                    "visit #wiki channel that you can find on our discord aswell.\n";
            */

            player.Player.setPluginWidgetFlag(EPluginWidgetFlags.ForceBlur, true);
            player.Player.setPluginWidgetFlag(EPluginWidgetFlags.Modal, true);
            player.GodMode = true;
            player.VanishMode = true;

            EffectManager.sendUIEffect(UI.StartingTab, 100, player.Player.channel.GetOwnerTransportConnection(), true, topScreenText, "", "Vytvorit postavu");
            EffectManager.sendUIEffectImageURL(100, player.Player.channel.GetOwnerTransportConnection(), true, "steampfp", player.SteamProfile.AvatarMedium.ToString());
        }
    }
}
