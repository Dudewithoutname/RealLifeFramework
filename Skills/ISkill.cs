using RealLifeFramework.Players;

namespace RealLifeFramework.Skills
{
    public interface ISkill
    {
        RealPlayer Player { get; set; }
        byte Level { get; set; }
        uint Exp { get; set; }


        string Name { get; }
        byte MaxLevel { get; }

        void LevelUp();
        void AddExp(uint exp); // check for levelup
        uint GetExpToNextLevel(); // level+1
    }
}
