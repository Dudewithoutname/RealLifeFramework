using RealLifeFramework.Players;

namespace RealLifeFramework.Skills
{
    public class Defense : IEducation
    {
        public static readonly byte Id = 4;

        public RealPlayer Player { get; set; }
        public string Name => "Martial Art & Weapons";
        public byte MaxLevel => 10;
        public byte Level { get; set; }

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

            RealLife.Database.UpdateEducation(Player.CSteamID, Id, Level);

        }

        public Defense(RealPlayer playerRef, byte level)
        {
            Player = playerRef;
            Level = level;
        }
    }
}
