using RealLifeFramework.Ranks;
using RealLifeFramework.RealPlayers;
using Rocket.API;
using Steamworks;
using System;
using System.Collections.Generic;
using SDG.Unturned;

namespace RealLifeFramework.Commands
{
    public class CmdVyplata : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "vyplata";

        public string Help => "vyplata";

        public string Syntax => "/vyplata";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string> { RankManager.PlayerPermission };

        private Dictionary<CSteamID, DateTime> salary = new Dictionary<CSteamID, DateTime>();

        public void Execute(IRocketPlayer caller, string[] command)
        {
            var player = RealPlayer.From(caller);

            if (salary.ContainsKey(player.CSteamID) && DateTime.Compare(salary[player.CSteamID], DateTime.Now) > -3600)
            {
                return;
            }

            if (player.RankUser.Job.Id != "unemployed" && player.RankUser.Job != null) 
            {
                var vyplataTyKokot = getSalary(player.RankUser.Job.Id);

                if (vyplataTyKokot > 0)
                {
                    player.CreditCardMoney += vyplataTyKokot;
                    salary.Add(player.CSteamID, DateTime.Now);
                    ChatManager.say(player.CSteamID, $"Obdrzal si vyplatu {Currency.FormatMoney(vyplataTyKokot.ToString())} za {player.RankUser.Job.DisplayName}!", Palette.COLOR_W, EChatMode.SAY, true);
                }
            }
        }

        private static uint getSalary(string jobId)
        {
            switch (jobId)
            {
                //  PD

                case "pd_kokot1":
                    return 6000;

                case "pd_kokot2":
                    return 8000;

                case "pd_kokot3":
                    return 12000;

                case "pd_sef":
                    return 10000;

                //  EMS

                case "ems_lekarnik":
                    return 4000;

                case "ems_zdravotnik":
                    return 6000;

                case "ems_lekar":
                    return 8000;

                case "ems_sef":
                    return 10000; 

                default: 
                    return 0;
            }
        }
    }
}
