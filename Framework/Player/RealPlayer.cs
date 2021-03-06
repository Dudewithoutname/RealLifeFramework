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
using RealLifeFramework.Ranks;

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
                if (Level < 20)
                    return (uint)(150 * Level);
                if (Level < 30)
                    return (uint)(175 * Level);
                if (Level < 40)
                    return (uint)(200 * Level);
                
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

        public RealPlayer(UnturnedPlayer player, RealPlayerData data)
        {
            Player = player.Player;
            CSteamID = player.CSteamID;
            IP = TransportConnection.GetAddress().ToString();
            var ipEnd = IP.LastIndexOf(':') + 1;
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

            R.Permissions.AddPlayerToGroup("unemployed", player);
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

        public static RealPlayer From(SteamPlayer player) => RealLife.Instance.RealPlayers.ContainsKey(player.playerID.steamID) ? RealLife.Instance.RealPlayers[player.playerID.steamID] : null;

        public static RealPlayer From(CSteamID csteamid) => RealLife.Instance.RealPlayers.ContainsKey(csteamid) ? RealLife.Instance.RealPlayers[csteamid] : null;

        public static RealPlayer From(UnturnedPlayer player) => RealLife.Instance.RealPlayers.ContainsKey(player.CSteamID) ? RealLife.Instance.RealPlayers[player.CSteamID] : null;

        public static RealPlayer From(IRocketPlayer player) => RealLife.Instance.RealPlayers.ContainsKey(((UnturnedPlayer)player).CSteamID) ? RealLife.Instance.RealPlayers[((UnturnedPlayer)player).CSteamID] : null;

        public static RealPlayer From(Player player) => RealLife.Instance.RealPlayers.ContainsKey(player.channel.owner.playerID.steamID) ? RealLife.Instance.RealPlayers[player.channel.owner.playerID.steamID] : null;

        #endregion
    }
}
