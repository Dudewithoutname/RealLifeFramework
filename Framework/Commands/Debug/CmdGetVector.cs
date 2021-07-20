using RealLifeFramework.Ranks;
using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RealLifeFramework.Commands
{
    public class CmdGetVector : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "getvector";

        public string Help => "getvector";

        public string Syntax => "/getvector";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string> { RankManager.Admins[4].PermIdentifier };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            var player = (UnturnedPlayer)caller;
            Logger.Log($"{player.Player.transform.position}");
            ChatManager.say(player.CSteamID, $"Vec3 {player.Player.transform.position}", Color.white, EChatMode.SAY, true);

        }
    }
}
