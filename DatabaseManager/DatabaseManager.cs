using System;
using System.Collections.Generic;
using Steamworks;
using MySql.Data.MySqlClient;
using RealLifeFramework.Jobs;
using RealLifeFramework.Skills;
using RealLifeFramework.Players;

namespace RealLifeFramework
{
    public class DatabaseManager
    {
        public static string TablePlayer = "PlayerInfo";
        public static string TablePlayerJob = "PlayerJob";
        public static string TablePlayerSkills = "PlayerSkills";

        public string Server { get; set; }
        public string DatabaseName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Port { get; set; }

        public MySqlConnection Connection { get; set; }

        private static DatabaseManager instance = null;

        public static DatabaseManager Instance()
        {
            if (instance == null)
                instance = new DatabaseManager();

            return instance;
        }

        public void Connect()
        {
            string connstring = string.Format("Server={0}; database={1}; UID={2}; password={3}; port={4}", Server, DatabaseName, UserName, Password, Port);
            Connection = new MySqlConnection(connstring);
            Connection.Open();
        }

        public bool IsConnect()
        {
            if (Connection == null)
            {
                if (String.IsNullOrEmpty(DatabaseName))
                    return false;

                string connstring = string.Format("Server={0}; database={1}; UID={2}; password={3}; port={4}", Server, DatabaseName, UserName, Password, Port);
                Connection = new MySqlConnection(connstring);
                Connection.Open();
                Logger.Log("[Database Manager] : Connected");
            }

            return true;
        }

        public void Close()
        {
            Logger.Log("[Database Manager] : Connection Closed");
            Connection.Close();
        }

        public void Debug()
        {
            string[] query = 
            { 
                $"DELETE FROM {TablePlayer}",
                $"DELETE FROM {TablePlayerJob}",
                $"DELETE FROM {TablePlayerSkills}",
            };

            for(int i = 0; i < query.Length; i++)
            {
                var cmd = new MySqlCommand(query[i], this.Connection);
                cmd.ExecuteNonQuery();
            }
        }

        public void Setup()
        {
            if (IsConnect())
            {
                List<MySqlCommand> Tables = new List<MySqlCommand>()
                {
                    new MySqlCommand($"CREATE TABLE IF NOT EXISTS {TablePlayer} ( " +
                        "steamid VARCHAR(17) PRIMARY KEY," +
                        "name VARCHAR(21)," +
                        "age VARCHAR(2)," +
                        "gender BIT(1)," +
                        "money VARCHAR(20)," +
                        "level TINYINT," +
                        "exp INT" +
                        ")", this.Connection),

                    new MySqlCommand($"CREATE TABLE IF NOT EXISTS {TablePlayerJob} ( " +
                        "steamid VARCHAR(17) PRIMARY KEY," +
                        "id TINYINT DEFAULT -1," +
                        "level TINYINT DEFAULT 0," +
                        "exp INT DEFAULT 0" +
                        ")", this.Connection),

                    new MySqlCommand($"CREATE TABLE IF NOT EXISTS {TablePlayerSkills} ( " +
                        "steamid VARCHAR(17) PRIMARY KEY," +
                        "edupoints INT DEFAULT 0," +
                        // Skills
                        $"s{Endurance.Id}lvl TINYINT DEFAULT 0," +
                        $"s{Endurance.Id}xp INT DEFAULT 0," +
                        $"s{Farming.Id}lvl TINYINT DEFAULT 0," +
                        $"s{Farming.Id}xp INT DEFAULT 0," +
                        $"s{Fishing.Id}lvl TINYINT DEFAULT 0," +
                        $"s{Fishing.Id}xp INT DEFAULT 0," +
                        $"s{Agitily.Id}lvl TINYINT DEFAULT 0," +
                        $"s{Agitily.Id}xp INT DEFAULT 0," +
                        // Educations
                        $"e{Engineering.Id}lvl TINYINT DEFAULT 0," +
                        $"e{Culinary.Id}lvl TINYINT DEFAULT 0," +
                        $"e{Crafting.Id}lvl TINYINT DEFAULT 0," +
                        $"e{Medicine.Id}lvl TINYINT DEFAULT 0," +
                        $"e{Defense.Id}lvl TINYINT DEFAULT 0" +
                        ")", this.Connection),
                };

                Tables.ForEach((table) => table.ExecuteNonQuery());
            }
        }

        #region default
        private void set(string table, string playerID, string key, string newValue)
        {
            if (IsConnect())
            {
                string query = $" UPDATE {table} SET {key} = '{newValue}' WHERE steamid = {playerID} ";
                var cmd = new MySqlCommand(query, this.Connection);
                cmd.ExecuteNonQuery();
            }
        }

