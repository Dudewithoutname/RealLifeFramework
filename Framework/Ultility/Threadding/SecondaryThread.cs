using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RealLifeFramework.SecondThread
{
    public class SecondaryThread
    {
        private static List<Action> queue = new List<Action>();
        private static List<Action> copiedQueue = new List<Action>();
        private static bool hasAction = false;
        private static Thread thread;

        public static void Start()
        {
            thread = new Thread(UpdateThread);
            thread.Start();
            Logger.Log("[SecondaryThread] : Started!");

        }

        public static void Execute(Action action)
        {
            if (action == null)
            {
                Logger.Log("[SecondaryThread] : No action to execute on Secondary thread!");
                return;
            }

            lock (queue)
            {
                queue.Add(action);
                hasAction = true;
            }
        }

        private static void UpdateThread()
        {
            while (true)
            {
                if (hasAction)
                {
                    copiedQueue.Clear();

                    lock (queue)
                    {
                        copiedQueue.AddRange(queue);
                        queue.Clear();
                        hasAction = false;
                    }

                    for (int i = 0; i < copiedQueue.Count; i++)
                    {
                        copiedQueue[i]();
                    }

                }

                Thread.Sleep(0);
            }
        }
    }
}
