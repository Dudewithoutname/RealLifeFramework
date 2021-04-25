using System;
using System.Collections.Generic;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using SDG.NetTransport;
using System.Linq;
using System.Text.RegularExpressions;

namespace RealLifeFramework.Players
{
    public static class RealPlayerCreation
    {
        public static Dictionary<CSteamID, PrePlayer> PrePlayers;

        private static string headText = "DudeTurned | Create your dream character";
        private const string regexPattern = @"_?<>./\u000C#-\[\]\{\}()*&^%$#@!;',+`|~"; // kokot na co tu mas ten regex aj tak to nejde

        public static void Load()
        {
            PrePlayers = new Dictionary<CSteamID, PrePlayer>();
            Logger.Log("[CreationManager] Loaded");
        }

        public static void OpenCreation(Player player)
        {
            PrePlayers.Add(player.channel.owner.playerID.steamID, new PrePlayer());

            EffectManager.askEffectClearByID(UI.StartingTab, player.channel.GetOwnerTransportConnection());
            EffectManager.sendUIEffect(UI.CreationTab, 101, true, "DudeTurned | Create your dream character", ""); // 2nd is error text

        }

        public static void SetGender(CSteamID steamId, byte gender)
        {
            ITransportConnection player = UnturnedPlayer.FromCSteamID(steamId).Player.channel.GetOwnerTransportConnection();

            if(gender == 0)
            {
                EffectManager.askEffectClearByID(UI.CreationF, player);
                PrePlayers[steamId].Gender = 0;
                EffectManager.sendUIEffect(UI.CreationM, 102, true);
            }
            else
            {
                EffectManager.askEffectClearByID(UI.CreationM, player);
                PrePlayers[steamId].Gender = 1;
                EffectManager.sendUIEffect(UI.CreationF, 102, true);
            }
        }

        public static void ValidateCharacter(CSteamID steamId)
        {
            ITransportConnection playerCon = UnturnedPlayer.FromCSteamID(steamId).Player.channel.GetOwnerTransportConnection();
            Player player = PlayerTool.getPlayer(steamId);

            if (!validateName(0, PrePlayers[steamId].FirstName, playerCon))
                return;

            if (!validateName(1, PrePlayers[steamId].LastName, playerCon))
                return;

            if (!validateAge(PrePlayers[steamId].Age, playerCon))
                return;

            if (PrePlayers[steamId].Gender == null)
            {
                EffectManager.sendUIEffect(UI.CreationTab, 101, playerCon, true, headText, "Error : Please select gender");
                return;
            }
            
            RealLife.Instance.RealPlayers.Add(steamId, new RealPlayer(UnturnedPlayer.FromPlayer(player), PrePlayers[steamId].GetFullName(), (ushort)PrePlayers[steamId].Age, (byte)PrePlayers[steamId].Gender));
            PrePlayers.Remove(steamId);

            player.setPluginWidgetFlag(EPluginWidgetFlags.ForceBlur, false);
            player.setPluginWidgetFlag(EPluginWidgetFlags.Modal, false);
            EffectManager.askEffectClearByID(UI.CreationTab, playerCon);
            EffectManager.askEffectClearByID(UI.CreationF, playerCon);
            EffectManager.askEffectClearByID(UI.CreationM, playerCon);

        }

        private static bool validateAge(byte? age, ITransportConnection player)
        {
            if(age == null)
            {
                EffectManager.sendUIEffect(UI.CreationTab, 101, player, true, headText, "Error : Bad age");
                return false;
            }
            else if (age < 18)
            {
                EffectManager.sendUIEffect(UI.CreationTab, 101, player, true, headText, "Error : Minimal age is 18");
                return false;
            }
            else if(age > 75)
            {
                EffectManager.sendUIEffect(UI.CreationTab, 101, player, true, headText, "Error : Maximal age is 75");
                return false;
            }
            else
            {
                return true;
            }
        }

        private static bool validateName(byte type, string str, ITransportConnection player)
        {
            Regex regex = new Regex(regexPattern);

            if (str == null)
                return false;

            if (type == 0) // firstname
            {
                if (str.All(char.IsDigit) || str.Contains(' ') || str.Contains('"') || regex.IsMatch(str))
                {
                    EffectManager.sendUIEffect(UI.CreationTab, 101, player, true, headText, "Error : Firstname contains restrited characters");
                    return false;
                }
                else if (str.Length < 3)
                {
                    EffectManager.sendUIEffect(UI.CreationTab, 101, player, true, headText, "Error : Firstname is too short");
                    return false;
                }
                else if (str.Length > 12)
                {
                    EffectManager.sendUIEffect(UI.CreationTab, 101, player, true, headText, "Error : Firstname is too long");
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else // lastName
            {
                if (str.All(char.IsDigit) || str.Contains(' ') || regex.IsMatch(str))
                {
                    EffectManager.sendUIEffect(UI.CreationTab, 101, player, true, headText, "Error : Lastname contains restrited characters");
                    return false;
                }
                else if (str.Length < 3)
                {
                    EffectManager.sendUIEffect(UI.CreationTab, 101, player, true, headText, "Error : Lastname is too short");
                    return false;
                }
                else if (str.Length > 15)
                {
                    EffectManager.sendUIEffect(UI.CreationTab, 101, player, true, headText, "Error : Lastname is too long");
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }

    public class PrePlayer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte? Age { get; set; }
        public byte? Gender { get; set; }

        public string GetFullName()
        {
            
            return FirstName + " " + LastName;
        }
    }
}
