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
        public uint StartingExp;

        public short Whisper;
        public short Normal;
        public short Shout;

        public void LoadDefaults()
        {
            DatabaseServer = "127.0.0.1";
            DatabasePort = "3306";
            DatabaseName = "unturned";
            DatabaseUsername = "root";
            DatabasePassword = "";
            DiscordInvite = "https://discord.com/";
            SteamGroupInvite = "https://store.steampowered.com/";
            StartingExp = 2500;
            Whisper = 50;
            Normal = 100;
            Shout = 150;
        }
    }
}