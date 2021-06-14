using System;
using System.Collections.Generic;
using RealLifeFramework.RealPlayers;

namespace RealLifeFramework.Economy
{
    public class EconomyUser
    {
        public RealPlayer Player { get; set; }
        public uint CreditCardMoney { get; set; }
        public uint WalletMoney { get; set; }

        public EconomyUser(RealPlayer player)
        {
            Player = player;
        }
    }
}
