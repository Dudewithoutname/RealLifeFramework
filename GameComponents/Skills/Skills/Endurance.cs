using System;
using RealLifeFramework.RealPlayers;

namespace RealLifeFramework.Skills
{
    public sealed class Endurance : ISkill
    {
        public static readonly byte Id = 0;

        public RealPlayer Player { get; set; }
        public string Name => "Vydrz";
        public byte MaxLevel => 10;
        public string Color => "#ff4747";

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
                        return 50;
                    case 2:
                        return 100;
                    case 3:
                        return 250;
                    case 4:
                        return 500;
                    case 5:
                        return 750;
                    case 6:
                        return 1000;
                    case 7:
                        return 1500;
                    case 8:
                        return 2000;
                    case 9:
                        return 2500;
                    case 10:
                        return 3000;
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
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Vitality[0], VanillaSkills.Vitality[1], 1);
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Immunity[0], VanillaSkills.Immunity[1], 1);
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Warmblooded[0], VanillaSkills.Warmblooded[1], 1);
                    break;
                case 2:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Survival[0], VanillaSkills.Survival[1], 1);
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Strength[0], VanillaSkills.Strength[1], 1);
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Toughness[0], VanillaSkills.Toughness[1], 1);
                    break;
                case 3:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Vitality[0], VanillaSkills.Vitality[1], 2);
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Immunity[0], VanillaSkills.Immunity[1], 2);
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Warmblooded[0], VanillaSkills.Warmblooded[1], 2);
                    break;
                case 4:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Survival[0], VanillaSkills.Survival[1], 2);
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Strength[0], VanillaSkills.Strength[1], 2);
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Toughness[0], VanillaSkills.Toughness[1], 2);
                    break;
                case 5:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Vitality[0], VanillaSkills.Vitality[1], 3);
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Immunity[0], VanillaSkills.Immunity[1], 3);
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Warmblooded[0], VanillaSkills.Warmblooded[1], 3);
                    break;
                case 6:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Warmblooded[0], VanillaSkills.Warmblooded[1], 4);
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Survival[0], VanillaSkills.Survival[1], 3);
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Toughness[0], VanillaSkills.Toughness[1], 3);
                    break;
                case 7:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Survival[0], VanillaSkills.Survival[1], 4);
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Strength[0], VanillaSkills.Strength[1], 3);
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Toughness[0], VanillaSkills.Toughness[1], 4);
                    break;
                case 8:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Vitality[0], VanillaSkills.Vitality[1], 4);
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Immunity[0], VanillaSkills.Immunity[1], 4);
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Warmblooded[0], VanillaSkills.Warmblooded[1], 5);
                    break;
                case 9:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Survival[0], VanillaSkills.Survival[1], 5);
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Strength[0], VanillaSkills.Strength[1], 4);
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Toughness[0], VanillaSkills.Toughness[1], 5);
                    break;
                case 10:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Vitality[0], VanillaSkills.Vitality[1], 5);
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Immunity[0], VanillaSkills.Immunity[1], 5);
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Strength[0], VanillaSkills.Strength[1], 5);
                    break;
            }
        }

        public Endurance(RealPlayer playerref, byte level, uint exp)
        {
            Player = playerref;
            Level = level;
            Exp = exp;
        }
    }
}
