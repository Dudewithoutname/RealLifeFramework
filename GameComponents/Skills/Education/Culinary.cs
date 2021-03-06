using RealLifeFramework.RealPlayers;

namespace RealLifeFramework.Skills
{
    public sealed class Culinary : IEducation
    {
        public static readonly byte Id = 1;

        public RealPlayer Player { get; set; }
        public string Name => "Kuchar";
        public byte MaxLevel => 3;
        public string Color => "#e38e4d";

        public byte Level { get; set; }

        public void Upgrade()
        {
            switch (++Level)
            {
                case 1:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Cooking[0], VanillaSkills.Cooking[1], 1);
                    break;
                case 2:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Cooking[0], VanillaSkills.Cooking[1], 2);
                    break;
                case 3:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Cooking[0], VanillaSkills.Cooking[1], 3);
                    break;
            }
        }

        public Culinary(RealPlayer playerRef, byte level)
        {
            Player = playerRef;
            Level = level;
        }
    }
}
