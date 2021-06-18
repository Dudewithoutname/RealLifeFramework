using RealLifeFramework.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealLifeFramework.Data
{
    public class SkillUserData
    {
        public ushort EducationPoints { get; set; }
        public List<ISkill> Skills { get; set; }
        public List<IEducation> Educations { get; set; }
    }
}
