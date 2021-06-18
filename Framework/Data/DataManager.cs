using Newtonsoft.Json;
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
        public static string DataPath = "D:\\SteamLibrary\\steamapps\\common\\U3DS\\Servers\\Default\\Rocket\\DudeTurned";
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

        private static void writeJson(string path, object value)
        {
            var json = JsonConvert.SerializeObject(value);

            using (var writer = new StreamWriter(path))
            {
                writer.Write(json);
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
                    File.Create(path);
                    writeJson(path, (object)player);
                }

            });
        }

        public static RealPlayer LoadPlayer(CSteamID steamId)
        {
            RealPlayer player = null;
            SecondaryThread.Execute(() =>
            {
                player = JsonConvert.DeserializeObject<RealPlayer>(File.ReadAllText($"{PlayerPath}\\{steamId}.json"));
            });
            return player;

        } 

    }
}
