using HarmonyLib;
using Newtonsoft.Json;
using RealLifeFramework.API.Models;
using RealLifeFramework.Ranks;
using RealLifeFramework.RealPlayers;
using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections.Generic;

namespace RealLifeFramework.Commands
{
    public class CmdReport : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "report";

        public string Help => "report";

        public string Syntax => "/report";

        public List<string> Aliases => new List<string>() { "nahlasit" };

        public List<string> Permissions => new List<string> { RankManager.PlayerPermission };

        public void Execute(IRocketPlayer caller, string[] args)
        {
            var player = RealPlayer.From(((UnturnedPlayer)caller).CSteamID);

            if (args.Length < 1) return;

            var txt = string.Join(" ", args);

            if (txt.Length < 12) 
            {
                ChatManager.say(player.CSteamID, $"Sprava musi byt aspon 12 pismen dlha!", Palette.COLOR_R, true);
                return;
            }

            Api.Send("/logs/report", JsonConvert.SerializeObject(
                new Report()
                {
                    message = txt,
                    name = $"[{UnturnedPlayer.FromPlayer(player.Player).SteamName}] {player.Name}",
                    steamId = player.CSteamID.ToString(),
                }
            ));

            foreach (SteamPlayer steamPlayer in Provider.clients)
            {
                if (steamPlayer.isAdmin && RealPlayer.From(steamPlayer).RankUser.Admin != null)
                {
                    ChatManager.say(steamPlayer.playerID.steamID, $"Hrac {player.Name} odoslal report na discord!", Palette.COLOR_O, true);
                }
            }
        }
    }
}
