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
    public class CmdReloadCarShop : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "reloadcarshop";

        public string Help => "reloadcarshop";

        public string Syntax => "/reloadcarshop";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string> { RankManager.Admins[4].PermIdentifier };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            var player = (UnturnedPlayer)caller;
            Autobazar.CarShop.ReloadCarShop();
            ChatManager.say(player.CSteamID, $"Reloaded", Color.white, EChatMode.SAY, true);

        }
    }
}
