using RealLifeFramework.Ranks;
using RealLifeFramework.RealPlayers;
using RealLifeFramework.Skills;
using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealLifeFramework.Commands
{
    public class Skills : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "skills";

        public string Help => "skills";

        public string Syntax => "/skills";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string> { RankManager.PlayerPermission };

        public void Execute(IRocketPlayer caller, string[] command)
        {
           SkillDisplay.SendOpenUI(RealPlayer.From(caller));
        }
    }
}
