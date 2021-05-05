using Steamworks;
using SDG.Unturned;
using SDG.NetTransport;
using Rocket.Unturned.Player;
using RealLifeFramework.Jobs;
using RealLifeFramework.Skills;
using RealLifeFramework.UserInterface;

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

        ///public ulong UntCoins { get; set; }

        // * Roleplay
        public JobUser JobUser { get; set; }
        public SkillUser SkillUser { get; set; }

        // * Leveling System
        public ushort Level { get; set; }
        public uint Exp { get; set; }
        public uint MaxExp { get; set; }

        // * Ultility | * References
        public uint Money { get; set; }
        public HUD HUD { get; set; }
        public string Avatar { get; set; }

        // * Admin
        public bool IsAdmin { get; set; }



        public RealPlayer(UnturnedPlayer player, DBPlayerResult result)
        {
            Player = player.Player;
            CSteamID = player.CSteamID;
            TransportConnection = player.Player.channel.GetOwnerTransportConnection();
            IP = TransportConnection.GetAddress().ToString();

            Name = result.Name;
            Age = result.Age;
            SetGender(result.Gender);
            Money = Player.skills.experience;
            PhoneNumber = "0"; // TODO : Mobile number

            Level = result.Level;
            Exp = result.Exp;
            MaxExp = GetExpForNextLevel();

            IsAdmin = player.IsAdmin;

            var jobResult = RealLife.Database.GetJobInfo(CSteamID);
            JobUser = new JobUser()
            {
                Job = jobResult.Job,
                Level = jobResult.Level,
                Exp = jobResult.Exp,
            };


            var skillResult = RealLife.Database.GetSkillsInfo(this);
            SkillUser = new SkillUser(this, skillResult);

            HUD = new HUD(this);

            Avatar = UnturnedPlayer.FromCSteamID(player.CSteamID).SteamProfile.AvatarIcon.ToString(); // Caching avatar
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
            Money = Player.skills.experience;
            PhoneNumber = "0"; // TODO : Mobile number

            Level = 1;
            Exp = 0;
            MaxExp = GetExpForNextLevel();

            JobUser = null;
            SkillUser = new SkillUser(this);
            HUD = new HUD(this);

            IsAdmin = player.IsAdmin;

            RealLife.Database.NewPlayer(player.CSteamID.ToString(), name, age, gender);

            Logger.Log($"[Characters] New Player : {Name}, {Age}, {Gender}");

            Avatar = UnturnedPlayer.FromCSteamID(player.CSteamID).SteamProfile.AvatarIcon.ToString(); // Caching avatar
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
                    Logger.Log("LGBT is unsupported"); // meme haha
                    break;
            }
        }

        #region Leveling System
        
        public void AddExp(uint exp)
        {
            Exp += exp;

            if (Exp >= MaxExp)
            {
                Exp -= MaxExp;
                levelUp();
            }

            RealLife.Database.set(DatabaseManager.TablePlayer, CSteamID.ToString(), "exp", $"{Exp}");

            HUD.UpdateExp();
        }

        private void levelUp()
        {
            MaxExp = GetExpForNextLevel();
            Level++;
            RealLife.Database.set(DatabaseManager.TablePlayer, CSteamID.ToString(), "level", $"{Level}");

            HUD.UpdateExp();
            HUD.UpdateLevel();

        }

        public uint GetExpForNextLevel()
        {
            if (Level < 10)
                return (uint)(100 * Level);
            else
                if (Level < 20)
                    return (uint)(150 * Level);
                else if(Level < 30)
                    return (uint)(175 * Level);
                else if(Level < 40)
                    return (uint)(200 * Level);
                else
                    return (uint)(250 * Level);
        }

        #endregion

    }
}
