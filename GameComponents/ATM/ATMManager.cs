using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealLifeFramework.ATM
{
    [EventHandler]
    public class ATMManager : IEventComponent
    {
        public void HookEvents()
        {
            PlayerEquipment.OnPunch_Global += (equipment, punch) => onPunch(equipment.player);
        }

        private void onPunch(Player player)
        {
            // zajtra dorobim
        }
    }
}
