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
using RealLifeFramework.SecondThread;
using RealLifeFramework.Framework.Data;

namespace RealLifeFramework
{
    public class RealLife : RocketPlugin<RealConfig>
    {
        public static RealLife Instance;
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
            Logger.Log("[Starting]- - - - - - - * RealLife * - - - - - - -");
            Logger.Log("[Author] : Dudewithoutname#3129");
            
            Instance = this;
            SecondaryThread.Start();

            DataManager.Settup();

            Database.Load();
            Database.Instance.Server = Configuration.Instance.DBServer;
            Database.Instance.DatabaseName = Configuration.Instance.DBDatabaseName;
            Database.Instance.UserName = Configuration.Instance.DBUserName;
            Database.Instance.Password = Configuration.Instance.DBPassword;
            Database.Instance.Port = Configuration.Instance.DBPort;            
            Database.Instance.IsConnect();
            Database.Instance.Setup();

            harmony = new Harmony("RLFUnturned");
            harmony.PatchAll();
                        
            RealPlayers = new Dictionary<CSteamID, RealPlayer>();

            EventManager.Load();
            RealPlayerCreation.Load();
            Level.onLevelLoaded += overrideServerStuff;

            Provider.onCommenceShutdown += () => saveData();
            InvokeRepeating(nameof(saveData), 1800f, 1800f);
            Logger.Log("[Finished]- - - - - - - * RealLife * - - - - - - -");
        }

        protected override void Unload()
        {
            Logger.Log("Unloading is unsupported!");
            harmony.UnpatchAll();
            Instance = null;
        }

        private void saveData()
        {
            SecondaryThread.Execute(() =>
            {
                Logger.Log("[Datamanager] Performing saving");

                foreach(var player in RealPlayers.Values)
                {
                    DataManager.SavePlayer(player);
                }

                Logger.Log("[Datamanager] Autosave finished");
            });
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
