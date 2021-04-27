using System;
using RealLifeFramework.Players;

namespace RealLifeFramework.Skills
{
    public class Fishing : ISkill
    {
        public static readonly byte Id = 2;

        public RealPlayer Player { get; set; }
        public string Name => nameof(Fishing);
        public byte MaxLevel => 5;

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
                        return 1000;
                    case 5:
                        return 2500;
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
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Fishing[0], VanillaSkills.Fishing[1], 1);
                    break;
                case 2:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Fishing[0], VanillaSkills.Fishing[1], 2);
                    break;
                case 3:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Fishing[0], VanillaSkills.Fishing[1], 3);
                    break;
                case 4:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Fishing[0], VanillaSkills.Fishing[1], 4);
                    break;
                case 5:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Fishing[0], VanillaSkills.Fishing[1], 5);
                    break;
            }
        }

        public Fishing(RealPlayer playerref, byte level, uint exp)
        {
            Player = playerref;
            Level = level;
            Exp = exp;
        }
    }
}
