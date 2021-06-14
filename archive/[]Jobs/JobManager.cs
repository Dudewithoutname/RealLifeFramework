using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealLifeFramework.Jobs
{
    public static class JobManager
    {
        public static List<Job> Jobs = new List<Job>(); // Instances of Jobs
        
        public static void Load()
        {
            Jobs.Add(new Fisherman());


            Jobs.ForEach( (Job) => Logger.Log($"[JobManager] {Job.Name} Loaded") );
        }

        public static Job GetJobByID(short id)
        {
            if (id != -1)
                return Jobs[id];
            else
                return null;
        }
    }
}
