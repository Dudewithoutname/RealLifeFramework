using System;
using System.Collections.Generic;
using SDG.Unturned;
using UnityEngine;
using RealLifeFramework.Players;
using Rocket.Unturned.Events;
using Rocket.Unturned;
using Rocket.Unturned.Player;

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
            switch (mode)
            {
                case EChatMode.GLOBAL:
                    isVisible = SendGlobalMessage(rplayer, mode, ref chatted, ref isRich, text, ref isVisible);
                    break;
                case EChatMode.LOCAL:
                    break;
                case EChatMode.GROUP:
                    break;
            }
        }

        private static bool SendGlobalMessage(RealPlayer player, EChatMode mode, ref Color chatted, ref bool isRich, string text, ref bool isVisible)
        {
            string message = refactorMessage(text, player.IsAdmin);

            if (isVisible)
            {
                if (message.StartsWith("/")) return false;

                ChatManager.serverSendMessage($"<size=11><color=#de4dff>[5]</color></size> <color=#ffffff>{player.Name} : {message}</color>", Color.white, null, null,
                     EChatMode.GLOBAL, player.Avatar, true);
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
