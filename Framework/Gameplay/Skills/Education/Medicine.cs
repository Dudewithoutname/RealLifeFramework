using RealLifeFramework.Players;

namespace RealLifeFramework.Skills
{
    public sealed class Medicine : IEducation
    {
        public static readonly byte Id = 3;

        public RealPlayer Player { get; set; }
        public string Name => nameof(Medicine);
        public byte MaxLevel => 7;
        public byte Level { get; set; }

        public void Upgrade()
        {
            switch (++Level)
            {
                case 1:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Healing[0], VanillaSkills.Healing[1], 1);
                    break;
                case 2:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Healing[0], VanillaSkills.Healing[1], 2);
                    break;
                case 3:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Healing[0], VanillaSkills.Healing[1], 3);
                    break;
                case 4:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Healing[0], VanillaSkills.Healing[1], 4);
                    break;
                case 5:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Healing[0], VanillaSkills.Healing[1], 5);
                    break;
                case 6:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Healing[0], VanillaSkills.Healing[1], 6);
                    break;
                case 7:
                    Player.Player.skills.ServerSetSkillLevel(VanillaSkills.Healing[0], VanillaSkills.Healing[1], 7);
                    break;
            }

            TPlayerSkills.UpdateEducation(Player.CSteamID, Id, Level);

        }

        public Medicine(RealPlayer playerRef, byte level)
        {
            Player = playerRef;
            Level = level;
        }
    }
}
