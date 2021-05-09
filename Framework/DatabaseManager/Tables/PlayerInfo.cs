using System;
using System.Reflection;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using Steamworks;

namespace RealLifeFramework
{
    [Table(nameof(PlayerInfo))]
    public class PlayerInfo : ITable
    {
        public static PlayerInfo Instnace;
        public static string Name => ((Table)Attribute.GetCustomAttribute(typeof(PlayerInfo), typeof(Table))).Name;

        public MySqlCommand Create()
        {
            Instnace = this;

            return new MySqlCommand($"CREATE TABLE IF NOT EXISTS {Name} ( " +
                        "steamid VARCHAR(17) PRIMARY KEY," +
                        "name VARCHAR(21)," +
                        "age VARCHAR(2)," +
                        "gender BIT(1)," +
                        "level TINYINT," +
                        "exp INT" +
                        ")", RealLife.Database.Connection);
        }

        public static void NewPlayer(string csteamid, string fullname, ushort age, byte gender)
        {
            if (RealLife.Database.IsConnect())
            {
                string queryPlayer = $"INSERT INTO {PlayerInfo.Name} (steamid, name, age, gender, level, exp) VALUES " +
                    $"('{csteamid}', '{fullname}', '{age}', '{gender}', '1','0')";

                string queryJob = $"INSERT INTO {PlayerJob.Name} (steamid, id) VALUES ('{csteamid}', '-1')";

                string querySkill = $"INSERT INTO {PlayerSkills.Name} (steamid) VALUES ('{csteamid}')";

                List<MySqlCommand> cmds = new List<MySqlCommand>()
                {
                    new MySqlCommand(queryPlayer, RealLife.Database.Connection),
                    new MySqlCommand(queryJob, RealLife.Database.Connection),
                    new MySqlCommand(querySkill, RealLife.Database.Connection),
                };

                cmds.ForEach((cmd) => cmd.ExecuteNonQuery());
            }
        }


        public static DBPlayerResult GetPlayer(CSteamID csteamid)
        {
            DBPlayerResult result = null;

            if (RealLife.Database.IsConnect())
            {
                var cmd = new MySqlCommand($" SELECT * FROM {PlayerInfo.Name} WHERE steamid = '{csteamid}' ", RealLife.Database.Connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    result = new DBPlayerResult()
                    {
                        Name = reader[1].ToString(),
                        Age = Convert.ToUInt16(reader[2].ToString()),
                        Gender = Convert.ToByte(reader[3].ToString()),
                        Level = Convert.ToUInt16(reader[4].ToString()),
                        Exp = Convert.ToUInt32(reader[5].ToString()),

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
