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
    public class CmdGetInstanceId : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "getinstanceid";

        public string Help => "getinstanceid";

        public string Syntax => "/getinstanceid";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string> { RankManager.Admins[4].PermIdentifier };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            var player = (UnturnedPlayer)caller;
            if (Physics.Raycast(player.Player.look.aim.position, player.Player.look.aim.forward, out RaycastHit hit, 10f, RayMasks.BARRICADE | RayMasks.BARRICADE_INTERACT))
            {
                if (BarricadeManager.tryGetInfo(hit.transform, out _, out _, out _, out var index, out var region))
                {
                    var barricade = region.barricades[index];
                    if (barricade == null) return;
                    Logger.Log($"{barricade.instanceID}");
                    ChatManager.say(player.CSteamID, $"{barricade.instanceID}", Color.white, EChatMode.SAY, true);
                }
            }
        }
    }
}
