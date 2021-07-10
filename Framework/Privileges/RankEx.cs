using RealLifeFramework.Privileges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealLifeFramework
{
    public static class RankEx
    {
        public static string ToPermission(this Rank rank) => rank.PermIdentifier;
    }
}
