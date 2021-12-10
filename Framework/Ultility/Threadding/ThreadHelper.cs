using RealLifeFramework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RealLifeFramework.Threadding
{
    public class ThreadHelper
    {
        public static void Execute(Action action)
        {
            ThreadPool.QueueUserWorkItem( _ =>
            {
                try
                {
                    action();
                }
                catch(Exception ex)
                {
                    Logger.LogWarn(ex.ToString());
                }
            });
        }

        public static void ExecuteAsync(Func<Task> task)
        {
            ThreadPool.QueueUserWorkItem(async _ =>
            {
                try
                {
                    await task();
                }
                catch (Exception ex)
                {
                    Logger.LogWarn(ex.ToString());
                }
            });
        }
    }
}
