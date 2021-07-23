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
    public class CmdShowNames : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "shownames";

        public string Help => "shownames";

        public string Syntax => "/shownames";

        public List<string> Aliases => new List<string>() { "showname" };

        public List<string> Permissions => new List<string> { RankManager.Admins[2].PermIdentifier };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            var player = (UnturnedPlayer)caller;

            player.Player.setPluginWidgetFlag(EPluginWidgetFlags.ShowInteractWithEnemy, true);
            ChatManager.say(player.CSteamID, $"Show Names!", Color.white, EChatMode.SAY, true);
        }
    }
}
