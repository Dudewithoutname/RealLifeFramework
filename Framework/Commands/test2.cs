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

        public List<string> Permissions => new List<string>();

        public void Execute(IRocketPlayer caller, string[] command)
        {
            RealPlayer player = RealPlayerManager.GetRealPlayer(caller);
            player.HUD.SendWidget("https://upload.wikimedia.org/wikipedia/commons/thumb/6/65/Circle-icons-car.svg/512px-Circle-icons-car.svg.png", UserInterface.EWidgetType.Blood);
        }
    }

    public class test3 : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "test3";

        public string Help => "change voice mode";

        public string Syntax => "/test3 (name)";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string>();

        public void Execute(IRocketPlayer caller, string[] command)
        {
            RealPlayer player = RealPlayerManager.GetRealPlayer(caller);
            player.HUD.RemoveWidget(Convert.ToInt32(command[0]));
        }
    }
}
