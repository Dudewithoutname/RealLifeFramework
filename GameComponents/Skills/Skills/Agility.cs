using System;
using RealLifeFramework.RealPlayers;

namespace RealLifeFramework.Skills
{
    public sealed class Agitily : ISkill
    {
        public static readonly byte Id = 3;

        public RealPlayer Player { get; set; }
        public string Name => "Agilita";
        public byte MaxLevel => 12;
        public string Color => "#fffc3d";


        public byte Level { get; set; }
        public uint Exp { get; set; }


        public void AddExp(uint exp)
        {
            Exp += exp;

            if (Exp >= GetExpToNextLevel())
            {
                Exp -= GetExpToNextLevel();
                Upgrade();
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
                        return 750;
                    case 5:
                        return 1000;
                    case 6:
                        return 1500;
                    case 7:
                        return 2000;
                    case 8:
                        return 2500;
                    case 9:
                        return 3000;
                    case 10:
                        return 4000;
                    case 11:
                        return 5000;
                    case 12:
                        return 6000;
                    default:
                        return 0;
                }
            else
                return 0;
        }

        public void Upgrade()
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

            SkillManager.SendLevelUp(Player, Id);
        }

        public Agitily(RealPlayer playerref, byte level, uint exp)
        {
            Player = playerref;
            Level = level;
            Exp = exp;
        }
    }
}
