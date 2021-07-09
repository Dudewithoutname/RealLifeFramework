using RealLifeFramework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RealLifeFramework.Threadding
{
    public class HelperThread
    {
        public static Dictionary<string, ISaveable> Values = new Dictionary<string, ISaveable>();

        private static List<Action> queue = new List<Action>();
        private static List<Action> copiedQueue = new List<Action>();
        private static bool hasAction = false;
        private static Thread thread;

        public static void Start()
        {
            thread = new Thread(UpdateThread);
            thread.Start();
            Logger.Log("[HelperThread] : Started!");

        }

        public static ISaveable Fetch(string key)
        {
            if (Values.ContainsKey(key))
            {
                return Values[key];
            }
            else
            {
                return null;
            }
        }

        public static void Execute(Action action)
        {
            if (action == null)
            {
                Logger.Log("[HelperThread] : No action to execute on Secondary thread!");
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

                    try
                    {
                        for (int i = 0; i < copiedQueue.Count; i++)
                        {
                            copiedQueue[i]();
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(ex.ToString());
                    }
                }

                Thread.Sleep(0);
            }
        }
    }
}
