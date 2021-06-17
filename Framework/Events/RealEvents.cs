using System;
using System.Collections.Generic;
using SDG.Unturned;
using RealLifeFramework.RealPlayers;
using RealLifeFramework.Skills;
using RealLifeFramework.UserInterface;
using RealLifeFramework.Patches;
using Rocket.Unturned.Events;
using Rocket.Unturned;
using Rocket.Unturned.Player;
using Steamworks;
using RealLifeFramework.Chatting;

namespace RealLifeFramework
{
    [EventHandler]
    public class RealEvents : IEventComponent
    {
        public void HookEvents()
        {

            VehicleManager.onDamageTireRequested += onDamageTireRequested;

           /* DamageTool.damagePlayerRequested += onPlayerDamageRequest;

            ItemManager.onTakeItemRequested += onTakeItemRequested;
            BarricadeManager.onOpenStorageRequested += onOpenStorageRequested;*/
        }
        private static void onDamageTireRequested(CSteamID instigatorSteamID, InteractableVehicle vehicle, int tireIndex, ref bool shouldAllow, EDamageOrigin damageOrigin)
        {
            // Player Prevention
            if (damageOrigin == EDamageOrigin.Bullet_Explosion || damageOrigin == EDamageOrigin.Punch || damageOrigin == EDamageOrigin.Useable_Gun || damageOrigin == EDamageOrigin.Useable_Melee && !RealPlayer.From(instigatorSteamID).IsAdmin)
                shouldAllow = false;
        }
    }
}
