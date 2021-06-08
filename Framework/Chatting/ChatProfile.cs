using RealLifeFramework.Players;
using RealLifeFramework.UserInterface;

namespace RealLifeFramework.Chatting
{
    public class ChatProfile
    {
        public RealPlayer RPlayer { get; set; }
        public string NameColor { get; set; }
        public string Avatar { get; set; }
        public EPlayerVoiceMode VoiceMode { get; set; }

        public ChatProfile(string ncolor, string avatar, EPlayerVoiceMode voicemode, RealPlayer realplayer)
        {
            NameColor = ncolor;
            Avatar = avatar;
            RPlayer = realplayer;
            VoiceChat.SetPlayerVoiceMode(RPlayer, voicemode);
        }

        public void ChangeVoicemode(EPlayerVoiceMode voicemode, string icon)
        {
            VoiceMode = voicemode;
            RPlayer.HUD.UpdateComponent(HUDComponent.Voice, icon);
        }
    }
}
