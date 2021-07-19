using System;
using System.Net;
using System.IO;
using RealLifeFramework.Threadding;

namespace RealLifeFramework
{
    public class Api
    {
        private static string domain => "http://157.90.138.191:3003";
        private static string token => "2a8di9023!i0ou.r897%u9wqf";

        public static void Send(string route, string rawJson)
        {
            // async
            Helper.Execute(() =>
            {
                string json = $"{{ \"token\" : \"{token}\", {rawJson.Remove(0, 1)}"; // WOW this is called pro programming :DDDDDDDDDDDD ano som moc jebly ze ?
                try
                {
                    var webRequest = WebRequest.Create($"{domain}{route}");
                    webRequest.ContentType = "application/json";
                    webRequest.Method = "POST";

                    using (var sw = new StreamWriter(webRequest.GetRequestStream()))
                    {
                        sw.Write(json);
                    }

                    var response = webRequest.GetResponse();
                }
                catch 
                {
                    //Logger.Log($"[API Error] : {ex}");
                }
            });
        }
    }
}
