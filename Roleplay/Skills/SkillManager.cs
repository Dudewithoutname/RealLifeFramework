using SDG.Unturned;
using RealLifeFramework.Players;
using RealLifeFramework.Skills;
using RealLifeFramework.Items;

namespace RealLifeFramework.Skills
{
    [EventHandler("SkillManager")]
    public class SkillManager : IEventComponent
    {
        public void HookEvents()
        {
            Player.onPlayerStatIncremented += HandleStatIncremented;
            UseableConsumeable.onConsumePerformed += HandleConsume;
        }

        public static void SendLevelUp(RealPlayer player, int skillId)
        {
            var skill = player.SkillUser.Skills[skillId];
            
            player.AddExp(5);

            if (RealLife.Debugging)
                Logger.Log($"Debug: levelUp {skill.Name} , {skill.Level} , {skill.Exp}");
        }

        public static void HandleStatIncremented(Player player, EPlayerStat stat)
        {
            var rplayer = RealPlayerManager.GetRealPlayer(player);

            switch (stat)
            {
                case EPlayerStat.FOUND_FISHES:
                    rplayer.SkillUser.AddExp(Fishing.Id, 50);
                    break;
                case EPlayerStat.TRAVEL_FOOT: // Fix This
                    rplayer.SkillUser.AddExp(Agitily.Id, 50);
                    break;
                case EPlayerStat.FOUND_PLANTS:
                    rplayer.SkillUser.AddExp(Farming.Id, 50);
                    break;
                case EPlayerStat.FOUND_RESOURCES:
                    rplayer.SkillUser.AddExp(Dexterity.Id, 50);
                    break;
            }
        }

        public static void HandleConsume(Player player, ItemConsumeableAsset consumeableAsset)
        {
            var rplayer = RealPlayerManager.GetRealPlayer(player);

            if (MedicalItems.Ids.Contains(consumeableAsset.id))
                rplayer.SkillUser.AddExp(Endurance.Id, 50);
        }
    }
}
