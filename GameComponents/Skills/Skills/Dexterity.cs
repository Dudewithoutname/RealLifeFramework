using System;
using RealLifeFramework.RealPlayers;

namespace RealLifeFramework.Skills
{
    public sealed class Dexterity : ISkill
    {
        public static readonly byte Id = 4;

        public RealPlayer Player { get; set; }
        public string Name => "Obratnost";
        public byte MaxLevel => 6;
        public string Color => "#f28749";

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
                    case 6:
                        return 2000;
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
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Dexerity[0], VanillaSkills.Dexerity[1], 1);
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Outdoors[0], VanillaSkills.Outdoors[1], 1);
                    break;
                case 2:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Dexerity[0], VanillaSkills.Dexerity[1], 2);
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Outdoors[0], VanillaSkills.Outdoors[1], 2);
                    break;
                case 3:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Dexerity[0], VanillaSkills.Dexerity[1], 3);
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Outdoors[0], VanillaSkills.Outdoors[1], 3);
                    break;
                case 4:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Dexerity[0], VanillaSkills.Dexerity[1], 4);
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Outdoors[0], VanillaSkills.Outdoors[1], 4);
                    break;
                case 5:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Outdoors[0], VanillaSkills.Outdoors[1], 5);
                    break;
                case 6:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Dexerity[0], VanillaSkills.Dexerity[1], 5);
                    break;
            }
        }

        public Dexterity(RealPlayer playerref, byte level, uint exp)
        {
            Player = playerref;
            Level = level;
            Exp = exp;
        }
    }
}
