using System;
using System.Collections.Generic;
using SDG.Unturned;
using RealLifeFramework.Patches;
using RealLifeFramework.Players;
using Steamworks;
using UnityEngine;

namespace RealLifeFramework.Chatting
{
    [EventHandler("VoiceChat")]
    public class VoiceChat : IEventComponent
    {
        private static short[] Distance = { 
            RealLife.Instance.Configuration.Instance.Whisper,
            RealLife.Instance.Configuration.Instance.Normal,
            RealLife.Instance.Configuration.Instance.Shout,
        };

        public void HookEvents() => VoiceRelay.onHandle += HandleVoiceRelay;

        public static void Subscribe(RealPlayer player) => player.Keyboard.KeyDown += ChangeVoiceViaButton;

        public static void UnSubscribe(RealPlayer player) => player.Keyboard.KeyDown -= ChangeVoiceViaButton;

        private static bool HandleVoiceRelay(PlayerVoice speaker, PlayerVoice listener)
        {
            RealPlayer rplayer = RealPlayerManager.GetRealPlayer(speaker.player);
            
            switch (rplayer.ChatProfile.VoiceMode)
            {
                case EPlayerVoiceMode.Whisper:
                    return VoiceDistanceHandler(rplayer.ChatProfile.VoiceMode, speaker, listener);
                case EPlayerVoiceMode.Normal:
                    return VoiceDistanceHandler(rplayer.ChatProfile.VoiceMode, speaker, listener);
                case EPlayerVoiceMode.Shout:
                    return VoiceDistanceHandler(rplayer.ChatProfile.VoiceMode, speaker, listener);
                default:
                    return true;
            }

        }

        private static bool VoiceDistanceHandler(EPlayerVoiceMode mode, PlayerVoice speaker, PlayerVoice listener)
        {
            if ((byte)mode >= 2) return false;

            if(RealLife.Debugging) // remove this later
                Logger.Log(Vector3.Distance(speaker.player.gameObject.transform.position, listener.player.gameObject.transform.position).ToString());

            if (Vector3.Distance(speaker.player.gameObject.transform.position, listener.player.gameObject.transform.position) <= Distance[(int)mode])
                return true;
            else
                return false;
        }

        private static void ChangeVoiceViaButton(Player player, UnturnedKey key)
        {
            Logger.Log(key.ToString());
            if (key == UnturnedKey.CodeHotkey1)
                GetNextVoiceMode(RealPlayerManager.GetRealPlayer(player));
        }

        public static void GetNextVoiceMode(RealPlayer player)
        {
            // TODO : ADD IT FOR JOBS EMS / PD
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
