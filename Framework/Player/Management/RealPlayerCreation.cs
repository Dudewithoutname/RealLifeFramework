using System;
using System.Collections.Generic;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using SDG.NetTransport;
using System.Linq;
using RealLifeFramework.UserInterface;
using RealLifeFramework.Items;
using RealLifeFramework.SecondThread;

namespace RealLifeFramework.RealPlayers
{
    public static class RealPlayerCreation
    {
        public static Dictionary<CSteamID, PrePlayer> PrePlayers;

        private const string disallowedCharacters = @"_?<>./\#-\[\]\{\}()*&^%$#@!;',-=+`|~"; 

        public static void Load()
        {
            PrePlayers = new Dictionary<CSteamID, PrePlayer>();
            Logger.Log("[CreationManager] Loaded");
        }

        public static void OpenCreation(Player player)
        {
            PrePlayers.Add(player.channel.owner.playerID.steamID, new PrePlayer());

            EffectManager.askEffectClearByID(UI.StartingTab, player.channel.GetOwnerTransportConnection());
            EffectManager.sendUIEffect(UI.CreationTab, 101, true, "DudeTurned | Vytvor si svoju postavu", ""); // 2nd is error text
        }

        public static void CreateCharacter(CSteamID steamId)
        {
            var playerCon = UnturnedPlayer.FromCSteamID(steamId).Player.channel.GetOwnerTransportConnection();
            var player = UnturnedPlayer.FromCSteamID(steamId);

            if (!validateName(0, PrePlayers[steamId].FirstName, playerCon))
                return;

            if (!validateName(1, PrePlayers[steamId].LastName, playerCon))
                return;

            if (!validateAge(PrePlayers[steamId].Age, playerCon))
                return;

            if (PrePlayers[steamId].Gender == null)
            {
                EffectManager.sendUIEffectText(101, playerCon, true, "errorText", "Error : Prosim vyber pohlavie");
                return;
            }
            //
            player.VanishMode = false;

            var RealPlayer = new RealPlayer(UnturnedPlayer.FromPlayer(player.Player), PrePlayers[steamId].GetFullName(), (ushort)PrePlayers[steamId].Age, (byte)PrePlayers[steamId].Gender);

            RealLife.Instance.RealPlayers.Add(steamId, RealPlayer);
            giveStartingItems(player.Player);

            player.Player.setPluginWidgetFlag(EPluginWidgetFlags.ForceBlur, false);
            player.Player.setPluginWidgetFlag(EPluginWidgetFlags.Modal, false);
            EffectManager.askEffectClearByID(UI.CreationTab, playerCon);
            EffectManager.askEffectClearByID(UI.CreationF, playerCon);
            EffectManager.askEffectClearByID(UI.CreationM, playerCon);
            
            PrePlayers.Remove(steamId);

            //Discord.SendNewPlayer(RealPlayer);
        }

        private static void giveStartingItems(Player player)
        {
            SecondaryThread.Execute(() =>
            {
                var uplayer = UnturnedPlayer.FromPlayer(player);
                var realplayer = RealPlayer.From(player);

                uplayer.GiveItem(StartClothes.GetRandomShirt(realplayer.Gender), 1);
                uplayer.GiveItem(StartClothes.GetPantsu(), 1);
                uplayer.Experience += RealLife.Instance.Configuration.Instance.StartingExp;
            });
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

        private static bool validateAge(byte? age, ITransportConnection player)
        {
            if(age == null)
            {
                EffectManager.sendUIEffectText(101, player, true, "errorText", "Error : Nespravny vek");
                return false;
            }
            else if (age < 18)
            {
                EffectManager.sendUIEffectText(101, player, true, "errorText", "Error : Minimalny vek je 18");
                return false;
            }
            else if(age > 75)
            {
                EffectManager.sendUIEffectText(101, player, true, "errorText", "Error : Maximalny vek je 75");
                return false;
            }
            else
            {
                return true;
            }
        }

        private static bool validateName(byte type, string str, ITransportConnection player)
        {

            if (str == null)
                return false;

            if (type == 0) // firstname
            {
                if (containsNumber(str) || str.Contains(' ') || str.Contains('"') || containsBadChar(str))
                {
                    EffectManager.sendUIEffectText(101, player, true, "errorText", "Error : Meno obsahuje nepovolene znaky");
                    return false;
                }
                else if (str == String.Empty)
                {
                    EffectManager.sendUIEffectText(101, player, true, "errorText", "Error : Meno nemoze byt prazdne");
                    return false;
                }
                else if (str.Length < 3)
                {
                    EffectManager.sendUIEffectText(101, player, true, "errorText", "Error : Meno je moc kratke");
                    return false;
                }
                else if (str.Length > 12)
                {
                    EffectManager.sendUIEffectText(101, player, true, "errorText", "Error : Meno je moc dlhe");
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else // lastName
            {
                if (containsNumber(str) || str.Contains(' ') || str.Contains('"') || containsBadChar(str))
                {
                    EffectManager.sendUIEffectText(101, player, true, "errorText", "Error : Priezvisko obsahuje nepovolene znaky");
                    return false;
                }
                else if(str == String.Empty)
                {
                    EffectManager.sendUIEffectText(101, player, true, "errorText", "Error : Priezvisko nemoze byt prazdne");
                    return false;
                }
                else if (str.Length < 3)
                {
                    EffectManager.sendUIEffectText(101, player, true, "errorText", "Error : Priezvisko je moc kratke");
                    return false;
                }
                else if (str.Length > 15)
                {
                    EffectManager.sendUIEffectText(101, player, true, "errorText", "Error : Priezvisko je moc dlhe");
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        private static bool containsNumber(string str) 
        {
            char[] chars = str.ToCharArray();

            foreach(char c in chars)
                if (Char.IsDigit(c))
                    return true;

            return false;
        }
        
        private static bool containsBadChar(string str)
        {
            char[] strChars = str.ToCharArray();
            char[] disallowedChars = disallowedCharacters.ToCharArray();

            foreach (char c in strChars)
                if (disallowedChars.Contains(c))
                    return true;

            return false;
        }

    }

    public class PrePlayer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte? Age { get; set; }
        public byte? Gender { get; set; }

        private static string formatName(string name)
        {
            return Char.ToUpper(name[0]) + name.Remove(0, 1).ToLower();
        }

        public string GetFullName()
        {
            return formatName(FirstName) + " " + formatName(LastName);
        }
    }
}
