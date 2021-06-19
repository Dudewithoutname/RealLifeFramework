using RealLifeFramework.RealPlayers;
using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealLifeFramework.Commands
{
    public class CHideHud : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "hidehud";

        public string Help => "hidehud";

        public string Syntax => "/hidehud";

        public List<string> Aliases => new List<string>() { "hideui", "hudhide", "uihide", "skrytui", "skrythud"};

        public List<string> Permissions => new List<string> { "dude.basic" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            var rp = RealPlayer.From(caller);
            rp.HUD.HideHud();
        }
    }
}
