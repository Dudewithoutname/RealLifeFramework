using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealLifeFramework.Realism
{
    [EventHandler(nameof(VehicleCrash))]
    public class VehicleCrash : IEventComponent
    {
        public void HookEvents()
        {
            VehicleManager.onDamageVehicleRequested += onDamageVehicle;
        }

        // original code by TH3AL3X
        private void onDamageVehicle(CSteamID instigatorSteamID, InteractableVehicle vehicle, ref ushort pendingTotalDamage, ref bool canRepair, ref bool shouldAllow, EDamageOrigin damageOrigin)
        {
            UnturnedPlayer player = UnturnedPlayer.FromCSteamID(instigatorSteamID);

            if (pendingTotalDamage <= 2)
                return;

            if (player == null)
                return;

            if (damageOrigin == EDamageOrigin.Vehicle_Collision_Self_Damage)
            {
                if (player.CurrentVehicle.asset.engine == EEngine.CAR)
                {
                    foreach (var passenger in vehicle.passengers)
                    {

                        if (jugador != null && !jugador.GetComponent<PlayerComponent>().niggagetwork)
                            StartCoroutine(crash(jugador));
                        // break; my cucumber idk, for prevent bugs
                        break;
                    }
                }
            }
        }
    }
}
