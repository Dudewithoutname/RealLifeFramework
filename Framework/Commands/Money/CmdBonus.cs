using RealLifeFramework.Ranks;
using RealLifeFramework.RealPlayers;
using Rocket.API;
using Steamworks;
using System;
using System.Collections.Generic;
using SDG.Unturned;
using UnityEngine;
/*
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

        private Dictionary<CSteamID, DateTime> salary = new Dictionary<CSteamID, DateTime>();

        public void Execute(IRocketPlayer caller, string[] command)
        {
            var player = RealPlayer.From(caller);

            if (salary.ContainsKey(player.CSteamID) && DateTime.Compare(salary[player.CSteamID], DateTime.Now) > -3600)
            {
                ChatManager.say(player.CSteamID, $"Vyplatu si mozes davat kazdu 1h", Color.red, EChatMode.SAY, true);
                return;
            }

            if (player.RankUser.Job.Id != "unemployed" && player.RankUser.Job != null) 
            {
                var newSalary = getSalary(player.RankUser.Job.Id);

                if (newSalary > 0)
                {
                    player.CreditCardMoney += newSalary;
                    salary.Add(player.CSteamID, DateTime.Now);
                    ChatManager.say(player.CSteamID, $"Obdrzal si vyplatu {Currency.FormatMoney(newSalary.ToString())} za {player.RankUser.Job.DisplayName}!", Color.white, EChatMode.SAY, true);
                }
                else
                {
                    ChatManager.say(player.CSteamID, $"Praca {player.RankUser.Job.DisplayName}, nema ziadnu vyplatu", Color.red, EChatMode.SAY, true);
                }
            }
            else
            {
                ChatManager.say(player.CSteamID, $"Nepracujes, jak chces dostat vyplatu?", Color.red, EChatMode.SAY, true);
            }
        }

        private static uint getSalary(byte jobId)
        {
            switch ()
            {
                //  PD

                case "pd_kadet":
                    return 4000;

                case "pd_strazmajster":
                    return 5000;

                case "pd_nadstrazmajster":
                    return 7500;

                case "pd_praporcik":
                    return 9500;
            }
        }
    }
}*/
