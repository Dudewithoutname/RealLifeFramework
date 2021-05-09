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
                        "steamid VARCHAR(17) PRIMARY KEY," +
                        "hwid VARCHAR(40)" +
                        ")", RealLife.Database.Connection);
        }

        public static void AddHWIDBan(string hwid)
        {
            if (RealLife.Database.IsConnect())
            {
                var cmd = new MySqlCommand($"INSERT INTO {Name} (hwid) VALUES ('{hwid}')", RealLife.Database.Connection);

                cmd.ExecuteNonQuery();

            }
        }

        public static void RemoveHWIDBan(string hwid)
        {
            if (RealLife.Database.IsConnect())
            {
                var cmd = new MySqlCommand($"DELETE FROM {Name} WHERE 'hwid' = {hwid}", RealLife.Database.Connection);

                cmd.ExecuteNonQuery();

            }
        }

    }
}
