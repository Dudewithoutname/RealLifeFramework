using RealLifeFramework.RealPlayers;

namespace RealLifeFramework.Skills
{
    public sealed class Crafting : IEducation
    {
        public static readonly byte Id = 2;

        public RealPlayer Player { get; set; }
        public string Name => "Remeselnik";
        public byte MaxLevel => 3;
        public string Color => "#ffe24f";

        public byte Level { get; set; }

        public void Upgrade()
        {
            switch (++Level)
            {
                case 1:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Crafting[0], VanillaSkills.Crafting[1], 1);
                    break;
                case 2:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Crafting[0], VanillaSkills.Crafting[1], 2);
                    break;
                case 3:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Crafting[0], VanillaSkills.Crafting[1], 3);
                    break;
            }
        }

        public Crafting(RealPlayer playerRef, byte level)
        {
            Player = playerRef;
            Level = level;
        }
    }
}
