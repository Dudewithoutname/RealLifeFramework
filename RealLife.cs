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
using HarmonyLib;

namespace RealLifeFramework
{
    public class RealLife : RocketPlugin<RealConfig>
    {
        public static RealLife Instance;
        public static DatabaseManager Database = DatabaseManager.Instance();
        public Dictionary<CSteamID,RealPlayer> RealPlayers;
        public static bool Debugging = true;

        private Harmony harmony;

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
            Player.instance.
            harmony = new Harmony("RLFUnturned");
            harmony.PatchAll();

            if (Debugging)
                Database.Debug();
                        
            RealPlayers = new Dictionary<CSteamID, RealPlayer>();
            //   public static void say(CSteamID target, string text, Color color, EChatMode mode, bool isRich = false);
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
            harmony.UnpatchAll();
        }

    }
}
