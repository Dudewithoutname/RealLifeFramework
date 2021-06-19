using Newtonsoft.Json;
using RealLifeFramework.RealPlayers;
using RealLifeFramework.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealLifeFramework.Data.Models
{
    public class SkillUserData
    {
        public ushort EducationPoints { get; set; }
        public object[] Skills { get; set; }
        public object[] Educations { get; set; }

        [JsonConstructor]
        public SkillUserData(ushort educationPoints, object[] skills, object[] educations)
        {
            EducationPoints = educationPoints;
            Skills = skills;
            Educations = educations;
        }

        public SkillUserData() { }
    }
}
