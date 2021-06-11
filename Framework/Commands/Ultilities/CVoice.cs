using System;
using SDG.Unturned;
using Rocket.API;
using System.Collections.Generic;
using RealLifeFramework.Players;
using RealLifeFramework.Skills;
using Rocket.Unturned.Player;
using RealLifeFramework.UserInterface;
using RealLifeFramework.Chatting;

namespace RealLifeFramework.Commands
{
    public class CVoice : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "voice";

        public string Help => "voice";

        public string Syntax => "/voice";

        public List<string> Aliases => new List<string>() { "vc", "changevoice" };

        public List<string> Permissions => new List<string> { "dude.basic" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            var player = RealPlayerManager.GetRealPlayer(((UnturnedPlayer)caller).CSteamID);

            VoiceChat.GetNextVoiceMode(player);
        }
    }
}
