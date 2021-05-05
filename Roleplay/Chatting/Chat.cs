using System;
using System.Collections.Generic;
using SDG.Unturned;
using UnityEngine;

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
            Logger.Log("test");
            /*// TODO : Mute System , Administration system and this obviously
            switch (mode)
            {
                case EChatMode.GLOBAL:

                    break;
                case EChatMode.LOCAL:
                    break;
                case EChatMode.GROUP:
                    break;
                case EChatMode.SAY:
                    break;
            }*/
        }


    }
}
