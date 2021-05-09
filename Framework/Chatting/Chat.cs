﻿using SDG.Unturned;
using UnityEngine;
using RealLifeFramework.Players;

namespace RealLifeFramework.Chatting
{

    [EventHandler("Chat")]
    public class Chat : IEventComponent
    {
        public void HookEvents()
        {
            ChatManager.onChatted += onPlayerChatted;
        }

        private static void onPlayerChatted(SteamPlayer player, EChatMode mode, ref Color chatted, ref bool isRich, string text, ref bool isVisible)
        {
            RealPlayer rplayer = RealPlayerManager.GetRealPlayer(player.player);
            // TODO : Mute System , Administration system and this obviously
            if (text.StartsWith("/")) 
            {
                isVisible = false;
                return;
            }

            switch (mode)
            {
                case EChatMode.GLOBAL:
                    isVisible = SendGlobalMessage(rplayer, mode, ref chatted, ref isRich, text, ref isVisible);
                    break;
                case EChatMode.LOCAL:
                    isVisible = SendLocalMessage(rplayer, mode, ref chatted, ref isRich, text, ref isVisible);
                    break;
                case EChatMode.GROUP:
                    isVisible = SendGroupMessage(rplayer, mode, ref chatted, ref isRich, text, ref isVisible);
                    break;
            }
        }

        private static bool SendGlobalMessage(RealPlayer player, EChatMode mode, ref Color chatted, ref bool isRich, string text, ref bool isVisible)
        {
            string message = refactorMessage(text, player.IsAdmin);

            if (isVisible)
            {
                ChatManager.serverSendMessage(
                    $"<size=11><color=#de4dff>[{player.Level}]</color></size> <color={player.ChatProfile.NameColor}>{player.Name}</color> <color=#ffffff>: {message}</color>",
                    Color.white, player.Player.channel.owner, null, EChatMode.GLOBAL, player.ChatProfile.Avatar, true);
            }

            return false;
        }

        private static bool SendLocalMessage(RealPlayer player, EChatMode mode, ref Color chatted, ref bool isRich, string text, ref bool isVisible)
        {
            string message = refactorMessage(text, player.IsAdmin);

            if(isVisible)
            {
                ChatManager.serverSendMessage(
                    $"<size=11><color=#b3babd>(local)</color> <color=#de4dff>[{player.Level}]</color></size> <color={player.ChatProfile.NameColor}>{player.Name}</color> <color=#ffffff>: {message}</color>",
                    Color.white, player.Player.channel.owner, null, EChatMode.LOCAL, player.ChatProfile.Avatar, true);
            }

            return false;
        }

        private static bool SendGroupMessage(RealPlayer player, EChatMode mode, ref Color chatted, ref bool isRich, string text, ref bool isVisible)
        {
            string message = refactorMessage(text, player.IsAdmin);

            if(isVisible)
            {
                ChatManager.serverSendMessage(
                    $"<size=11><color=#b3babd>(group)</color> <color=#de4dff>[{player.Level}]</color></size> <color={player.ChatProfile.NameColor}>{player.Name}</color> <color=#ffffff>: {message}</color>",
                    Color.white, player.Player.channel.owner, null, EChatMode.GROUP, player.ChatProfile.Avatar, true);
            }

            return false;
        }

        private static string refactorMessage(string message, bool isAdmin)
        {
            string output = message;

            if ((message.Contains("<") || message.Contains(">")) && !isAdmin)
            {
                output.Replace("<", "(");
                output.Replace(">", ")");
            }

            return output;
        }
    }
}