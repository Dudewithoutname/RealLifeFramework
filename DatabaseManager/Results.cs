using System.Collections.Generic;
using Steamworks;
using RealLifeFramework.Jobs;
using RealLifeFramework.Skills;

namespace RealLifeFramework
{
    public class DBPlayerResult
    {
        public string Name { get; set; }
        public ushort Age { get; set; }
        public byte Gender { get; set; }
        public ulong Money { get; set; }
        public ushort Level { get; set; }
        public uint Exp { get; set; }
    }

    public class DBJobInfoResult
    {
        public Job Job { get; set; }
        public ushort Level { get; set; }
        public uint Exp { get; set; }
    }

    public class DBSkillsResult
    {
        public ushort EducationPoints { get; set; }
        public List<ISkill> Skills { get; set; }
        public List<IEducation> Educations { get; set; }
    }
}
