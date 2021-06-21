using Rocket.API;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace RealLifeFramework
{
    public class RealConfig : IRocketPluginConfiguration
    {
        public string DBServer;
        public string DBDatabaseName;
        public string DBUserName;
        public string DBPassword;
        public string DBPort;

        public string DiscordInvite;
        public string SteamGroupInvite;
        public uint StartingExp;
        
        public short Whisper;
        public short Normal;
        public short Shout;

        public void LoadDefaults()
        {
            DBServer = "127.0.0.1";
            DBDatabaseName = "unturned";
            DBUserName = "root";
            DBPassword = "";
            DBPort = "3306";
            DiscordInvite = "https://discord.com/";
            SteamGroupInvite = "https://store.steampowered.com/";
            StartingExp = 2500;
            Whisper = 5;
            Normal = 30;
            Shout = 60;
        }
    }
}