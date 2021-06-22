using System;
using HarmonyLib;
using SDG.Unturned;

namespace RealLifeFramework.Patches
{
    [HarmonyPatch(typeof(PlayerVoice), "handleRelayVoiceCulling_Proximity")]
    internal class VoiceRelay
    {
        public static Handle onHandle;

        [HarmonyPrefix]
        private static bool Handler(PlayerVoice speaker, PlayerVoice listener)
        {
            return onHandle.Invoke(speaker, listener);
        }

        public delegate bool Handle(PlayerVoice speaker, PlayerVoice listener);
    }
}
