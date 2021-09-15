using SDG.Unturned;
using System;

namespace RealLifeFramework
{
    public static class Logger
    {
        public static void Log(object message) => CommandWindow.Log($"[{DateTime.Now.ToString("HH:mm:ss")}] <DT> {message}");
        public static void LogError(object message) => CommandWindow.LogError($"[{DateTime.Now.ToString("HH:mm:ss")}] <DT> {message}");
        public static void LogWarn(object message) => CommandWindow.LogWarning($"[{DateTime.Now.ToString("HH:mm:ss")}] <DT> {message}");
    }
}
