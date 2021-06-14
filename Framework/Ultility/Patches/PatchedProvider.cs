using System;
using HarmonyLib;
using SDG.Unturned;
using UnityEngine;
using Steamworks;

namespace RealLifeFramework.Patches
{
    [HarmonyPatch(typeof(Provider), "accept", new Type[] { typeof(SteamPlayerID), typeof(bool), typeof(bool), typeof(byte), typeof(byte), typeof(byte), typeof(Color), typeof(Color), typeof(Color), typeof(bool), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int[]), typeof(string[]), typeof(string[]), typeof(EPlayerSkillset), typeof(string), typeof(CSteamID) } )]
    internal class PatchedProvider
    {
        public static OnPreConnect onPlayerPreConnected;

        [HarmonyPrefix]
        private static void Accept(SteamPlayerID playerID, bool isPro, bool isAdmin, byte face, byte hair, byte beard, Color skin, Color color, Color markerColor, bool hand, int shirtItem, int pantsItem, int hatItem, int backpackItem, int vestItem, int maskItem, int glassesItem, int[] skinItems, string[] skinTags, string[] skinDynamicProps, EPlayerSkillset skillset, string language, CSteamID lobbyID)
        {
            onPlayerPreConnected.Invoke(playerID);
        }

        public delegate void OnPreConnect(SteamPlayerID playerID);
    }
}
