using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealLifeFramework.Ranks
{
    public struct Rank
    {
        public string Name;
        public string Prefix;
        public string Icon;

        public byte Level;
        public string PermIdentifier;

        public Rank(byte level, string perm, string name, string prefix, string icon)
        {
            Level = level;
            PermIdentifier = perm;

            Name = name;
            Prefix = prefix;
            Icon = icon;
        }
    }
}
