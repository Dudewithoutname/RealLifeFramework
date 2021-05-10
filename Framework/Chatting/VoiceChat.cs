using System;
using System.Collections.Generic;
using SDG.Unturned;
using RealLifeFramework.Patches;
using RealLifeFramework.Players;
using Steamworks;

namespace RealLifeFramework.Chatting
{
    [EventHandler("VoiceChat")]
    public class VoiceChat : IEventComponent
    {
        public void HookEvents()
        {
            VoiceRelay.onHandle += HandleVoiceRelay;
            PlayerInput.onPluginKeyTick = ChangeVoiceViaButton;
        }

        private static void HandleVoiceRelay(PlayerVoice speaker, PlayerVoice listener)
        {
            
        }

        private static void ChangeVoiceViaButton(Player player, uint simulation, byte key, bool state)
        {
            RealPlayer rplayer = RealPlayerManager.GetRealPlayer(player);

            if(state != rplayer.ChatProfile.keyState && rplayer.ChatProfile.keyState != false)
            {
                GetNextVoiceMode(rplayer);
            }

            rplayer.ChatProfile.keyState = state;
        }

        public static void GetNextVoiceMode(RealPlayer player)
        {
            // TODO : ADD IT FOR JOBS
            if ((byte)player.ChatProfile.VoiceMode >= 2)
                player.ChatProfile.ChangeVoicemode(EPlayerVoiceMode.Whisper);
            else
                player.ChatProfile.ChangeVoicemode(player.ChatProfile.VoiceMode++);
            
        }

        public static string GetVoiceModeName(EPlayerVoiceMode voicemode)
        {
            switch (voicemode)
            {
                case EPlayerVoiceMode.Whisper:
                    return "Whisper";
                case EPlayerVoiceMode.Normal:
                    return "Normal";
                case EPlayerVoiceMode.Shout:
                    return "Shout";
                case EPlayerVoiceMode.PoliceChannel:
                    return "Police";
                case EPlayerVoiceMode.EMSChannel:
                    return "EMS";
            }

            return "";
        }
    }

    public enum EPlayerVoiceMode
    {
        Whisper,
        Normal,
        Shout,
        PoliceChannel,
        EMSChannel
    }
}
