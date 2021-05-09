using RealLifeFramework.Players;

namespace RealLifeFramework.Skills
{
    public sealed class Crafting : IEducation
    {
        public static readonly byte Id = 2;

        public RealPlayer Player { get; set; }
        public string Name => nameof(Crafting);
        public byte MaxLevel => 3;
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

            PlayerSkills.UpdateEducation(Player.CSteamID, Id, Level);
        }

        public Crafting(RealPlayer playerRef, byte level)
        {
            Player = playerRef;
            Level = level;
        }
    }
}
