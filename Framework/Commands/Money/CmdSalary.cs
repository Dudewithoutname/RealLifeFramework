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
    public class CmdSalary : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "salary";

        public string Help => "salary";

        public string Syntax => "/salary";

        public List<string> Aliases => new List<string>() { "vyplata" };

        public List<string> Permissions => new List<string> { RankManager.PlayerPermission };

        private Dictionary<CSteamID, DateTime> salary = new Dictionary<CSteamID, DateTime>();

        public void Execute(IRocketPlayer caller, string[] command)
        {
            var player = RealPlayer.From(caller);

            if (salary.ContainsKey(player.CSteamID) && DateTime.Now < salary[player.CSteamID].AddHours(1))
            {
                ChatManager.say(player.CSteamID, $"Vyplatu si mozes davat kazdu 1h", Color.red, EChatMode.SAY, true);
                return;
            }

            if (player.RankUser.Job.Id != "unemployed" && player.RankUser.Job != null) 
            {
                var newSalary = getSalary(player.RankUser.Job.Id);

                if (newSalary > 0)
                {
                    if (salary.ContainsKey(player.CSteamID)) salary.Remove(player.CSteamID);
                    salary.Add(player.CSteamID, DateTime.Now);

                    player.CreditCardMoney += newSalary;
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

        private static uint getSalary(string jobId)
        {
            switch (jobId)
            {
                //  PD

                case "pd_kadet":
                    return 4000;

                case "pd_strazmajster":
                    return 5000;

                case "pd_nadstrazmajster":
                    return 6000;

                case "pd_praporcik":
                    return 7500;

                case "pd_porucik":
                    return 10000;

                case "pd_major":
                    return 12000;

                case "pd_sef":
                    return 13000;

                //  EMS

                case "ems_zachranar":
                    return 4000;

                case "ems_zdravotnik":
                    return 6000;

                case "ems_doktor":
                    return 8000;

                case "ems_sef":
                    return 10000;

                // Custom

                case "kebab":
                    return 1000;

                case "vinar":
                    return 500;

                case "amazon":
                    return 500;

                default: 
                    return 0;
            }
        }
       
    }
}
