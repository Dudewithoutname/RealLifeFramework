using HarmonyLib;
using RealLifeFramework.Ranks;
using RealLifeFramework.RealPlayers;
using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections.Generic;

namespace RealLifeFramework.Commands
{
    public class CmdBM : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "bm";

        public string Help => "bm";

        public string Syntax => "/bm";

        public List<string> Aliases => new List<string>() { "blackmarket" };

        public List<string> Permissions => new List<string> { RankManager.PlayerPermission };

        public void Execute(IRocketPlayer caller, string[] args)
        {
            var player = RealPlayer.From(((UnturnedPlayer)caller).CSteamID);

            if (args.Length < 1) return;

            var txt = string.Join(" ", args);
            if (txt.Length < 2) return;
            if (txt.Contains("<")) txt.Replace("<", "(");

            foreach (SteamPlayer steamPlayer in Provider.clients)
            {
                var LoopPlayer = PlayerTool.getPlayer(steamPlayer.playerID.steamID);

                if (LoopPlayer.channel.owner.isAdmin)
                {
                    ChatManager.say(steamPlayer.playerID.steamID, $"<color=#242424><b>Blackmarket > (<color=#6efdff>{player.Name}</color>)</b></color><color=#cfcfcf> {string.Join(" ", args)} </color>", Palette.COLOR_W, true);
                    continue;
                }

                if (!UnturnedPlayer.FromCSteamID(steamPlayer.playerID.steamID).HasPermission(RankManager.PolicePermission))
                {
                    ChatManager.say(steamPlayer.playerID.steamID, $"<color=#242424><b>Blackmarket > </b></color><color=#cfcfcf> {txt} </color>", Palette.COLOR_W, true);
                }
            }
        }
    }
}
