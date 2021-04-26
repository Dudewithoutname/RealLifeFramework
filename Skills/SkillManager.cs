using SDG.Unturned;
using RealLifeFramework.Players;
using RealLifeFramework.Skills;
using RealLifeFramework.Items;

namespace RealLifeFramework.Skills
{
    public static class SkillManager
    {
        public static void SendLevelUp(RealPlayer player, int skillId)
        {
            
        }

        public static void HandleStatIncremented(Player player, EPlayerStat stat)
        {
            var rplayer = RealPlayerManager.GetRealPlayer(player);

            switch (stat)
            {
                case EPlayerStat.FOUND_FISHES:
                    rplayer.SkillUser.AddExp(Fishing.Id, 2);
                    break;
                case EPlayerStat.TRAVEL_FOOT:
                    rplayer.SkillUser.AddExp(Agitily.Id, 2);
                    break;
                case EPlayerStat.FOUND_PLANTS:
                    rplayer.SkillUser.AddExp(Farming.Id, 2);
                    break;
            }
        }

        public static void HandleConsume(Player player, ItemConsumeableAsset consumeableAsset)
        {
            var rplayer = RealPlayerManager.GetRealPlayer(player);

            if (MedicalItems.Ids.Contains(consumeableAsset.id))
                rplayer.SkillUser.AddExp(Endurance.Id, 2);
        }
    }
}
