using Newtonsoft.Json;
using RealLifeFramework.RealPlayers;
using RealLifeFramework.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealLifeFramework.Data.Models
{
    public class RealPlayerData
    {
        public string Name { get; set; }
        public ushort Age { get; set; }
        public string Gender { get; set; }

        public ushort Level { get; set; }
        public uint Exp { get; set; }

        public SkillUserData SkillUser { get; set; }
        public bool IsAdmin { get; set; }

        public uint WalletMoney { get; set; }
        public uint CreditCardMoney { get; set; }

        public static explicit operator RealPlayerData(RealPlayer player)
        {
            return new RealPlayerData()
            {
                Name = player.Name,
                Age = player.Age,
                Gender = player.Gender,
                Level = player.Level,
                Exp = player.Exp,
                SkillUser = new SkillUserData() 
                { 
                    EducationPoints = player.SkillUser.EducationPoints,
                    Skills = player.SkillUser.Skills.ToArray(),
                    Educations = player.SkillUser.Educations.ToArray(),
                },
                IsAdmin = player.IsAdmin,
                WalletMoney = player.WalletMoney,
                CreditCardMoney = player.CreditCardMoney,
            };
        }

        [JsonConstructor]
        public RealPlayerData(string name, ushort age, string gender, ushort level, uint exp, SkillUserData skillUser, bool isAdmin, uint walletMoney, uint creditCardMoney)
        {
            Name = name;
            Age = age;
            Gender = gender;
            Level = level;
            Exp = exp;
            SkillUser = skillUser;
            IsAdmin = isAdmin;
            WalletMoney = walletMoney;
            CreditCardMoney = creditCardMoney;
        }

        public RealPlayerData() { }
    }
}
