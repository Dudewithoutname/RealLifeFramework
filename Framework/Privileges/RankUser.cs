using RealLifeFramework.RealPlayers;
using Rocket.API;
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
        public RealPlayer Player;

        public string DisplayIcon;
        public string DisplayPrefix;
        public string DisplayRankName;

        public Rank? Admin;
        public Rank? Vip;

        public RocketPermissionsGroup Job;

        public RankUser(RealPlayer player)
        {
            Player = player;

            var rocketp = UnturnedPlayer.FromCSteamID(player.CSteamID);
            int[] lvl = { -1, -1 };

            Job = R.Permissions.GetGroups(rocketp, false)[0];

            foreach (var vip in RankManager.VIPs)
            {
                if (vip.Level > lvl[1] && rocketp.HasPermission(vip.PermIdentifier))
                {
                    lvl[1] = vip.Level;
                    Vip = vip;
                }
            }

            if (Vip != null)
            {
                DisplayIcon = ((Rank)Vip).Icon;
                DisplayPrefix = ((Rank)Vip).Prefix;
                DisplayRankName = ((Rank)Vip).Name;
            }

            foreach (var admin in RankManager.Admins)
            {
                if (admin.Level > lvl[0] && rocketp.HasPermission(admin.PermIdentifier))
                {
                    lvl[0] = admin.Level;
                    Admin = admin;

                    DisplayIcon = admin.Icon;
                    DisplayPrefix = admin.Prefix;
                    DisplayRankName = admin.Name;
                }
            }

            if (Admin != null)
            {
                DisplayIcon = ((Rank)Admin).Icon;
                DisplayPrefix = ((Rank)Admin).Prefix;
                DisplayRankName = ((Rank)Admin).Name;
            }
        }
    }
}
