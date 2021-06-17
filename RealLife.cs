using System;
using System.Collections.Generic;
using SDG.Unturned;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Events;
using Steamworks;
using RealLifeFramework.RealPlayers;
using RealLifeFramework.Items;
using HarmonyLib;
using RealLifeFramework.Data;

namespace RealLifeFramework
{
    public class RealLife : RocketPlugin<RealConfig>
    {
        public static RealLife Instance;
        public static DatabaseManager Database = DatabaseManager.Instance();
        public Dictionary<CSteamID,RealPlayer> RealPlayers;
        public static bool Debugging = false;
        private Harmony harmony;
        /*
                     new JobRank("Novice", 1, 0),
            new JobRank("Beginner", 2, 100),
            new JobRank("Regular", 3, 300),
            new JobRank("Intermediate", 4, 500),
            new JobRank("Advanced", 5, 1000),
            new JobRank("Expert", 6, 1500),
            new JobRank("Professional", 7, 2500),
            new JobRank("Master", 8, 5000),

         */
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

            DataManager.Settup();

            Database.IsConnect();
            Database.Setup();

            harmony = new Harmony("RLFUnturned");
            harmony.PatchAll();
            if (Debugging)
                Database.Debug();
                        
            RealPlayers = new Dictionary<CSteamID, RealPlayer>();

            EventManager.Load();
            RealPlayerCreation.Load();
            Level.onLevelLoaded += overrideServerStuff;

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

        private void overrideServerStuff(int level)
        {
            Provider.maxPlayers = 60;
            SteamGameServer.SetMaxPlayerCount(24);
            SteamGameServer.SetBotPlayerCount(0);
            SteamGameServer.SetKeyValue("rocketplugins",
            " | * DudeTurned Roleplay," +
            " | * Plugin: RealLifeFramework," +
           $" | * Version: {Assembly.GetName().Version}," +
            " | * Author: Dudewithoutname#3129" +
            "");
        }
    }
}
