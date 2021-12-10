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

            var txt = string.Join(" ", args);
            if (txt.Length < 2) return;
            if (txt.Contains("<")) txt.Replace("<", "(");

            ChatManager.say(player.CSteamID, $"<color=#42f59e><b>Tiesnova Linka | Odoslal si pomoc :</color><color=#ffffff> {string.Join(" ", args)} </color>", Palette.COLOR_W, true);
            foreach (var steamPlayer in Provider.clients)
            {
                var LoopPlayer = PlayerTool.getPlayer(steamPlayer.playerID.steamID);

                if (LoopPlayer.channel.owner.isAdmin && !RankManager.PolicePermission.Contains(RealPlayer.From(LoopPlayer).RankUser.Job.Id)) continue;

                if (UnturnedPlayer.FromCSteamID(steamPlayer.playerID.steamID).HasPermission(RankManager.PolicePermission))
                {
                    steamPlayer.player.quests.sendSetMarker(true, player.Player.transform.position);
                    ChatManager.say(player.CSteamID, $"<color=#ff595f><b>Tiesnova Linka |</b> Obcan <color=#ffa6a9>{player.Name}</color> potrebuje pomoc a je oznaceny na mape </color>", Palette.COLOR_W, true);
                    ChatManager.say(player.CSteamID, $"<color=#ff595f><b>Tiesnova Linka |</b> Sprava : <color=#ffa6a9> {txt} </color>", Palette.COLOR_W, true);
                }
            }
        }
    }
}
