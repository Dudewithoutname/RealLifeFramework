using System;
using RealLifeFramework.Players;

namespace RealLifeFramework.Skills
{
    public class Agitily : ISkill
    {
        public static readonly byte Id = 3;

        public RealPlayer Player { get; set; }
        public string Name => nameof(Agitily);
        public byte MaxLevel => 12;

        public byte Level { get; set; }
        public uint Exp { get; set; }


        public void AddExp(uint exp)
        {
            Exp += exp;

            if (Exp >= GetExpToNextLevel())
            {
                Exp -= GetExpToNextLevel();
                LevelUp();
            }
        }


        public uint GetExpToNextLevel()
        {
            byte NextLevel = Convert.ToByte(Level + 1);

            if (NextLevel != (MaxLevel + 1))
                switch (NextLevel)
                {
                    case 1:
                        return 100;
                    case 2:
                        return 250;
                    case 3:
                        return 500;
                    case 4:
                        return 1000;
                    case 5:
                        return 1500;
                    case 6:
                        return 2000;
                    case 7:
                        return 2500;
                    case 8:
                        return 3000;
                    case 9:
                        return 4000;
                    case 10:
                        return 5000;
                    case 11:
                        return 6000;
                    case 12:
                        return 7500;
                    default:
                        return 0;
                }
            else
                return 0;
        }

        public void LevelUp()
        {
            switch (++Level)
            {
                case 1:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Cardio[0], VanillaSkills.Cardio[1], 1);
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Diving[0], VanillaSkills.Diving[1], 1);
                    break;
                case 2:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Exercise[0], VanillaSkills.Exercise[1], 1);
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Parkour[0], VanillaSkills.Parkour[1], 1);
                    break;
                case 3:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Cardio[0], VanillaSkills.Cardio[1], 2);
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Diving[0], VanillaSkills.Diving[1], 2);
                    break;
                case 4:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Exercise[0], VanillaSkills.Exercise[1], 2);
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Parkour[0], VanillaSkills.Parkour[1], 2);
                    break;
                case 5:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Cardio[0], VanillaSkills.Cardio[1], 3);
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Diving[0], VanillaSkills.Diving[1], 3);
                    break;
                case 6:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Diving[0], VanillaSkills.Diving[1], 4);
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Exercise[0], VanillaSkills.Exercise[1], 3);
                    break;
                case 7:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Cardio[0], VanillaSkills.Cardio[1], 4);
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Parkour[0], VanillaSkills.Parkour[1], 3);
                    break;
                case 8:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Diving[0], VanillaSkills.Diving[1], 5);
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Exercise[0], VanillaSkills.Exercise[1], 4);
                    break;
                case 9:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Cardio[0], VanillaSkills.Cardio[1], 5);
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Parkour[0], VanillaSkills.Parkour[1], 4);
                    break;
                case 10:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Exercise[0], VanillaSkills.Exercise[1], 5);
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Parkour[0], VanillaSkills.Parkour[1], 5);
                    break;
            }
        }

        public Agitily(RealPlayer playerref, byte level, uint exp)
        {
            Player = playerref;
            Level = level;
            Exp = exp;
        }
    }
}
