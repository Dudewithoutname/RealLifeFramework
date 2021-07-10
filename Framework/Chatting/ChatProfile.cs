using RealLifeFramework.RealPlayers;
using RealLifeFramework.UserInterface;
using Rocket.Unturned.Player;

namespace RealLifeFramework.Chatting
{
    public class ChatProfile
    {
        public RealPlayer RealPlayer { get; set; }
        public string NameColor { get; set; }
        public string Avatar { get; set; }
        public EPlayerVoiceMode VoiceMode { get; set; } = EPlayerVoiceMode.Normal;

        public ChatProfile(RealPlayer realplayer)
        {
            NameColor = "#ffffff";
            Avatar = (realplayer.RankUser.DisplayIcon == "player")? UnturnedPlayer.FromCSteamID(realplayer.CSteamID).SteamProfile.AvatarFull.ToString() : realplayer.RankUser.DisplayIcon;
            RealPlayer = realplayer;

            RealPlayer.HUD.UpdateComponent(HUDComponent.Voice, VoiceChat.Icons[(int)EPlayerVoiceMode.Normal]);
        }

        public ChatProfile(RealPlayer realplayer, ProfileData data)
        {
            NameColor = data.NameColor;
            Avatar = (realplayer.RankUser.DisplayIcon == "player")? UnturnedPlayer.FromCSteamID(realplayer.CSteamID).SteamProfile.AvatarFull.ToString() : realplayer.RankUser.DisplayIcon;
            RealPlayer = realplayer;

            RealPlayer.HUD.UpdateComponent(HUDComponent.Voice, VoiceChat.Icons[(int)EPlayerVoiceMode.Normal]);
        }

        public void ChangeVoicemode(EPlayerVoiceMode voicemode, string icon)
        {
            VoiceMode = voicemode;
            RealPlayer.HUD.UpdateComponent(HUDComponent.Voice, icon);
        }
    }
}
