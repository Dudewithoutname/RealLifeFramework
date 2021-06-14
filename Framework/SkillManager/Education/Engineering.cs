using RealLifeFramework.RealPlayers;

namespace RealLifeFramework.Skills
{
    public sealed class Engineering : IEducation
    {
        public static readonly byte Id = 0;

        public RealPlayer Player { get; set; }
        public string Name => nameof(Engineering);
        public byte MaxLevel => 5;
        public byte Level { get; set; }

        public void Upgrade()
        {
            switch (++Level)
            {
                case 1:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Mechanic[0], VanillaSkills.Mechanic[1], 1);
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Engineer[0], VanillaSkills.Engineer[1], 1);
                    break;
                case 2:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Mechanic[0], VanillaSkills.Mechanic[1], 2);
                    break;
                case 3:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Mechanic[0], VanillaSkills.Mechanic[1], 3);
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Engineer[0], VanillaSkills.Engineer[1], 2);
                    break;
                case 4:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Mechanic[0], VanillaSkills.Mechanic[1], 4);
                    break;
                case 5:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Mechanic[0], VanillaSkills.Mechanic[1], 5);
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Engineer[0], VanillaSkills.Engineer[1], 3);
                    break;
            }

            TPlayerSkills.UpdateEducation(Player.CSteamID, Id, Level);

        }

        public Engineering(RealPlayer playerRef, byte level)
        {
            Player = playerRef;
            Level = level;
        }
    }
}
