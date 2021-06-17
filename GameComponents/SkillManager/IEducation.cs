using Newtonsoft.Json;
using RealLifeFramework.RealPlayers;

namespace RealLifeFramework.Skills
{
    public interface IEducation
    {
        [JsonIgnore]
        RealPlayer Player { get; set; }
        byte Level { get; set; }

        [JsonIgnore]
        string Name { get; }
        [JsonIgnore]
        byte MaxLevel { get; }

        void Upgrade();

    }
}
