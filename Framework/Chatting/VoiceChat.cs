using System;
using System.Collections.Generic;
using SDG.Unturned;
using RealLifeFramework.Patches;
using RealLifeFramework.RealPlayers;
using Steamworks;
using UnityEngine;
using System.Runtime.InteropServices.WindowsRuntime;

namespace RealLifeFramework.Chatting
{
    [EventHandler]
    public class VoiceChat : IEventComponent
    {
        public static readonly short[] Distance = { 
            RealLife.Instance.Configuration.Instance.Whisper,
            RealLife.Instance.Configuration.Instance.Normal,
            RealLife.Instance.Configuration.Instance.Shout,
        };

        public static readonly string[] Icons = {
            "https://i.ibb.co/fdZt7dG/silent.png",
            "https://i.ibb.co/HXncLTs/normal.png",
            "https://i.ibb.co/g3dbHP7/shout.png",
        };

        public void HookEvents() => VoiceRelay.onHandle += HandleVoiceRelay;

        public static void Subscribe(RealPlayer player) => player.Keyboard.KeyDown += ChangeVoiceViaButton;

        public static void UnSubscribe(RealPlayer player) => player.Keyboard.KeyDown -= ChangeVoiceViaButton;

        private static bool HandleVoiceRelay(PlayerVoice speaker, PlayerVoice listener)
        {
            var rplayer = RealPlayer.From(speaker.player);

            if (rplayer == null) return false;

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
            switch (mode)
            {
                // thx to uncle Mike <3 
                // for the when switch operato
                case EPlayerVoiceMode.Whisper when Vector3.Distance(speaker.player.gameObject.transform.position, listener.player.gameObject.transform.position) <= Distance[(int)EPlayerVoiceMode.Whisper]:
                    return true;
                case EPlayerVoiceMode.Normal when Vector3.Distance(speaker.player.gameObject.transform.position, listener.player.gameObject.transform.position) <= Distance[(int)EPlayerVoiceMode.Normal]:
                    return true;
                case EPlayerVoiceMode.Shout when Vector3.Distance(speaker.player.gameObject.transform.position, listener.player.gameObject.transform.position) <= Distance[(int)EPlayerVoiceMode.Shout]:
                    return true;
                default:
                    return false;
            }
        }

        private static void ChangeVoiceViaButton(Player player, UnturnedKey key)
        {
            if (key == UnturnedKey.CodeHotkey1 && ((object)player.movement.getVehicle()) == null)
            {
                GetNextVoiceMode(RealPlayer.From(player));
            }
        }

        public static void GetNextVoiceMode(RealPlayer player)
        {
            // TODO : ADD IT FOR JOBS EMS / PD
            switch (player.ChatProfile.VoiceMode)
            {
                case EPlayerVoiceMode.Whisper:
                    player.ChatProfile.ChangeVoicemode(EPlayerVoiceMode.Normal, Icons[(int)EPlayerVoiceMode.Normal]);
                    break;

                case EPlayerVoiceMode.Normal:
                    player.ChatProfile.ChangeVoicemode(EPlayerVoiceMode.Shout, Icons[(int)EPlayerVoiceMode.Shout]);
                    break;

                case EPlayerVoiceMode.Shout:
                    player.ChatProfile.ChangeVoicemode(EPlayerVoiceMode.Whisper, Icons[(int)EPlayerVoiceMode.Whisper]);
                    break;
            }
        }

        public static void SetPlayerVoiceMode(RealPlayer player, EPlayerVoiceMode voice)
        {
            // TODO : ADD IT FOR JOBS EMS / PD
            switch (voice)
            {
                case EPlayerVoiceMode.Whisper:
                    player.ChatProfile.ChangeVoicemode(EPlayerVoiceMode.Whisper, Icons[(int)EPlayerVoiceMode.Whisper]);
                    break;

                case EPlayerVoiceMode.Normal:
                    player.ChatProfile.ChangeVoicemode(EPlayerVoiceMode.Normal, Icons[(int)EPlayerVoiceMode.Normal]);
                    break;

                case EPlayerVoiceMode.Shout:
                    player.ChatProfile.ChangeVoicemode(EPlayerVoiceMode.Shout, Icons[(int)EPlayerVoiceMode.Shout]);
                    break;
            }
        }

    }

    public enum EPlayerVoiceMode : int // actually i don't know what byte inheritance does with it but whatever it looks cool XD
    {
        Whisper = 0,
        Normal = 1,
        Shout = 2,
        PoliceChannel = 3,
        EMSChannel = 4
    }
}
