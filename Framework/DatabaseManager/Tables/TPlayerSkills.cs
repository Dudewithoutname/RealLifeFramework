using System;
using MySql.Data.MySqlClient;
using RealLifeFramework.Skills;
using System.Collections.Generic;
using Steamworks;
using RealLifeFramework.Players;

namespace RealLifeFramework
{
    [Table(nameof(TPlayerSkills))]
    public class TPlayerSkills : ITable
    {
        public static TPlayerSkills Instnace;
        public static string Name => ((Table)Attribute.GetCustomAttribute(typeof(TPlayerSkills), typeof(Table))).Name;

        public MySqlCommand Create()
        {
            Instnace = this;

            return new MySqlCommand($"CREATE TABLE IF NOT EXISTS {Name} ( " +
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
                        $"s{Dexterity.Id}lvl TINYINT DEFAULT 0," +
                        $"s{Dexterity.Id}xp INT DEFAULT 0," +
                        // Educations
                        $"e{Engineering.Id}lvl TINYINT DEFAULT 0," +
                        $"e{Culinary.Id}lvl TINYINT DEFAULT 0," +
                        $"e{Crafting.Id}lvl TINYINT DEFAULT 0," +
                        $"e{Medicine.Id}lvl TINYINT DEFAULT 0," +
                        $"e{Defense.Id}lvl TINYINT DEFAULT 0" +
                        ")", RealLife.Database.Connection);
        }

        private static int getCI(int id, byte type)
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

        private static string getCbyId(int id, byte type, string extras)
        {
            // Skill
            if (type == 0)
                return $"s{id}{extras}";
            // Education
            else
                return $"e{id}{extras}";
        }

        public static void UpdateSkill(CSteamID player, int id, byte level, uint exp)
        {
            RealLife.Database.set(TPlayerSkills.Name, player.ToString(), getCbyId(id, 0, "lvl"), level.ToString());
            RealLife.Database.set(TPlayerSkills.Name, player.ToString(), getCbyId(id, 0, "xp"), exp.ToString());
        }

        public static void UpdateEducation(CSteamID player, int id, byte level) => RealLife.Database.set(TPlayerSkills.Name, player.ToString(), getCbyId(id, 1, "lvl"), level.ToString());

        public static void UpdateEducationPoints(CSteamID player, ushort points) => RealLife.Database.set(TPlayerSkills.Name, player.ToString(), "edupoints", points.ToString());

        public static DBSkillsResult GetSkillsInfo(RealPlayer player)
        {
            DBSkillsResult result = null;

            if (RealLife.Database.IsConnect())
            {
                var cmd = new MySqlCommand($" SELECT * FROM {TPlayerSkills.Name} WHERE steamid = '{player.CSteamID}' ", RealLife.Database.Connection);
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
                            new Dexterity(player, Convert.ToByte(reader[getCI(Dexterity.Id, 0)].ToString()) , Convert.ToUInt32(reader[getCI(Dexterity.Id+1, 0)].ToString()) ),
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
    }

    public class DBSkillsResult
    {
        public ushort EducationPoints { get; set; }
        public List<ISkill> Skills { get; set; }
        public List<IEducation> Educations { get; set; }
    }
}
