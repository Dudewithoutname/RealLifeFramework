using RealLifeFramework.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealLifeFramework.Data
{
    public class RealPlayerData
    {
        // * Character
        public string Name { get; set; }
        public ushort Age { get; set; }
        public string Gender { get; set; }

        public SkillUser SkillUser { get; set; }

    }
}
