using Steamworks;
using SDG.Unturned;
using SDG.NetTransport;
using Rocket.Unturned.Player;
using RealLifeFramework.Skills;
using RealLifeFramework.UserInterface;
using RealLifeFramework.Chatting;
using RealLifeFramework.Patches;
using UnityEngine;
using Rocket.API;
using Newtonsoft.Json;
using System.IO;
using RealLifeFramework.Data;
using RealLifeFramework.Data.Models;

namespace RealLifeFramework.RealPlayers
{
    public class RealPlayer
    {
        // * Global
        public Player Player { get; private set; }
        public CSteamID CSteamID { get; private set; }
        public string IP { get; private set; }
        public ITransportConnection TransportConnection { get; private set; }

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
        public bool IsAdmin { get; set; }

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
            TransportConnection = player.Player.channel.GetOwnerTransportConnection();
            IP = TransportConnection.GetAddress().ToString();
            int ipEnd = IP.LastIndexOf(':') + 1;
            IP = IP.Substring(ipEnd, IP.Length - ipEnd);

            Name = data.name;
            Age = data.age;
            Gender = data.gender;

            Level = data.level;
            Exp = data.exp;

            IsAdmin = player.IsAdmin;

            SkillUser = new SkillUser(this, data.skillUser);

            Keyboard = new UnturnedKeyWatcher(player.Player);
            HUD = new HUD(this);

            ChatProfile = new ChatProfile(this, data.profileData);

            VoiceChat.Subscribe(this);
        }

        // New RealPlayer
        public RealPlayer(UnturnedPlayer player, string name, ushort age, byte gender)
        {
            Player = player.Player;
            CSteamID = player.CSteamID;
            TransportConnection = player.Player.channel.GetOwnerTransportConnection();
            IP = TransportConnection.GetAddress().ToString();
            int ipEnd = IP.LastIndexOf(':') + 1;
            IP = IP.Substring(ipEnd, IP.Length - ipEnd);

            Name = name;
            Age = age;
            SetGender(gender);

            Level = 1;
            Exp = 0;

            SkillUser = new SkillUser(this);
            

            Logger.Log($"[Characters] New Player : {Name}, {Age}, {Gender}");

            Keyboard = new UnturnedKeyWatcher(player.Player);
            HUD = new HUD(this);
            ChatProfile = new ChatProfile("#ffffff", UnturnedPlayer.FromCSteamID(player.CSteamID).SteamProfile.AvatarIcon.ToString(), EPlayerVoiceMode.Normal, this);

            VoiceChat.Subscribe(this);
            
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

            if (Exp >= MaxExp)
            {
                Exp -= MaxExp;
                levelUp();
            }

            HUD.UpdateComponent(HUDComponent.Exp);
        }

        private void levelUp()
        {
            Level++;

            HUD.UpdateComponent(HUDComponent.Exp);
            HUD.UpdateComponent(HUDComponent.Level);

        }
        #endregion

        #region GetRealPlayer

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
