using RealLifeFramework.Autobazar;
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
    public class CmdSetCarShop : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "setcarshop";

        public string Help => "setcarshop";

        public string Syntax => "/setcarshop";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string> { RankManager.Admins[4].PermIdentifier };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            var player = (UnturnedPlayer)caller;
            if (command.Length < 1) return;

            if (uint.TryParse(command[0], out var result))
            {
                CarShop.InstanceId = result;
                ChatManager.say(player.CSteamID, $"Carshop new Instance Id result", Color.white, EChatMode.SAY, true);
            }
            else
            {
                ChatManager.say(player.CSteamID, $"Napicu si to napisal", Color.white, EChatMode.SAY, true);
            }
        }
    }
}
