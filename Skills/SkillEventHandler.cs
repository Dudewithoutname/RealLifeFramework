using SDG.Unturned;
using RealLifeFramework.Players;
using RealLifeFramework.Skills;

namespace RealLifeFramework.Skills
{
    public static class SkillEventHandler
    {
        public static void HandleStatEvent(Player player, EPlayerStat stat)
        {
            switch (stat)
            {
                case EPlayerStat.FOUND_FISHES:
                    // Fisherman Increment
                    break;
                case EPlayerStat.TRAVEL_FOOT:
                    // Stamina increment but check if not op
                    break;
            }
        }

        private static void incrementSkill(Player player, int id, uint amount)
        {
            var rplayer = RealPlayerManager.GetRealPlayer(player);
            rplayer.SkillUser.AddExp(id, amount);
        }
    }
}
