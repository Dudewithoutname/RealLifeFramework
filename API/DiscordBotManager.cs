using Newtonsoft.Json;
using RealLifeFramework.API.Models;
using RealLifeFramework.Patches;
using RealLifeFramework.SecondThread;
using RealLifeFramework.UserInterface;
using SDG.Unturned;
using System.Threading;
using System.Timers;
using UnityEngine;

namespace RealLifeFramework
{
    public class DiscordBotManager
    {
        public static void Load()
        {
            UpdateServerStats();
            Provider.onCommenceShutdown += onShutDown;
        }
        
        public static void UpdateServerStats()
        {
            int playerCount = 0;
            int pdCount = 0;
            int emsCount = 0;

            foreach (SteamPlayer player in Provider.clients)
            {
                playerCount++;
            }
            Logger.Log($"{playerCount}");
            // message
            Api.Send("/info/stats", JsonConvert.SerializeObject(
                new Stats()
                {
                    online = true,
                    players = playerCount,
                    pd = pdCount,
                    ems = emsCount,
                    serverIP = RealLife.Instance.Configuration.Instance.IP,
                    serverPort = Provider.port.ToString()
                }
            ));

            // tab
            Api.Send("/info/tab", JsonConvert.SerializeObject(
                new Tab()
                {
                    players = playerCount,
                    time = $"{HUD.FormatTime(Patches.Time.Current[0], Patches.Time.Current[1])}",
                    night = (Patches.Time.Current[0] < 5 || Patches.Time.Current[0] > 22),
                }
            ));
        }

        private static void onShutDown()
        {
            // message
            Api.Send("/info/stats", JsonConvert.SerializeObject(
                new Stats()
                {
                    online = false,
                }
            ));

            // tab
            Api.Send("/info/tab", JsonConvert.SerializeObject(
                new Tab()
                {
                    players = 0,
                    time = "offline",
                }
            ));
        }
    }
}
