using Newtonsoft.Json;
using RealLifeFramework.Data.Models;
using RealLifeFramework.RealPlayers;
using RealLifeFramework.SecondThread;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RealLifeFramework.Data
{
    public class DataManager 
    {
        public static string DataPath = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\U3DS\\Servers\\Default\\DudeTurned";
        public static string PlayerPath = $"{DataPath}\\Players";

        public static void Settup()
        {
            if (!Directory.Exists(DataPath)) 
            {
                Logger.Log("[DataManager] : Data not existing creating folders");
                Directory.CreateDirectory(DataPath);
                Directory.CreateDirectory(PlayerPath);
            }
        }

        public static bool ExistPlayer(CSteamID steamId) => File.Exists($"{PlayerPath}\\{steamId}.json");

        public static void CreatePlayer(RealPlayer player)
        {
            SecondaryThread.Execute(() =>
            {
                if (!ExistPlayer(player.CSteamID))
                {
                    var path = $"{PlayerPath}\\{player.CSteamID}.json";
                    File.Create(path).Close();
                    writeJson(path, (RealPlayerData)player);
                }
            });
        }

        public static RealPlayerData LoadPlayer(CSteamID steamId)
        {
            RealPlayerData player = null;
            SecondaryThread.Execute(() =>
            {
                if (ExistPlayer(steamId))
                {
                    player = JsonConvert.DeserializeObject<RealPlayerData>(File.ReadAllText($"{PlayerPath}\\{steamId}.json"));
                }
            });
            return player;
        }

        public static void SavePlayer(RealPlayer player)
        {
            SecondaryThread.Execute(() =>
            {
                if (!ExistPlayer(player.CSteamID))
                {
                    var path = $"{PlayerPath}\\{player.CSteamID}.json";
                    writeJson(path, (RealPlayerData)player);
                }
            });
        }

        private static void writeJson(string path, object value)
        {
            var json = JsonConvert.SerializeObject(value);

            using (var writer = new StreamWriter(path))
            {
                writer.Write(json);
            }
        }
    }
}
