using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace RealLifeFramework
{
    [Table(nameof(TSecurity))]
    public class TSecurity : ITable
    {
        public static string Name => ((Table)Attribute.GetCustomAttribute(typeof(TSecurity), typeof(Table))).Name;

        public MySqlCommand Create()
        {
            return new MySqlCommand($"CREATE TABLE IF NOT EXISTS {Name} ( " +
                        "hwid VARCHAR(40) PRIMARY KEY," +
                        "steamid VARCHAR(17)," +
                        "ban BIT DEFAULT 0" +
                        ")", RealLife.Database.Connection);
        }

        public static void CheckRegister(string steamid, string hwid)
        {
            if (RealLife.Database.IsConnected() && RealLife.Database.get(Name, 0, "hwid", hwid) == null)
                new MySqlCommand($"INSERT INTO {Name} (hwid, steamid) VALUES ('{hwid}','{steamid}')", RealLife.Database.Connection).ExecuteNonQuery();
        }

        public static void AddHWIDBan(string steamid)
        {
            if (RealLife.Database.IsConnected())
                new MySqlCommand($"UPDATE {Name} SET ban = 1 WHERE steamid = '{steamid}'", RealLife.Database.Connection).ExecuteNonQuery();
        }

        public static void RemoveHWIDBan(string steamid)
        {
            if (RealLife.Database.IsConnected())
                new MySqlCommand($"UPDATE {Name} SET ban = 0 WHERE steamid = '{steamid}'", RealLife.Database.Connection).ExecuteNonQuery();
        }

    }
}
