using System;
using RealLifeFramework.RealPlayers;

namespace RealLifeFramework.Skills
{
    public sealed class Farming : ISkill
    {
        public static readonly byte Id = 1;

        public RealPlayer Player { get; set; }
        public string Name => "Farmenie";
        public byte MaxLevel => 7;
        public string Color => "#78ff47";

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
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Agriculture[0], VanillaSkills.Agriculture[1], 1);
                    break;
                case 2:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Agriculture[0], VanillaSkills.Agriculture[1], 2);
                    break;
                case 3:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Agriculture[0], VanillaSkills.Agriculture[1], 3);
                    break;
                case 4:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Agriculture[0], VanillaSkills.Agriculture[1], 4);
                    break;
                case 5:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Agriculture[0], VanillaSkills.Agriculture[1], 5);
                    break;
                case 6:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Agriculture[0], VanillaSkills.Agriculture[1], 6);
                    break;
                case 7:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Agriculture[0], VanillaSkills.Agriculture[1], 7);
                    break;
            }
        }

        public Farming(RealPlayer playerref, byte level, uint exp)
        {
            Player = playerref;
            Level = level;
            Exp = exp;
        }
    }
}
