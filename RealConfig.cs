using RealLifeFramework.Database;
using Rocket.API;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace RealLifeFramework
{
    public class RealConfig : IRocketPluginConfiguration
    {
        public ConnectionString ConString;
        public string DiscordInvite;
        public string SteamGroupInvite;
        public uint StartingExp;

        public short Whisper;
        public short Normal;
        public short Shout;

        public string IP;

        public string SkillIconURL;
        public string DefaulUserURL;

        public Vector3 CarSpawnPoint;
        public uint CarShopSignIID;

        public ushort TaserID;
        public float TasedTime;

        public void LoadDefaults()
        {
            ConString = new ConnectionString()
            {
                Server = "127.0.0.1",
                Database = "unturned",
                Username = "root",
                Password = "",
                Port = "3306",
            };

            DiscordInvite = "https://discord.gg/zqF5PQYGpq";
            SteamGroupInvite = "https://steamcommunity.com/groups/dturnednoscamnohack69";
            StartingExp = 2500;
            Whisper = 5;
            Normal = 30;
            Shout = 60;

            IP = "157.90.138.191";

            SkillIconURL = "https://i.ibb.co/XYPQv2p/running.png";
            DefaulUserURL = "https://i.ibb.co/r3T5CPw/ico.png";

            CarSpawnPoint = new Vector3(0, 0, 0);
            CarShopSignIID = 1457;

            TaserID = 63026;
            TasedTime = 12;
        }
    }
}