using Newtonsoft.Json;
using RealLifeFramework.Data.Models;
using RealLifeFramework.RealPlayers;
using RealLifeFramework.Threadding;
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
        private static readonly string dataPath = @"D:\SteamLibrary\steamapps\common\U3DS\Servers\Default\Dudeturned";
        private static readonly string[] storages = {"Players", "Server"};

        public static void Settup()
        {
            if (!Directory.Exists(dataPath)) 
            {
                Logger.Log("[DataManager] : Data not existing creating folders");
                Directory.CreateDirectory(dataPath);
                foreach(string storage in storages) { Directory.CreateDirectory($@"{dataPath}\{storage}"); };
            }
            Logger.Log("[DataManager] : Loaded");
        }

        #region Data
        public static bool ExistData(string storage, string key) => File.Exists($@"{dataPath}\{storage}\{key}.json");

        public static void CreateData(string storage, string key, object data)
        {
            ManagementThread.Execute(() =>
            {
                if (!ExistData(storage, key))
                {
                    var path = $@"{dataPath}\{storage}\{key}.json";
                    File.Create(path).Close();
                    writeJson(path, data);
                }
            });
        }

        public static ISaveable LoadData<ISaveable>(string storage, string key)
        {
            if (ExistData(storage, key))
            {
                return JsonConvert.DeserializeObject<ISaveable>(File.ReadAllText($@"{dataPath}\{storage}\{key}.json"));
            }

            return default(ISaveable);
        }

        public static void SaveData(string storage, string key, object data)
        {
            ManagementThread.Execute(() =>
            {
                if (ExistData(storage, key))
                {
                    var path = $@"{dataPath}\{storage}\{data}.json";
                    writeJson(path, data);
                }
            });
        }
        #endregion

        #region player Data 
         // am just lazy to rewrite code to normal data :P maybe in laaaaate future :D
        public static bool ExistPlayer(CSteamID steamId) => File.Exists($@"{dataPath}\Players\{steamId}.json");

        public static void CreatePlayer(RealPlayer player)
        {
            ManagementThread.Execute(() =>
            {
                if (!ExistPlayer(player.CSteamID))
                {
                    var path = $@"{dataPath}\Players\{player.CSteamID}.json";
                    File.Create(path).Close();
                    writeJson(path, (RealPlayerData)player);
                }
            });
        }

        public static void DeletePlayer(CSteamID steamId)
        {
            ManagementThread.Execute(() =>
            {
                if (!ExistPlayer(steamId))
                {
                    var path = $@"{dataPath}\Players\{steamId}.json";
                    File.Delete(path);
                }
            });
        }

        public static RealPlayerData LoadPlayer(CSteamID steamId)
        {
            RealPlayerData player = null;
            
            if (ExistPlayer(steamId))
            {
                player = JsonConvert.DeserializeObject<RealPlayerData>(File.ReadAllText($@"{dataPath}\Players\{steamId}.json"));
            }

            return player;
        }

        public static void SavePlayer(RealPlayer player)
        {
            ManagementThread.Execute(() =>
            {
                if (ExistPlayer(player.CSteamID))
                {
                    var path = $@"{dataPath}\Players\{player.CSteamID}.json";
                    writeJson(path, (RealPlayerData)player);
                }
            });
        }
        #endregion

        private static void writeJson(string path, object value) => File.WriteAllText(path, JsonConvert.SerializeObject(value, Formatting.Indented));
    }
}
