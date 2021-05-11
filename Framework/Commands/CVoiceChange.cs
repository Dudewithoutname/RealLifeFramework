using System;
using SDG.Unturned;
using Rocket.API;
using System.Collections.Generic;
using RealLifeFramework.Players;
using RealLifeFramework.Skills;

namespace RealLifeFramework.Commands
{

    public class CVoiceChange : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "voicechange";

        public string Help => "change voice mode";

        public string Syntax => "/voicechange (name)";

        public List<string> Aliases => new List<string>() { "vc", "vcm", "changevoicemode"};

        public List<string> Permissions => new List<string> { "dude.voice" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            RealPlayer player = RealPlayerManager.GetRealPlayer(caller);
            player.AddExp(30);
        }
    }
}
