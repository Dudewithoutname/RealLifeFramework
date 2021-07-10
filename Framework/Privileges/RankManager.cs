using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealLifeFramework.Privileges
{
    public static class RankManager
    {
        public static readonly Dictionary<string, Rank> VIPs = new Dictionary<string, Rank>()
        {
            { "vip.epic", new Rank(0, "vip.epic", "Epic", "<color=#ad33ff><b>Epic</b></color>", "player") }, // 5 euro
            { "vip.legend", new Rank(1, "vip.legend", "Legend", "<color=#3dffc5><b>Legend</b></color>", "player") }, // 10 euro
            { "vip.mythical", new Rank(2, "vip.mythical", "Mythical", "<color=#fff93d><b>Mythical</b></color>", "player") }, // 25 euro
        };

        public static readonly Dictionary<string, Rank> Admins = new Dictionary<string, Rank>()
        {
            { "admin.zkhelper", new Rank(0, "admin.zkhelper", "Zk. Helper", "<color=#00FF6E><b>Zk. Helper</b></color>", "https://i.ibb.co/n17F91Y/zkshield.png") },
            { "admin.helper", new Rank(1, "admin.helper", "Helper", "<color=#2BF314><b>Helper</b></color>", "https://i.ibb.co/tM33PQj/helshield.png") },
            { "admin.mod", new Rank(2, "admin.mod", "Mod", "<color=#00edd1><b>Mod</b></color>", "player") },
            { "admin.admin", new Rank(3, "admin.admin", "Admin", "<color=#9661ff><b>Admin</b></color>", "player") },
            { "admin.owner", new Rank(4, "admin.owner", "Owner", "<color=#fb9d8f><b>Owner</b></color>", "player") },
        };
    }
}
