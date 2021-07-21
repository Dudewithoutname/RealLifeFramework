using Pathfinding;
using Rocket.API.Serialisation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealLifeFramework.Ranks
{
    public static class RankManager
    {
        // unemployed default job
        public static readonly string PlayerPermission = "dude.player";
        public static readonly string PolicePermission = "dude.jobs.police";
        public static readonly string EMSPermission = "dude.jobs.ems";


        public static readonly List<Rank> VIPs = new List<Rank>()
        {
            new Rank(0, "dude.vip.veteran", "Veteran", string.Empty, ""),
            new Rank(1, "dude.vip.epic", "Epic", "<color=#ad33ff>Epic</color>", "player"),
            new Rank(2, "dude.vip.legend", "Legend", "<color=#3dffc5>Legend</color>", "player"),
            new Rank(3, "dude.vip.mythical", "Mythical", "<color=#fff93d>Mythical</color>", "player")
        };

        public static readonly List<Rank> Admins = new List<Rank>()
        {
            new Rank(0, "dude.admin.zkhelper", "Zk. Helper", "<color=#00FF6E>Zk. Helper</color>", "https://i.ibb.co/n17F91Y/zkshield.png"),
            new Rank(1, "dude.admin.helper", "Helper", "<color=#2BF314>Helper</color>", "https://i.ibb.co/tM33PQj/helshield.png"),
            new Rank(2, "dude.admin.mod", "Mod", "<color=#00edd1>Mod</color>", "player"),
            new Rank(3, "dude.admin.admin", "Admin", "<color=#9661ff>Admin</color>", "player"),
            new Rank(4, "dude.admin.owner", "Owner", "<color=#fb9d8f>Owner</color>", "player")
        };

        public static readonly List<string> PoliceRanks = new List<string>()
        {
            "pd_kadet",
            "pd_strazmajster",
            "pd_nadstrazmajster",
            "pd_praporcik",
            "pd_porucik",
            "pd_major",
            "pd_sef",
        };

        public static readonly List<string> EMSRanks = new List<string>()
        {
            "ems_zachranar",
            "ems_zdravotnik",
            "ems_doktor",
            "ems_sef",
        };

        public static string GetNameByGroups(List<RocketPermissionsGroup> playerGroups)
        {
            if (playerGroups == null) return "Obcan";

            string adminPrefix = string.Empty;
            string vipPrefix = string.Empty;

            int[] lvl = { -1, -1 };

            foreach (var group in playerGroups)
            {
                foreach (var perm in group.Permissions)
                {
                    if (perm.Name.StartsWith("dude.vip"))
                    {
                        foreach (var vip in VIPs)
                        {
                            if (vip.PermIdentifier == perm.Name && vip.Level > lvl[0])
                            {
                                lvl[0] = vip.Level;
                                vipPrefix = vip.Name;
                            }
                        }
                    }

                    if (perm.Name.StartsWith("dude.admin"))
                    {
                        foreach (var admin in Admins)
                        {
                            if (admin.PermIdentifier == perm.Name && admin.Level > lvl[1])
                            {
                                lvl[1] = admin.Level;
                                adminPrefix = admin.Name;
                            }
                        }
                    }
                }
            }

            string result = string.Empty;

            if (lvl[0] != -1)
            {
                result = vipPrefix;
            }

            if (lvl[1] != -1)
            {
                result = adminPrefix;
            }

            return (result != string.Empty) ? result : "Obcan";
        }

        public static int GetVIPLevel(List<RocketPermissionsGroup> playerGroups)
        {
            if (playerGroups == null) return -1;

            int lvl = -1;

            foreach (var group in playerGroups)
            {
                foreach (var perm in group.Permissions)
                {
                    if (perm.Name.StartsWith("dude.vip"))
                    {
                        foreach (var vip in VIPs)
                        {
                            if (vip.PermIdentifier == perm.Name && vip.Level > lvl)
                            {
                                lvl = vip.Level;
                            }
                        }
                    }
                }
            }

            return lvl;
        }

    }
}
