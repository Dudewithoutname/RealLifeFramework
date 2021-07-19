using Newtonsoft.Json;
using RealLifeFramework.API.Models;
using RealLifeFramework.Patches;
using RealLifeFramework.Ranks;
using RealLifeFramework.RealPlayers;
using RealLifeFramework.UserInterface;
using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using UnityEngine;

namespace RealLifeFramework
{
    public class DiscordBotManager
    {
        public static void Load()
        {
            UpdateServerStats();
            Provider.onCommenceShutdown += onShutDown;
            ChatManager.onChatted += onChatted;
        }
        
        private static void onChatted(SteamPlayer player, EChatMode mode, ref Color chatted, ref bool isRich, string text, ref bool isVisible)
        {
            var rp = RealPlayer.From(player);

            string color = "#5c708a";

            if (text.StartsWith("/"))
                color = "#828a4d";

            Api.Send("/logs/chat", JsonConvert.SerializeObject(
                new ChatMessage()
                {
                    avatar = rp.ChatProfile.Avatar,
                    color = color,
                    message = text,
                    name = $"[{(rp.RankUser.DisplayRankName)}] {rp.Name} ({rp.CSteamID})",
                    steamId = rp.CSteamID.ToString(),
                }
            ));
        }

        public static void UpdateServerStats()
        {
            int playerCount = 0;
            int pdCount = 0;
            int emsCount = 0;

            foreach (SteamPlayer player in Provider.clients)
            {
                playerCount++;

                var up = UnturnedPlayer.FromSteamPlayer(player);

                if (!up.IsAdmin)
                {
                    if (up.HasPermission(RankManager.EMSPermission))
                    {
                        emsCount++;
                    }

                    if (up.HasPermission(RankManager.PolicePermission))
                    {
                        pdCount++;
                    }
                }
                else
                {
                    var rp = RealPlayer.From(up);
                    
                    if (rp == null) continue;

                    if (rp.RankUser.Job.Id.Contains("ems"))
                    {
                        emsCount++;
                    }

                    if (rp.RankUser.Job.Id.Contains("pd"))
                    {
                        pdCount++;
                    }
                }
            }

            // message
            Api.Send("/info/stats", JsonConvert.SerializeObject(
                new Stats()
                {
                    online = true,
                    players = playerCount,
                    pd = pdCount,
                    ems = emsCount,
                    serverIP = RealLife.Instance.Configuration.Instance.IP,
                    serverPort = Provider.port.ToString()
                }
            ));

            // tab
            Api.Send("/info/tab", JsonConvert.SerializeObject(
                new Tab()
                {
                    players = playerCount,
                    time = $"{HUD.FormatTime(Patches.Time.Current[0], Patches.Time.Current[1])}",
                    night = getNight(),
                }
            ));

            SteamGameServer.SetGameDescription($"<color=#fb9d8f>| {playerCount} Hracov | {emsCount} EMS | {pdCount} PD |</color>");
        }

        private static void onShutDown()
        {
            // message
            Api.Send("/info/stats", JsonConvert.SerializeObject(
                new Stats()
                {
                    online = false,
                }
            ));

            // tab
            Api.Send("/info/tab", JsonConvert.SerializeObject(
                new Tab()
                {
                    players = 0,
                    time = "offline",
                }
            ));
        }

        private static bool getNight()
        {
            if (Patches.Time.Current[0] > 22)
                return true;
            if (Patches.Time.Current[0] < 5)
                return true;

            return false;
        }
    }
}
