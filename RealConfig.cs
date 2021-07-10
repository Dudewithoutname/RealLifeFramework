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
        public string IP;

        public string SkillIconURL;
        public string DefaulUserURL;

        public void LoadDefaults()
        {
            DatabaseServer = "127.0.0.1";
            DatabasePort = "3306";
            DatabaseName = "unturned";
            DatabaseUsername = "root";
            DatabasePassword = "";

            DiscordInvite = "https://discord.gg/zqF5PQYGpq";
            SteamGroupInvite = "https://store.steampowered.com/";
            StartingExp = 2500;
            Whisper = 5;
            Normal = 30;
            Shout = 60;
            IP = "127.0.0.1";

            SkillIconURL = "https://i.ibb.co/XYPQv2p/running.png";
            DefaulUserURL = "https://i.ibb.co/r3T5CPw/ico.png";
        }
    }
}