using Rocket.API;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace RealLifeFramework
{
    public class RealConfig : IRocketPluginConfiguration
    {
        public string DiscordInvite;
        public string SteamGroupInvite;
        public uint StartingExp;

        public short Whisper;
        public short Normal;
        public short Shout;

        public void LoadDefaults()
        {
            DiscordInvite = "https://discord.com/";
            SteamGroupInvite = "https://store.steampowered.com/";
            StartingExp = 2500;
            Whisper = 5;
            Normal = 30;
            Shout = 60;
        }
    }
}