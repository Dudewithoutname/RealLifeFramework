using System;
using SDG.Unturned;
using Rocket.API;
using System.Collections.Generic;
using RealLifeFramework.RealPlayers;
using RealLifeFramework.Skills;
using Rocket.Unturned.Player;
using RealLifeFramework.UserInterface;
using RealLifeFramework.Chatting;
using RealLifeFramework.Ranks;

namespace RealLifeFramework.Commands
{
    public class CVoice : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "voice";

        public string Help => "voice";

        public string Syntax => "/voice";

        public List<string> Aliases => new List<string>() { "vc", "changevoice" };

        public List<string> Permissions => new List<string> { RankManager.PlayerPermission };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            var player = RealPlayer.From(((UnturnedPlayer)caller).CSteamID);

            if(command.Length < 1)
            {
                VoiceChat.GetNextVoiceMode(player);
                return;
            }

            if (Enum.TryParse(command[0], out EPlayerVoiceMode result))
            {
                player.ChatProfile.ChangeVoicemode(result, VoiceChat.Icons[(int)result]);
            }
            else
            {
                ChatManager.say(player.CSteamID, "Zly nazov voice modu", Palette.COLOR_R, EChatMode.SAY, false);
            }
        }
    }
}
