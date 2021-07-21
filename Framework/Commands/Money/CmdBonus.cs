using RealLifeFramework.Ranks;
using RealLifeFramework.RealPlayers;
using Rocket.API;
using Steamworks;
using System;
using System.Collections.Generic;
using SDG.Unturned;
using UnityEngine;

namespace RealLifeFramework.Commands
{
    public class CmdBonus : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "bonus";

        public string Help => "bonus";

        public string Syntax => "/bonus";

        public List<string> Aliases => new List<string>() { "vipbonus" };

        public List<string> Permissions => new List<string> { RankManager.PlayerPermission };

        private Dictionary<CSteamID, DateTime> bonus = new Dictionary<CSteamID, DateTime>();

        public void Execute(IRocketPlayer caller, string[] command)
        {
            var player = RealPlayer.From(caller);

            if (bonus.ContainsKey(player.CSteamID) && DateTime.Now < bonus[player.CSteamID].AddHours(3))
            {
                ChatManager.say(player.CSteamID, $"VIP Bonus si mozes dat 1x za 3h", Color.red, EChatMode.SAY, true);
                return;
            }

            if (player.RankUser.Vip != null)
            {
                var newBonus = getBonus(player.RankUser.Vip.Value.Level);

                if (newBonus > 0)
                {
                    if (bonus.ContainsKey(player.CSteamID)) bonus.Remove(player.CSteamID);
                    bonus.Add(player.CSteamID, DateTime.Now);

                    player.CreditCardMoney += newBonus;
                    ChatManager.say(player.CSteamID, $"Obdrzal si bonus {Currency.FormatMoney(newBonus.ToString())} za {player.RankUser.Vip.Value.Prefix}!", Color.white, EChatMode.SAY, true);
                }
                else
                {
                    ChatManager.say(player.CSteamID, $"Pre tvoje vip nie je ziaden bonus", Color.red, EChatMode.SAY, true);
                }
            }
            else
            {
                ChatManager.say(player.CSteamID, $"Nejsi VIP.", Color.red, EChatMode.SAY, true);
            }
        }

        private static uint getBonus(byte level)
        {
            switch (level)
            {
                case 1:
                    return 5000;

                case 2:
                    return 10000;

                case 3:
                    return 15000;
            }

            return 0;
        }
    }
}
