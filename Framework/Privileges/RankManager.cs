using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealLifeFramework.Privileges
{
    public static class RankManager
    {
        public static readonly string PlayerPermission = "dude.player";
        public static readonly string PolicePermission = "dude.jobs.police";
        public static readonly string EMSPermission = "dude.jobs.ems";


        public static readonly List<Rank> VIPs = new List<Rank>()
        {
            new Rank(0, "dude.vip.veteran", "Veteran", "", "player"),
            new Rank(1, "dude.vip.epic", "Epic", "<color=#ad33ff><b>Epic</b></color>", "player"),
            new Rank(2, "dude.vip.legend", "Legend", "<color=#3dffc5><b>Legend</b></color>", "player"),
            new Rank(3, "dude.vip.mythical", "Mythical", "<color=#fff93d><b>Mythical</b></color>", "player")
        };

        public static readonly List<Rank> Admins = new List<Rank>()
        {
            new Rank(0, "dude.admin.zkhelper", "Zk. Helper", "<color=#00FF6E><b>Zk. Helper</b></color>", "https://i.ibb.co/n17F91Y/zkshield.png"),
            new Rank(1, "dude.admin.helper", "Helper", "<color=#2BF314><b>Helper</b></color>", "https://i.ibb.co/tM33PQj/helshield.png"),
            new Rank(2, "dude.admin.mod", "Mod", "<color=#00edd1><b>Mod</b></color>", "player"),
            new Rank(3, "dude.admin.admin", "Admin", "<color=#9661ff><b>Admin</b></color>", "player"),
            new Rank(4, "dude.admin.owner", "Owner", "<color=#fb9d8f><b>Owner</b></color>", "player")
        };
    }
}
