using SDG.Unturned;
using System;

namespace RealLifeFramework
{
    public static class Logger
    {
        public static void Log(string message) => CommandWindow.Log($"[{DateTime.Now.ToString("HH:mm:ss")}] <DT> {message}");
    }
}
