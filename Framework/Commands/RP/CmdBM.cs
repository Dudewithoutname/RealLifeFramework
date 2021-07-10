using RealLifeFramework.Privileges;
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
            
            foreach (SteamPlayer steamPlayer in Provider.clients)
            {
                var LoopPlayer = PlayerTool.getPlayer(steamPlayer.playerID.steamID);
                float distance = (LoopPlayer.gameObject.transform.position - player.Player.gameObject.transform.position).sqrMagnitude;

                if (distance <= 450)
                {
                    if (!UnturnedPlayer.FromCSteamID(steamPlayer.playerID.steamID).HasPermission(RankManager.PolicePermission))
                    {
                        ChatManager.say(steamPlayer.playerID.steamID, $"<color=#242424><b>Blackmarket > </b></color><color=#cfcfcf> {string.Join(" ", args)} </color>", Palette.COLOR_W, true);
                    }
                }
            }
        }
    }
}
