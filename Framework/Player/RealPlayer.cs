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

namespace RealLifeFramework.RealPlayers
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

        // * Roleplay
        public SkillUser SkillUser { get; set; }

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

        // * Ultility | * References
        public HUD HUD { get; set; }
        public ChatProfile ChatProfile { get; set; }
        public UnturnedKeyWatcher Keyboard { get; set; }

        // * Economy
        public uint WalletMoney { get; set; } = 0;
        public uint CreditCardMoney
        {
            get => Player.skills.experience;
            set => Player.skills.ServerSetExperience(value);
        }

        // * Admin
        public bool IsAdmin { get; set; }

        public void SaveToJson()
        {
            var json = JsonConvert.SerializeObject(this);
            using (var writer = new StreamWriter("D:\\SteamLibrary\\steamapps\\common\\U3DS\\Servers\\Default\\Rocket\\DudeTurned_Data\\cigi.json")) 
            {
                writer.Write(json);
            }
        }

        public RealPlayer(UnturnedPlayer player, DBPlayerResult result)
        {
            Player = player.Player;
            CSteamID = player.CSteamID;
            TransportConnection = player.Player.channel.GetOwnerTransportConnection();
            IP = TransportConnection.GetAddress().ToString();
            int ipEnd = IP.LastIndexOf(':') + 1;
            IP = IP.Substring(ipEnd, IP.Length - ipEnd);

            Name = result.Name;
            Age = result.Age;
            SetGender(result.Gender);

            Level = result.Level;
            Exp = result.Exp;

            IsAdmin = player.IsAdmin;

            var skillResult = TPlayerSkills.GetSkillsInfo(this);
            SkillUser = new SkillUser(this, skillResult);

            Keyboard = new UnturnedKeyWatcher(player.Player);
            HUD = new HUD(this);
            ChatProfile = new ChatProfile("#ffffff", UnturnedPlayer.FromCSteamID(player.CSteamID).SteamProfile.AvatarIcon.ToString(), EPlayerVoiceMode.Normal, this);
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

            IsAdmin = player.IsAdmin;

            TPlayerInfo.NewPlayer(player.CSteamID.ToString(), name, age, gender);

            Logger.Log($"[Characters] New Player : {Name}, {Age}, {Gender}");

            Keyboard = new UnturnedKeyWatcher(player.Player);
            HUD = new HUD(this);
            ChatProfile = new ChatProfile("#ffffff", UnturnedPlayer.FromCSteamID(player.CSteamID).SteamProfile.AvatarIcon.ToString(), EPlayerVoiceMode.Normal, this);
            VoiceChat.Subscribe(this);
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

            RealLife.Database.set(TPlayerInfo.Name, CSteamID.ToString(), "exp", $"{Exp}");

            HUD.UpdateComponent(HUDComponent.Exp);
        }

        private void levelUp()
        {
            Level++;
            RealLife.Database.set(TPlayerInfo.Name, CSteamID.ToString(), "level", $"{Level}");

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
