using System;
using SDG.Unturned;
using Rocket.API;
using System.Collections.Generic;
using RealLifeFramework.Players;
using RealLifeFramework.Skills;

namespace RealLifeFramework.Commands
{
    public class test2 : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "test2";

        public string Help => "change voice mode";

        public string Syntax => "/test2 (name)";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>() { "dude.test" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            RealPlayer player = RealPlayerManager.GetRealPlayer(caller);
        }
    }
}
