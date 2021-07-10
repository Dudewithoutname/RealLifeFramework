using RealLifeFramework.RealPlayers;
using Rocket.API.Serialisation;
using Rocket.Core;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealLifeFramework.Privileges
{
    public class RankUser
    {
        public string DisplayIcon;

        public Rank Admin;
        public Rank Vip;

        public RocketPermissionsGroup Job;

        public RankUser(RealPlayer player)
        {

            Job = R.Permissions.GetGroups(UnturnedPlayer.FromCSteamID(player.CSteamID), false)[0];
            // switch for police job
        }
    }
}
