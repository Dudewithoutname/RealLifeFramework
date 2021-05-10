using System;
using HarmonyLib;
using SDG.Unturned;

namespace RealLifeFramework.Patches
{
    [HarmonyPatch(typeof(PlayerVoice), "handleRelayVoiceCulling_Proximity")]
    internal class VoiceRelay
    {
        public static handle onHandle;

        [HarmonyPrefix]
        private static void Handler(PlayerVoice speaker, PlayerVoice listener)
        {
            onHandle.Invoke(speaker, listener);
        }

        public delegate void handle(PlayerVoice speaker, PlayerVoice listener);
    }
}
