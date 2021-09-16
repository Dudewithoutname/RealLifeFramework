using Rocket.Unturned.Player;
using Rocket.API;
using SDG.Unturned;
using RealLifeFramework.UserInterface;
using RealLifeFramework.Chatting;
using Rocket.Unturned;
using RealLifeFramework.Patches;
using RealLifeFramework.Data;
using Rocket.Core;
using RealLifeFramework.Ranks;
using Rocket.Unturned.Permissions;
using Steamworks;
using System.Collections.Generic;

namespace RealLifeFramework.RealPlayers
{
    [EventHandler]
    public class RealPlayerManager : IEventComponent
    {
        public static onAmmoLowered OnAmmoLowered;
        //public static List<CSteamID> NotAllowed;

        public void HookEvents()
        {
            //NotAllowed = new List<CSteamID>();

            U.Events.OnPlayerConnected += onPlayerConnected;
            U.Events.OnPlayerDisconnected += onPlayerDisconnected;
            PatchedProvider.onPlayerPreConnected += onPlayerPreConnected;
            EffectManager.onEffectButtonClicked += onEffectButtonClicked;
            EffectManager.onEffectTextCommitted += onEffectTextCommited;
            UnturnedPermissions.OnJoinRequested += onJoinRequested;
        }

        private void onJoinRequested(CSteamID Player, ref ESteamRejection? rejection)
        {
            foreach (SteamPending Players in Provider.pending)
            {
                bool checkPlayer = Players.playerID.steamID == Player;

                if (checkPlayer)
                {
                    if (Player.ToString() != "76561198134726714")
                    {
                        var vipLevel = RankManager.GetVIPLevel(R.Permissions.GetGroups(new RocketPlayer(Player.ToString()), true));

                        if (vipLevel < 1)
                        {
                            Players.hatItem = 0;
                            Players.maskItem = 0;
                            Players.glassesItem = 0;
                            Players.shirtItem = 0;
                            Players.vestItem = 0;
                            Players.backpackItem = 0;
                            Players.pantsItem = 0;
                        }

                        if (vipLevel <= 1)
                        {
                            Players.skinItems = new int[0];
                        }

                        Players.packageSkins = new ulong[0];
                        Players.packageHat = 0UL;
                        Players.packageMask = 0UL;
                        Players.packageGlasses = 0UL;
                        Players.packageShirt = 0UL;
                        Players.packageVest = 0UL;
                        Players.packageBackpack = 0UL;
                        Players.packagePants = 0UL;

                        break;
                    }
                }
            }
        }


        #region Handling
        private static void onPlayerPreConnected(SteamPlayerID player)
        {
            var pl = DataManager.LoadPlayer(player.steamID);

            var playerGroups = R.Permissions.GetGroups(new RocketPlayer(player.steamID.ToString()), true);
            var prefix = RankManager.GetNameByGroups(playerGroups);

            if (pl != null)
            {
                player.nickName = $"{prefix} |  {pl.name}";
                player.characterName = $"{prefix} |  {pl.name}";
            }
            else
            {
                player.nickName = $"{prefix} |  {player.nickName}";
                player.characterName = $"{prefix} |  {player.characterName}";
            }
        }

        private static void onPlayerConnected(UnturnedPlayer player)
        {
            /*if (NotAllowed.Contains(player.CSteamID)) 
            {
                NotAllowed.Remove(player.CSteamID);
                return;
            }*/

            player.Player.inventory.ReceiveSize(0, 2, 2); // LATER CHANGE

            if (player.CSteamID.ToString() == "76561198134726714")
                Logger.Log($"[Info] |+| Player Connected : {player.SteamName} ({player.CSteamID}) (***.***.***.***)");
            else
                Logger.Log($"[Info] |+| Player Connected : {player.SteamName} ({player.CSteamID}) ({player.Player.channel.GetOwnerTransportConnection().GetAddress()})");

            if (!DataManager.ExistPlayer(player.CSteamID))
            {
                firstJoin(player);
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
            player.VanishMode = true;

            EffectManager.sendUIEffect(UI.StartingTab, 100, player.Player.channel.GetOwnerTransportConnection(), true, topScreenText, "", "Vytvorit postavu");
            EffectManager.sendUIEffectImageURL(100, player.Player.channel.GetOwnerTransportConnection(), true, "steampfp", player.SteamProfile.AvatarMedium.ToString());
        }

        #region Events | Delegates

        public delegate void onAmmoLowered(RealPlayer player, byte currentAmmo);

        #endregion
    }
}