        private string get(string table, int pos, string name, string value)
        {
            string x = null;

            if (IsConnect())
            {

                string query = $" SELECT * FROM {table} WHERE {name} = '{value}' ";
                var cmd = new MySqlCommand(query, this.Connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    x = reader[pos].ToString();
                }
                reader.Close();
                return x;
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Player Management

        public void NewPlayer(string csteamid, string fullname, ushort age, byte gender, uint money)
        {
            if (IsConnect())
            {
                string queryPlayer = $"INSERT INTO {TablePlayer} (steamid, name, age, gender, money, level, exp) VALUES " +
                    $"('{csteamid}', '{fullname}', '{age}', '{gender}', '{money}', '1', '0')";

                string queryJob = $"INSERT INTO {TablePlayerJob} (steamid, id) VALUES ('{csteamid}', '-1')";

                string querySkill = $"INSERT INTO {TablePlayerSkills} (steamid) VALUES ('{csteamid}')";

                List<MySqlCommand> cmds = new List<MySqlCommand>()
                {
                    new MySqlCommand(queryPlayer, this.Connection),
                    new MySqlCommand(queryJob, this.Connection),
                    new MySqlCommand(querySkill, this.Connection),
                };

                cmds.ForEach((cmd) => cmd.ExecuteNonQuery());
            }
        }


        public DBPlayerResult GetPlayer(CSteamID csteamid)
        {
            DBPlayerResult result = null;

            if (IsConnect())
            {
                var cmd = new MySqlCommand($" SELECT * FROM {TablePlayer} WHERE steamid = '{csteamid}' ", this.Connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    result = new DBPlayerResult()
                    {
                        Name = reader[1].ToString(),
                        Age = Convert.ToUInt16(reader[2].ToString()),
                        Gender = Convert.ToByte(reader[3].ToString()),
                        Money = Convert.ToUInt64(reader[4].ToString()),
                        Level = Convert.ToUInt16(reader[5].ToString()),
                        Exp = Convert.ToUInt32(reader[6].ToString()),

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

        #endregion

        #region Job Related Stuff

        public DBJobInfoResult GetJobInfo(CSteamID csteamid)
        {
            DBJobInfoResult result = null;

            if (IsConnect())
            {
                var cmd = new MySqlCommand($" SELECT * FROM {TablePlayerJob} WHERE steamid = '{csteamid}' ", this.Connection);
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

        #endregion

        #region Skills & Educations

        // Get Column Index
        private int getCI(int id, byte type)
        {
            // Skill
            if (type == 0) 
                if (id != 0)
                    return id * 2;
                else
                    return 2;
            // Education
            else
                if (id != 0)
                    return 9 + id;
                else
                    return 2;
        }

        private string getCbyId(int id, byte type, string extras)
        {
            // Skill
            if (type == 0)
                return $"s{id}{extras}";
            // Education
            else
                return $"e{id}{extras}";
        }

        public void UpdateSkill(CSteamID player, int id, byte level, uint exp)
        {
            set(TablePlayerSkills, player.ToString(), getCbyId(id, 0, "lvl"), level.ToString());
            set(TablePlayerSkills, player.ToString(), getCbyId(id, 0, "exp"), exp.ToString());
        }

        public void UpdateEducation(CSteamID player, int id, byte level) => set(TablePlayerSkills, player.ToString(), getCbyId(id, 1, "lvl"), level.ToString());

        public void UpdateEducationPoints(CSteamID player, ushort points) => set(TablePlayerSkills, player.ToString(), "edupoints", points.ToString());

        public DBSkillsResult GetSkillsInfo(RealPlayer player)
        {
            DBSkillsResult result = null;

            if (IsConnect())
            {
                var cmd = new MySqlCommand($" SELECT * FROM {TablePlayerSkills} WHERE steamid = '{player.CSteamID}' ", this.Connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    result = new DBSkillsResult()
                    {
                        EducationPoints = Convert.ToUInt16(reader[3].ToString()),

                        Skills = new List<ISkill>() { 
                            new Endurance(player, Convert.ToByte(reader[getCI(Endurance.Id, 0)].ToString()) , Convert.ToUInt32(reader[getCI(Endurance.Id+1, 0)].ToString()) ),
                            new Farming(player, Convert.ToByte(reader[getCI(Farming.Id, 0)].ToString()) , Convert.ToUInt32(reader[getCI(Farming.Id+1, 0)].ToString()) ),
                            new Fishing(player, Convert.ToByte(reader[getCI(Fishing.Id, 0)].ToString()) , Convert.ToUInt32(reader[getCI(Fishing.Id+1, 0)].ToString()) ),
                            new Agitily(player, Convert.ToByte(reader[getCI(Agitily.Id, 0)].ToString()) , Convert.ToUInt32(reader[getCI(Agitily.Id+1, 0)].ToString()) ),
                        },

                        Educations = new List<IEducation>()
                        {
                            new Engineering(player, Convert.ToByte(reader[getCI(Engineering.Id, 1)].ToString())),
                            new Culinary(player, Convert.ToByte(reader[getCI(Culinary.Id, 1)].ToString())),
                            new Crafting(player, Convert.ToByte(reader[getCI(Crafting.Id, 1)].ToString())),
                            new Medicine(player, Convert.ToByte(reader[getCI(Medicine.Id, 1)].ToString())),
                            new Defense(player, Convert.ToByte(reader[getCI(Defense.Id, 1)].ToString())),
                        }
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

        #endregion
    }
}
