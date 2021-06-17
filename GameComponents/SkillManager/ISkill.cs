using RealLifeFramework.RealPlayers;
using Newtonsoft.Json;

namespace RealLifeFramework.Skills
{
    public interface ISkill
    {
        [JsonIgnore]
        RealPlayer Player { get; set; }
        byte Level { get; set; }
        uint Exp { get; set; }

        [JsonIgnore]
        string Name { get; }
        [JsonIgnore]
        byte MaxLevel { get; }

        void Upgrade();
        void AddExp(uint exp); 
        uint GetExpToNextLevel();
    }
}
