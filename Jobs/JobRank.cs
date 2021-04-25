using System;
using System.Collections.Generic;

namespace RealLifeFramework.Jobs
{
    public static class JobRanks
    {
        public readonly static List<JobRank> Ranks = new List<JobRank>() { 
            new JobRank("Novice", 1, 0),
            new JobRank("Beginner", 2, 100),
            new JobRank("Regular", 3, 300),
            new JobRank("Intermediate", 4, 500),
            new JobRank("Advanced", 5, 1000),
            new JobRank("Expert", 6, 1500),
            new JobRank("Professional", 7, 2500),
            new JobRank("Master", 8, 5000),
        };
        
    }

    public class JobRank
    {
        public string Name { get; set; }
        public ushort RefLevel { get; set; }
        public uint MaxExp { get; set; }

        public JobRank(string name, ushort level, uint exp)
        {
            Name = name;
            RefLevel = level;
            MaxExp = exp;
        }
    }
}
