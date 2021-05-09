using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using Steamworks;
using RealLifeFramework.Jobs;

namespace RealLifeFramework
{
    [Table(nameof(PlayerJob))]
    public class PlayerJob : ITable
    {
        public static PlayerJob Instnace;
        public static string Name => ((Table)Attribute.GetCustomAttribute(typeof(PlayerJob), typeof(Table))).Name;

        public MySqlCommand Create()
        {
            Instnace = this;

            return new MySqlCommand($"CREATE TABLE IF NOT EXISTS {Name} ( " +
                        "steamid VARCHAR(17) PRIMARY KEY," +
                        "id TINYINT DEFAULT -1," +
                        "level TINYINT DEFAULT 0," +
                        "exp INT DEFAULT 0" +
                        ")", RealLife.Database.Connection);
        }

        public static DBJobInfoResult GetJobInfo(CSteamID csteamid)
        {
            DBJobInfoResult result = null;

            if (RealLife.Database.IsConnect())
            {
                var cmd = new MySqlCommand($" SELECT * FROM {PlayerJob.Name} WHERE steamid = '{csteamid}' ", RealLife.Database.Connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    result = new DBJobInfoResult()
                    {
                        Job = JobManager.GetJobByID(Convert.ToInt16(reader[1].ToString())),
                        Level = Convert.ToUInt16(reader[2].ToString()),
                        Exp = Convert.ToUInt32(reader[3].ToString()),
                    };
                }
                 
                reader.Close();

                return result;
            }
            else
            {
                return null;
            }
        }

    }
}
