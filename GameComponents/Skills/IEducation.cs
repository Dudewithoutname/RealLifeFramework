using RealLifeFramework.RealPlayers;

namespace RealLifeFramework.Skills
{
    public interface IEducation
    {
        RealPlayer Player { get; set; }
        byte Level { get; set; }

        string Name { get; }
        string Color { get; }
        byte MaxLevel { get; }

        void Upgrade();

    }
}
