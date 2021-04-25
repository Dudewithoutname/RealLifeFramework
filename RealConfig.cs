using Rocket.API;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace RealLifeFramework
{
    public class RealConfig : IRocketPluginConfiguration
    {
        public string DatabaseServer;
        public string DatabaseName;
        public string DatabaseUsername;
        public string DatabasePassword;
        public string DatabasePort;

        public string DiscordInvite;
        public string SteamGroupInvite;

        public void LoadDefaults()
        {
            DatabaseServer = "127.0.0.1";
            DatabasePort = "3306";
            DatabaseName = "unturned";
            DatabaseUsername = "root";
            DatabasePassword = "";
            DiscordInvite = "https://discord.com/";
            SteamGroupInvite = "https://store.steampowered.com/";
        }
    }
}