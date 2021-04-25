using Steamworks;
using SDG.Unturned;
using SDG.NetTransport;
using Rocket.Unturned.Player;
using RealLifeFramework.Jobs;
using RealLifeFramework.Skills;

namespace RealLifeFramework.Players
{
    public class RealPlayer
    {
        // * Global
        public Player Player { get; set; }
        public CSteamID CSteamID { get; set; }
        public string IP { get; set; }
        public ITransportConnection TransportConnection { get; set; }

        // * Character
        public string Name { get; set; }
        public ushort Age { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }

        // * Currency
        public ulong Money { get; set; }
        ///public ulong UntCoins { get; set; }

        // * Roleplay
        public ushort Level { get; set; }
        public uint Exp { get; set; }


        public JobUser JobUser { get; set; }
        public SkillUser SkillUser { get; set; }


        public RealPlayer(UnturnedPlayer player, DBPlayerResult result)
        {
            Player = player.Player;
            CSteamID = player.CSteamID;
            TransportConnection = player.Player.channel.GetOwnerTransportConnection();
            IP = TransportConnection.GetAddress().ToString();

            Name = result.Name;
            Age = result.Age;
            SetGender(result.Gender);
            PhoneNumber = "0"; // TODO : Mobile number

            Level = result.Level;
            Exp = result.Exp;

            var jobResult = RealLife.Database.GetJobInfo(CSteamID);

            JobUser = new JobUser()
            {
                Job = jobResult.Job,
                Level = jobResult.Level,
                Exp = jobResult.Exp,
            };


            var skillResult = RealLife.Database.GetSkillsInfo(this);
            SkillUser = new SkillUser(this, skillResult);
        }

        // New RealPlayer
        public RealPlayer(UnturnedPlayer player, string name, ushort age, byte gender)
        {
            Player = player.Player;
            CSteamID = player.CSteamID;
            TransportConnection = player.Player.channel.GetOwnerTransportConnection();
            IP = TransportConnection.GetAddress().ToString();

            Name = name;
            Age = age;
            SetGender(gender);
            PhoneNumber = "0"; // TODO : Mobile number

            Level = 1;
            Exp = 0;
            JobUser = null;
            SkillUser = new SkillUser(this);

            RealLife.Database.NewPlayer(player.CSteamID.ToString(), name, age, gender);
            Logger.Log($"[Characters] New Player : {Name}, {Age}, {Gender}");
            // discord new player info
        }

        
        public void SetGender(byte gender)
        {
            switch (gender)
            { 
                case 0:
                    Gender = "Male";
                    break;
                case 1:
                    Gender = "Female";
                    break;
                default:
                    Logger.Log("LGBT is unsupported");
                    break;
            }
        }

    }
}
