using System.Collections.Generic;
using SDG.Unturned;
using Rocket.Core.Plugins;
using Steamworks;
using RealLifeFramework.RealPlayers;
using HarmonyLib;
using RealLifeFramework.Data;
using RealLifeFramework.Threadding;
using RealLifeFramework.Database;
using RealLifeFramework.Pointmap;

namespace RealLifeFramework
{
    public class RealLife : RocketPlugin<RealConfig>
    {
        public static RealLife Instance;
        public static bool Debugging = false;
        public Dictionary<CSteamID, RealPlayer> RealPlayers;
        private Harmony _harmony;

        // Novice, Beginner, Regular, Intermediate, Advanced, Expert, Professional, Master

        protected override void Load()
        {
            
            Logger.Log("[Starting]- - - - - - - * RealLife * - - - - - - -");
            Logger.Log("[Author] : Dudewithoutname#3129");
            Instance = this;
            
            DataManager.Settup();

            DatabaseManager.Create();
            //DatabaseManager.Singleton.Clear();

            _harmony = new Harmony("RLFUnturned");
            _harmony.PatchAll();
                        
            RealPlayers = new Dictionary<CSteamID, RealPlayer>();

            EventManager.Load();
            RealPlayerCreation.Load();
            gameObject.AddComponent<PointmapManager>();

            Level.onLevelLoaded += onServerLoaded;
            Provider.onCommenceShutdown += saveData;
            InvokeRepeating(nameof(saveData), 1800f, 1800f);

            CommandWindow.shouldLogJoinLeave = false;

            Logger.Log("[Finished]- - - - - - - * RealLife * - - - - - - -");
        }

        protected override void Unload()
        {
            Logger.Log("Unloading is unsupported!");
            _harmony.UnpatchAll();
            Instance = null;
        }

        private void saveData()
        {
            ThreadHelper.Execute( () =>
            {
                Logger.Log("[Datamanager] Performing saving");

                foreach(var player in RealPlayers.Values)
                {
                    DataManager.SavePlayer(player);
                }

                Logger.Log("[Datamanager] Autosave finished");
            });
        }

        private void onServerLoaded(int level)
        {
            Provider.maxPlayers = 74;
            SteamGameServer.SetServerName("CZ/SK | DudeTurned Roleplay RP");
            SteamGameServer.SetGameDescription("<color=#fb9d8f>| 0 Hracov | 0 EMS | 0 PD |</color>");
            SteamGameServer.SetMaxPlayerCount(24);
            SteamGameServer.SetBotPlayerCount(0);
            SteamGameServer.SetKeyValue("pf", "rm");
            SteamGameServer.SetKeyValue("rocketplugins",
            "Dudewithoutname#3129" +
            ",hmm, potrebujes nieco ? :D");

            DiscordBotManager.Load();
            InvokeRepeating(nameof(updateStats), 20f, 20f);
        }

        private void updateStats() => DiscordBotManager.UpdateServerStats();
    }
}
