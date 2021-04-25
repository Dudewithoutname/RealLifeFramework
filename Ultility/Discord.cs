using System;
using System.Net;
using System.IO;
using Newtonsoft;

namespace RealLifeFramework
{
    public static class Discord
    {
        public static string WebHookURL = "https://discord.com/api/webhooks/835275783986610196/vfqWbSAyxj0glwF51CCEuRhItuSkZELM6S8Q-Sz2ipMxk1GYY9wEcAxp38fvHKIjVvVS";

        // TODO: better send message
        public static void SendDiscord(string message)
        {
            var json =
                ("{  " +
                "'username': 'TvojTatko',  " +
                "'avatar_url': 'https://dennikpolitika.sk/wp-content/uploads/2015/12/cigan.png',  " +
                "'embeds': [ {    " +
               $"       'title': '{message}',    " +
                "       'color': 15258703,  " +
                "       'footer': { " +
               $"           'text': 'Dudeturned | {DateTime.Now.ToString("hh:mm:ss dd.MM.yyyy")}'   " +
                "        }    " +
                "} ]" +
                "}").Replace('\'','"');

            var webRequest = WebRequest.Create(WebHookURL);
            webRequest.ContentType = "application/json";
            webRequest.Method = "POST";

            using (var sw = new StreamWriter(webRequest.GetRequestStream()))
                sw.Write(json);

            var x = webRequest.GetResponse();
            Logger.Log(x.ToString());
        }
    }
}
