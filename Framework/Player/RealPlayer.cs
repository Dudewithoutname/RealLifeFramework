using Steamworks;
using SDG.Unturned;
using SDG.NetTransport;
using Rocket.Unturned.Player;
using RealLifeFramework.Skills;
using RealLifeFramework.UserInterface;
using RealLifeFramework.Chatting;
using RealLifeFramework.Patches;
using Rocket.API;
using RealLifeFramework.Data;
using RealLifeFramework.Data.Models;
using System.Collections.Generic;
using Rocket.Core;
using RealLifeFramework.Privileges;

namespace RealLifeFramework.RealPlayers
{
    public class RealPlayer
    {
        // * Global
        public Player Player { get; private set; }
        public CSteamID CSteamID { get; private set; }
        public string IP { get; private set; }
        public ITransportConnection TransportConnection => Player.channel.GetOwnerTransportConnection();
        public RealPlayerComponent Component { get; private set; }

        // * Privilige
        public RankUser RankUser { get; private set; }

        // * Character
        public string Name { get; set; }
        public ushort Age { get; set; }
        public string Gender { get; set; }

        // * Leveling System
        public ushort Level { get; set; }
        public uint Exp { get; set; }
        
        public uint MaxExp 
        { 
            get 
            {
                if (Level < 10)
                    return (uint)(100 * Level);
                else if (Level < 20)
                    return (uint)(150 * Level); 
                else if(Level < 30)
                    return (uint)(175 * Level);
                else if(Level < 40)
                    return (uint)(200 * Level);
                else
                    return (uint)(250 * Level);
            } 
        }

        // * Roleplay
        public SkillUser SkillUser { get; set; }

        // * Economy
        public uint WalletMoney { get; set; } = 0;
        public uint CreditCardMoney
        {
            get => Player.skills.experience;
            set => Player.skills.ServerSetExperience(value);
        }

        // * Ultility
        public HUD HUD { get; set; }
        public ChatProfile ChatProfile { get; set; }
        public UnturnedKeyWatcher Keyboard { get; set; }

        // * Jobs
        public string JobName { get; set; }
        public bool IsEMS => UnturnedPlayer.FromCSteamID(CSteamID).HasPermission("job.ems");
        public bool IsPolice => UnturnedPlayer.FromCSteamID(CSteamID).HasPermission("job.police");
        public byte PoliceLevel { get; set; }

        public RealPlayer(UnturnedPlayer player, RealPlayerData data)
        {
            Player = player.Player;
            CSteamID = player.CSteamID;
            IP = TransportConnection.GetAddress().ToString();
            int ipEnd = IP.LastIndexOf(':') + 1;
            IP = IP.Substring(ipEnd, IP.Length - ipEnd);

            RankUser = new RankUser(this);

            Name = data.name;
            Age = data.age;
            Gender = data.gender;

            Level = data.level;
            Exp = data.exp;

            Component = player.Player.gameObject.AddComponent<RealPlayerComponent>();
            Component.Player = this;

            SkillUser = new SkillUser(this, data.skillUser);

            Keyboard = new UnturnedKeyWatcher(player.Player);
            HUD = new HUD(this);

            ChatProfile = new ChatProfile(this, data.profileData);

            VoiceChat.Subscribe(this);

            if (RankUser.Admin != null)
            {
                if (RankUser.Admin.Value.Level >= 1)
                {
                    Player.look.sendFreecamAllowed(true);
                    Player.look.sendSpecStatsAllowed(true);
                }

                if (RankUser.Admin.Value.Level >= 2)
                {
                    Player.look.sendWorkzoneAllowed(true);
                }
            }

            ChatProfile.ChangeVoicemode(EPlayerVoiceMode.Normal, VoiceChat.Icons[(int)EPlayerVoiceMode.Normal]);
        }

        // New RealPlayer
        public RealPlayer(UnturnedPlayer player, string name, ushort age, byte gender)
        {
            Player = player.Player;
            CSteamID = player.CSteamID;
            IP = TransportConnection.GetAddress().ToString();
            int ipEnd = IP.LastIndexOf(':') + 1;
            IP = IP.Substring(ipEnd, IP.Length - ipEnd);

            RankUser = new RankUser(this);

            Name = name;
            Age = age;
            SetGender(gender);

            Level = 1;
            Exp = 0;

            Component = player.Player.gameObject.AddComponent<RealPlayerComponent>();
            Component.Player = this;

            SkillUser = new SkillUser(this);

            Logger.Log($"[Characters] New Player : {Name}, {Age}, {Gender}");

            Keyboard = new UnturnedKeyWatcher(player.Player);
            HUD = new HUD(this);
            ChatProfile = new ChatProfile(this);

            VoiceChat.Subscribe(this);

            if (RankUser.Admin != null)
            {
                if(RankUser.Admin.Value.Level >= 1)
                {
                    Player.look.sendFreecamAllowed(true);
                    Player.look.sendSpecStatsAllowed(true);
                }

                if (RankUser.Admin.Value.Level >= 2)
                {
                    Player.look.sendWorkzoneAllowed(true);
                }
            }

            ChatProfile.ChangeVoicemode(EPlayerVoiceMode.Normal, VoiceChat.Icons[(int)EPlayerVoiceMode.Normal]);

            DataManager.CreatePlayer(this);
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
            tryLevelUp();

            HUD.UpdateComponent(HUDComponent.Exp);
        }

        private void tryLevelUp()
        {
            if (Exp >= MaxExp)
            {
                Exp -= MaxExp;
                levelUp();
            }
        }

        private void levelUp()
        {
            Level++;
            SkillUser.AddEducationPoints(1);

            HUD.UpdateComponent(HUDComponent.Exp);
            HUD.UpdateComponent(HUDComponent.Level);
            
            tryLevelUp();
        }
        #endregion

        #region GetRealPlayer

        public static RealPlayer From(SteamPlayer player)
        {
            if (RealLife.Instance.RealPlayers.ContainsKey(player.playerID.steamID))
                return RealLife.Instance.RealPlayers[player.playerID.steamID];
            else
                return null;
        }

        public static RealPlayer From(CSteamID csteamid)
        {
            if (RealLife.Instance.RealPlayers.ContainsKey(csteamid))
                return RealLife.Instance.RealPlayers[csteamid];
            else
                return null;
        }

        public static RealPlayer From(UnturnedPlayer player)
        {
            if (RealLife.Instance.RealPlayers.ContainsKey(player.CSteamID))
                return RealLife.Instance.RealPlayers[player.CSteamID];
            else
                return null;
        }

        public static RealPlayer From(IRocketPlayer player)
        {
            var p = (UnturnedPlayer)player;

            if (RealLife.Instance.RealPlayers.ContainsKey(p.CSteamID))
                return RealLife.Instance.RealPlayers[p.CSteamID];
            else
                return null;

        }

        public static RealPlayer From(Player player)
        {
            CSteamID p = player.channel.owner.playerID.steamID;

            if (RealLife.Instance.RealPlayers.ContainsKey(p))
                return RealLife.Instance.RealPlayers[p];
            else
                return null;
        }

        #endregion
    }
}
