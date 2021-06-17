using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealLifeFramework.Data
{
    public class DataManager
    {
        public static string DataPath = "D:\\SteamLibrary\\steamapps\\common\\U3DS\\Servers\\Default\\Rocket\\DudeTurned_Data";

        public static void Settup()
        {
            if (!Directory.Exists(DataPath)) 
            {
                Logger.Log("[DataManager] : Data not existing creating folders");
                Directory.CreateDirectory(DataPath);
            }
        }

        /*public static T GetData<T>()
        {
            return null;
        }*/
    }
}
