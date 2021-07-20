using HarmonyLib;
using RealLifeFramework.Ranks;
using RealLifeFramework.RealPlayers;
using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections.Generic;

namespace RealLifeFramework.Commands
{
    public class CmdPomoc : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "pomoc";

        public string Help => "pomoc";

        public string Syntax => "/pomoc";

        public List<string> Aliases => new List<string>() { "emergency" };

        public List<string> Permissions => new List<string> { RankManager.PlayerPermission };

        public void Execute(IRocketPlayer caller, string[] args)
        {
            var player = RealPlayer.From(((UnturnedPlayer)caller).CSteamID);

            if (args.Length < 1) return;
            if (string.Join(" ", args).Length < 2) return;

            ChatManager.say(player.CSteamID, $"<color=#42f59e><b>Tiesnova Linka | <color=#bdffde>TY</color></b> :</color><color=#ffffff> {string.Join(" ", args)} </color>", Palette.COLOR_W, true);
            foreach (SteamPlayer steamPlayer in Provider.clients)
            {
                var LoopPlayer = PlayerTool.getPlayer(steamPlayer.playerID.steamID);

                if (LoopPlayer.channel.owner.isAdmin && !RankManager.PolicePermission.Contains(RealPlayer.From(LoopPlayer).RankUser.Job.Id)) continue;

                if (UnturnedPlayer.FromCSteamID(steamPlayer.playerID.steamID).HasPermission(RankManager.PolicePermission))
                {
                    ChatManager.say(player.CSteamID, $"<color=#ff595f><b>Tiesnova Linka |</b> Obcan <color=#ffa6a9>{player.Name}</color> Potrebuje Pomoc :</color><color=#ffa6a9> {string.Join(" ", args)} </color>", Palette.COLOR_W, true);
                }
            }
        }
    }
}
