using HarmonyLib;
using Newtonsoft.Json;
using RealLifeFramework.RealPlayers;
using RealLifeFramework.Skills;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RealLifeFramework.Data.Models
{
    public class SkillUserData
    {
        public ushort EducationPoints { get; set; }
        public SkillData[] Skills { get; set; }
        public int[] Educations { get; set; }

        [JsonConstructor]
        public SkillUserData(ushort educationPoints, SkillData[] skills, int[] educations)
        {
            EducationPoints = educationPoints;
            Skills = skills;
            Educations = educations;
        }

        public SkillUserData() { }

        public static explicit operator SkillUserData(RealPlayer player)
        {
            List<int> education = new List<int>(); foreach (var edu in player.SkillUser.Educations) { education.Add(edu.Level); }
            List<SkillData> sd = new List<SkillData>(); foreach (var skl in player.SkillUser.Skills) { sd.Add(new SkillData() { Level = skl.Level, Exp = skl.Exp }); }

            return new SkillUserData()
            {
                EducationPoints = player.SkillUser.EducationPoints,
                Skills = sd.ToArray(),
                Educations = education.ToArray(),
            };
        }
    }
}
