using SDG.Unturned;
using Rocket.API;
using System.Collections.Generic;
using RealLifeFramework.RealPlayers;
using Rocket.Unturned.Player;
using RealLifeFramework.UserInterface;
using RealLifeFramework.Ranks;

namespace RealLifeFramework.Commands
{
    public class CSeatBelt : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "seatbelt";

        public string Help => "seatbelt";

        public string Syntax => "/seatbelt";

        public List<string> Aliases => new List<string>() { "zapasat", "belt" };

        public List<string> Permissions => new List<string> { RankManager.PlayerPermission };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            var player = RealPlayer.From(((UnturnedPlayer)caller).CSteamID);

            if (player.Player.movement.getVehicle().asset.engine == EEngine.CAR)
            {
                if (player.HUD.HasSeatBelt)
                {
                    player.HUD.HasSeatBelt = false;
                    EffectManager.sendUIEffect(HUDComponent.RemoveBelt, 956, player.TransportConnection, false);
                    player.HUD.UpdateComponent(HUDComponent.Seatbelt[1], false);
                    player.HUD.UpdateComponent(HUDComponent.Seatbelt[0], true);
                }
                else
                {
                    player.HUD.HasSeatBelt = true;
                    EffectManager.sendUIEffect(HUDComponent.UseBelt, 956, player.TransportConnection, false);
                    player.HUD.UpdateComponent(HUDComponent.Seatbelt[0], false);
                    player.HUD.UpdateComponent(HUDComponent.Seatbelt[1], true);
                }
            }

        }
    }
}
