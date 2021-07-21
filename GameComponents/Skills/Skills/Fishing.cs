using System;
using RealLifeFramework.RealPlayers;

namespace RealLifeFramework.Skills
{
    public sealed class Fishing : ISkill
    {
        public static readonly byte Id = 2;

        public RealPlayer Player { get; set; }
        public string Name => "Rybarenie";
        public byte MaxLevel => 5;
        public string Color => "#47e0ff";

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
                        return 1000;
                    default:
                        return UInt32.MaxValue;
                }
            else
                return UInt32.MaxValue;
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
