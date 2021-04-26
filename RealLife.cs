using System;
using System.Collections.Generic;
using SDG.Unturned;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Events;
using Steamworks;
using RealLifeFramework.Players;
using RealLifeFramework.Jobs;
using RealLifeFramework.Items;

namespace RealLifeFramework
{
    public class RealLife : RocketPlugin<RealConfig>
    {
        public static RealLife Instance;
        public static DatabaseManager Database = DatabaseManager.Instance();
        public Dictionary<CSteamID,RealPlayer> RealPlayers;
        public static bool Debugging = true;

        protected override void Load()
        {
            Instance = this;
            Logger.Log("[Starting]- - - - - - - * RealLife * - - - - - - -");
            Logger.Log("[Author] : Dudewithoutname#3129");

            Database.Server = Configuration.Instance.DatabaseServer;
            Database.DatabaseName = Configuration.Instance.DatabaseName;
            Database.UserName = Configuration.Instance.DatabaseUsername;
            Database.Password = Configuration.Instance.DatabasePassword;
            Database.Port = Configuration.Instance.DatabasePort;
            Database.IsConnect();
            Database.Setup();

            if (Debugging)
                Database.Debug();

            //Discord.SendDiscord("Server Testing kokot");
                        
            RealPlayers = new Dictionary<CSteamID, RealPlayer>();

            EventManager.Load();
            JobManager.Load();
            RealPlayerCreation.Load();

            Logger.Log("[Finished]- - - - - - - * RealLife * - - - - - - -");
        }

        protected override void Unload()
        {
            Logger.Log("[Unloading] - - - [RealLife Framework]");
            Database.Close();
            Database = null;
            Instance = null;
        }

    }
}
