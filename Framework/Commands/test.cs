﻿using System;
using SDG.Unturned;
using Rocket.API;
using System.Collections.Generic;
using RealLifeFramework.RealPlayers;
using RealLifeFramework.Skills;

namespace RealLifeFramework.Commands
{
    public class test : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "test";

        public string Help => "test";

        public string Syntax => "/test";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string> { EPrivilege.OWNER.ToPermission() };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            RealPlayer player = RealPlayer.From(caller);
            player.AddExp(30);
        }
    }
}