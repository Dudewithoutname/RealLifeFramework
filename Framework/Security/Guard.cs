using System;
using System.Collections.Generic;
using SDG.Unturned;

namespace RealLifeFramework.Security
{
    [EventHandler("Security")]
    public class Guard : IEventComponent
    {
        public static Guard Instance;
        
        private void Load()
        {
            Instance = this;
            Logger.Log("[Guard] Loaded");
        }

        public void HookEvents()
        {
            Load();
            Provider.onEnemyConnected += CheckConnection;
        }

        public void CheckConnection(SDG.Unturned.SteamPlayer player)
        {
            string HWID = BitConverter.ToString(player.playerID.hwid);
            Logger.Log(HWID);
        }
    }
}
