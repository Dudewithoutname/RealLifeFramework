using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealLifeFramework.ATM
{
    public class ATMSession
    {
        public Player Player;
        public string[] Data;
        public ATMCathegory CurrentCathegory;
        public bool depositAllCycle;
        public bool isDoingWork;

        public ATMSession(Player player)
        {
            Player = player;
            Data = new string[5];
            depositAllCycle = false;
            isDoingWork = false;
            CurrentCathegory = ATMCathegory.Menu;
        }
    }
}
