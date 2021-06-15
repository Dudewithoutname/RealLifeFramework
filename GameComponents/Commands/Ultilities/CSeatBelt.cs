using System;
using SDG.Unturned;
using Rocket.API;
using System.Collections.Generic;
using RealLifeFramework.RealPlayers;
using RealLifeFramework.Skills;
using Rocket.Unturned.Player;
using RealLifeFramework.UserInterface;

namespace RealLifeFramework.Commands
{
    public class CSeatBelt : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "seatbelt";

        public string Help => "seatbelt";

        public string Syntax => "/seatbelt";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string> { "dude.basic" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            var player = RealPlayer.From(((UnturnedPlayer)caller).CSteamID);

            if (player.Player.movement.getVehicle().asset.engine == EEngine.CAR)
            {
                if (player.HUD.HasSeatBelt)
                {
                    EffectManager.sendUIEffect(HUDComponent.RemoveBelt, 956, player.TransportConnection, false);
                    player.HUD.UpdateComponent(HUDComponent.Seatbelt[1], false);
                    player.HUD.UpdateComponent(HUDComponent.Seatbelt[0], true);
                }
                else
                {
                    EffectManager.sendUIEffect(HUDComponent.UseBelt, 956, player.TransportConnection, false);
                    player.HUD.UpdateComponent(HUDComponent.Seatbelt[0], false);
                    player.HUD.UpdateComponent(HUDComponent.Seatbelt[1], true);
                }
            }
        }
    }
}
