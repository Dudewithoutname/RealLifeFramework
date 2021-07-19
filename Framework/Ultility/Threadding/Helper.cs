using RealLifeFramework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RealLifeFramework.Threadding
{
    public class Helper
    {
        public static void Execute(Action action)
        {
            ThreadPool.QueueUserWorkItem( (_) =>
            {
                try
                {
                    action();
                }
                catch
                {

                }
            });
        }
    }
}
