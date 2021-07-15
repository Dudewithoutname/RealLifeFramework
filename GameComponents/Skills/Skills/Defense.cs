using RealLifeFramework.RealPlayers;
using System;

namespace RealLifeFramework.Skills
{
    public sealed class Defense : ISkill
    {
        public static readonly byte Id = 5;

        public RealPlayer Player { get; set; }
        public string Name => "Sila";
        public byte MaxLevel => 10;
        public string Color => "#c847ff";

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
                        return 5000;
                    default:
                        return 100;
                }
            else
                return 0;
        }

        public void Upgrade()
        {
           
            switch (++Level)
            {
                case 1:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Overkill[0], VanillaSkills.Overkill[1], 1);
                    break;
                case 2:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Sharpshooter[0], VanillaSkills.Sharpshooter[1], 1);
                    break;
                case 3:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Overkill[0], VanillaSkills.Overkill[1], 2);
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Sharpshooter[0], VanillaSkills.Sharpshooter[1], 2);
                    break;
                case 4:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Overkill[0], VanillaSkills.Overkill[1], 3);
                    break;
                case 5:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Overkill[0], VanillaSkills.Overkill[1], 4);
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Sharpshooter[0], VanillaSkills.Sharpshooter[1], 3);
                    break;
                case 6:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Sharpshooter[0], VanillaSkills.Sharpshooter[1], 4);
                    break;
                case 7:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Overkill[0], VanillaSkills.Overkill[1], 5);
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Sharpshooter[0], VanillaSkills.Sharpshooter[1], 5);
                    break;
                case 8:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Overkill[0], VanillaSkills.Overkill[1], 6);
                    break;
                case 9:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Sharpshooter[0], VanillaSkills.Sharpshooter[1], 6);
                    break;
                case 10:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Overkill[0], VanillaSkills.Overkill[1], 7);
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Sharpshooter[0], VanillaSkills.Sharpshooter[1], 7);
                    break;
            }

            SkillManager.SendLevelUp(Player, Id);
        }

        public Defense(RealPlayer playerRef, byte level, uint exp)
        {
            Player = playerRef;
            Level = level;
            Exp = exp;
        }
    }
}
