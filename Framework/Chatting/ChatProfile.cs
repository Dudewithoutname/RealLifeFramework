using RealLifeFramework.RealPlayers;
using RealLifeFramework.UserInterface;

namespace RealLifeFramework.Chatting
{
    public class ChatProfile
    {
        public RealPlayer RealPlayer { get; set; }
        public string NameColor { get; set; }
        public string Avatar { get; set; }
        public EPlayerVoiceMode VoiceMode { get; set; } = EPlayerVoiceMode.Normal;

        public ChatProfile(string ncolor, string avatar, EPlayerVoiceMode voicemode, RealPlayer realplayer)
        {
            NameColor = ncolor;
            Avatar = avatar;
            RealPlayer = realplayer;
            RealPlayer.HUD.UpdateComponent(HUDComponent.Voice, VoiceChat.Icons[(int)voicemode]);
        }

        public ChatProfile(RealPlayer realplayer, ProfileData data)
        {
            NameColor = data.NameColor;
            Avatar = data.Avatar;
            RealPlayer = realplayer;
            RealPlayer.HUD.UpdateComponent(HUDComponent.Voice, VoiceChat.Icons[(int)VoiceMode]);
        }

        public void ChangeVoicemode(EPlayerVoiceMode voicemode, string icon)
        {
            VoiceMode = voicemode;
            RealPlayer.HUD.UpdateComponent(HUDComponent.Voice, icon);
        }
    }
}
