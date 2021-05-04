using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealLifeFramework.Jobs
{
    public class JobUser
    {
        public ushort Level { get; set; }
        public uint Exp { get; set; }
        public JobRank JobRank { get; set; }
        public Job Job { get; set; }

        public virtual void AddExp(uint exp)
        {
            Exp += exp;

            if (Exp >= JobRank.MaxExp)
            {
                Exp -= JobRank.MaxExp;
                LevelUp();
                // TODO: Notify about new level
            }
            else
            {
                // TODO Skip,  Just return 
            }
        }

        public virtual void LevelUp()
        {
            if (Level < Job.MaxLevel)
            {
                Level++;
                Exp = 0;
                JobRank = JobRanks.Ranks[Level - 1];

                // TODO: LEVEL UP INFO TO PLAYER
            }
        }

    }
}
