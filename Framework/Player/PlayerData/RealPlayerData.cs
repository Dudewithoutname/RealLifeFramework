using HarmonyLib;
using Newtonsoft.Json;
using RealLifeFramework.Chatting;
using RealLifeFramework.RealPlayers;
using RealLifeFramework.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealLifeFramework.Data.Models
{
    public class RealPlayerData : ISaveable
    {
        public string name { get; set; }
        public ushort age { get; set; }
        public string gender { get; set; }

        public ushort level { get; set; }
        public uint exp { get; set; }

        public SkillUserData skillUser { get; set; }
        public ProfileData profileData { get; set; }
        public bool isAdmin { get; set; }

        public uint walletMoney { get; set; }
        public uint creditcardMoney { get; set; }

        public static explicit operator RealPlayerData(RealPlayer player)
        {
            return new RealPlayerData()
            {
                name = player.Name,
                age = player.Age,
                gender = player.Gender,
                level = player.Level,
                exp = player.Exp,
                skillUser = (SkillUserData)player,
                profileData = (ProfileData)player.ChatProfile,
                isAdmin = player.IsAdmin,
                walletMoney = player.WalletMoney,
                creditcardMoney = player.CreditCardMoney,
            };
        }
    }
}
