using System;
using System.Net;
using System.IO;
using RealLifeFramework.RealPlayers;
using Rocket.Unturned.Player;
using Steamworks;
using SDG.Unturned;
using RealLifeFramework.SecondThread;

namespace RealLifeFramework
{
    public static class Discord
    {
        public static readonly string WebHookName = "DudeTurned";
        public static readonly string WebHookImage = "https://dennikpolitika.sk/wp-content/uploads/2015/12/cigan.png";
        public static string WebHookURL = "https://discord.com/api/webhooks/835275783986610196/vfqWbSAyxj0glwF51CCEuRhItuSkZELM6S8Q-Sz2ipMxk1GYY9wEcAxp38fvHKIjVvVS";

        public static void SendNewPlayer(RealPlayer player)
        {
            var untPlayer = UnturnedPlayer.FromCSteamID(player.CSteamID);

            string json = 
               ("{  " +
               $"'username': '{WebHookName}',  " +
               $"'avatar_url': '{WebHookImage}',  " +
                "'embeds': [ " +
                "   {    " +
                "       'color': 15258703,  " +
                "       'author': {" +
               $"           'name': '{untPlayer.SteamName}  ({untPlayer.CSteamID})'," +
               $"           'icon_url': '{untPlayer.SteamProfile.AvatarMedium}'  " +
                "        }," +
                "       'fields': [" +
                "           {   " +
                "               'name': '**Name**',  " +
               $"               'value': '{player.Name}'," +
                "               'inline': 'true'     " +
                "           },  " +
                "           {   " +
                "               'name': '**Age**', " +
               $"               'value': '{player.Age}'," +
                "               'inline': 'true'     " +
                "           },  " +
                "           {   " +
                "               'name': '**Gender**', " +
               $"               'value': '{player.Gender}'," +
                "               'inline': 'true'     " +
                "           }  " +
                "       ]," +
                "       'footer': { " +
               $"           'text': 'Dudeturned | {DateTime.Now.ToString("HH:mm:ss dd.MM.yyyy")}'   " +
                "        }    " +
                "   } " +
                " ]" +
                "}").Replace('\'', '"');

            sendToDiscord(json);
        }

        private static void sendToDiscord(string req)
        {
            SecondaryThread.Execute(() =>
            {
                try
                {
                    var webRequest = WebRequest.Create(WebHookURL);
                    webRequest.ContentType = "application/json";
                    webRequest.Method = "POST";

                    using (var sw = new StreamWriter(webRequest.GetRequestStream()))
                        sw.Write(req);

                    var x = webRequest.GetResponse();
                }
                catch (Exception ex)
                {
                    CommandWindow.Log($"Discord Error : {ex}");
                }
            });
        }

        private static void sendToAPI(string req)
        {
            SecondaryThread.Execute(() =>
            {
                try
                {
                    var webRequest = WebRequest.Create(WebHookURL);
                    webRequest.ContentType = "application/json";
                    webRequest.Method = "POST";

                    using (var sw = new StreamWriter(webRequest.GetRequestStream()))
                        sw.Write(req);

                    var x = webRequest.GetResponse();
                }
                catch (Exception ex)
                {
                    CommandWindow.Log($"Discord Error : {ex}");
                }
            });
        }
    }
}
