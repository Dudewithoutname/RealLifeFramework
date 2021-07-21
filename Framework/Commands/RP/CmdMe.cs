using RealLifeFramework.Ranks;
using RealLifeFramework.RealPlayers;
using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections.Generic;

namespace RealLifeFramework.Commands
{
    public class CmdMe : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "me";

        public string Help => "me";

        public string Syntax => "/me";

        public List<string> Aliases => new List<string>();

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
                float distance = (LoopPlayer.gameObject.transform.position - player.Player.gameObject.transform.position).sqrMagnitude;

                if (distance <= 450)
                {
                    ChatManager.say(steamPlayer.playerID.steamID, $"<color=#69dba0><b>Me > {player.Name} |</b></color><color=#bce8d1> {txt} </color>", Palette.COLOR_W, true);
                }
            }
        }
    }
}
