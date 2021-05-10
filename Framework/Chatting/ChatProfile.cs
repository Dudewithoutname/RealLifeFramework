using RealLifeFramework.Players;

namespace RealLifeFramework.Chatting
{
    public class ChatProfile
    {
        public RealPlayer RPlayer { get; set; }
        public string NameColor { get; set; }
        public string Avatar { get; set; }
        public EPlayerVoiceMode VoiceMode { get; set; }
        
        public bool keyState { get; set; }

        public ChatProfile(string ncolor, string avatar, EPlayerVoiceMode voicemode, RealPlayer realplayer)
        {
            NameColor = ncolor;
            Avatar = avatar;
            VoiceMode = voicemode;
            RPlayer = realplayer;
            keyState = false;
        }

        public void ChangeVoicemode(EPlayerVoiceMode voicemode)
        {
            VoiceMode = voicemode;
            RPlayer.HUD.UpdateVoice(voicemode);
        }
    }
}
