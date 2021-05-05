using System;
using System.Collections.Generic;
using SDG.Unturned;
using RealLifeFramework.Players;
using RealLifeFramework.Skills;
using RealLifeFramework.UserInterface;
using RealLifeFramework.Chatting;
using Rocket.Unturned.Events;
using Rocket.Unturned;
using Rocket.Unturned.Player;
using Steamworks;

namespace RealLifeFramework
{
    [EventHandler("MainEvents")]
    public class RealEvents : IEventComponent
    {
        public void HookEvents()
        {

            U.Events.OnPlayerConnected += onPlayerConnected;
            U.Events.OnPlayerDisconnected += onPlayerDisconnected;

            VehicleManager.onDamageTireRequested += onDamageTireRequested;

            DamageTool.damagePlayerRequested += onPlayerDamageRequest;

            ItemManager.onTakeItemRequested = onTakeItemRequested;
            BarricadeManager.onOpenStorageRequested = onOpenStorageRequested;

            EffectManager.onEffectButtonClicked = onEffectButtonClicked;
            EffectManager.onEffectTextCommitted = onEffectTextCommited;

        }

        private static void onPlayerConnected(UnturnedPlayer player)
        {
            Logger.Log($"[Info] |+| Player Connected : {player.SteamName} ({player.CSteamID}) ({player.Player.channel.GetOwnerTransportConnection().GetAddress()})");

            RealPlayerManager.InitializePlayer(player);

            player.Player.setPluginWidgetFlag(EPluginWidgetFlags.ShowInteractWithEnemy, false);
            player.Player.setPluginWidgetFlag(EPluginWidgetFlags.ShowHealth, false);
            player.Player.setPluginWidgetFlag(EPluginWidgetFlags.ShowFood, false);
            player.Player.setPluginWidgetFlag(EPluginWidgetFlags.ShowWater, false);
            player.Player.setPluginWidgetFlag(EPluginWidgetFlags.ShowStamina, false);
            player.Player.setPluginWidgetFlag(EPluginWidgetFlags.ShowOxygen, false);
            player.Player.setPluginWidgetFlag(EPluginWidgetFlags.ShowVirus, false);
            player.Player.setPluginWidgetFlag(EPluginWidgetFlags.ShowStatusIcons, false);


            //player.Player.inventory.onInventoryAdded = OnInventoryItemAdded;
        }

        private static void onPlayerDisconnected(UnturnedPlayer player)
        {
            Logger.Log($"[Info] |-| Player Disconnected : {player.SteamName} ({player.CSteamID}) ");

            RealPlayerManager.HandleDisconnect(player);
        }

        private static void onDamageTireRequested(CSteamID instigatorSteamID, InteractableVehicle vehicle, int tireIndex, ref bool shouldAllow, EDamageOrigin damageOrigin)
        {
            if (damageOrigin == EDamageOrigin.Bullet_Explosion || damageOrigin == EDamageOrigin.Punch || damageOrigin == EDamageOrigin.Useable_Gun || damageOrigin == EDamageOrigin.Useable_Melee)
                shouldAllow = false;
        }

        // patched by Time 
        public static void onTimeUpdated(ushort hours, ushort minutes)
        {
            HUDManager.UpdateTime(hours, minutes);
        }

        private static void OnInventoryItemAdded(byte page, byte index, ItemJar jar)
        {

        }

        private static void onTakeItemRequested(Player player, byte x, byte y, uint instanceID, byte to_x, byte to_y, byte to_rot, byte to_page, ItemData itemData, ref bool shouldAllow)
        {
            RealPlayerManager.GetRealPlayer(player);
            shouldAllow = false;
        }

        private static void onOpenStorageRequested(CSteamID instigator, InteractableStorage storage, ref bool shouldAllow)
        {

        }

        private static void onPlayerDamageRequest(ref DamagePlayerParameters parameters, ref bool shouldAllow)
        {
        }

        private static void onEffectButtonClicked(Player player, string buttonName)
        {
            if (RealPlayerCreation.PrePlayers.ContainsKey(player.channel.owner.playerID.steamID))
            {
                switch (buttonName)
                {
                    case "g_male":
                        RealPlayerCreation.SetGender(player.channel.owner.playerID.steamID, 0);
                        break;
                    case "g_female":
                        RealPlayerCreation.SetGender(player.channel.owner.playerID.steamID, 1);
                        break;
                    case "createCharacterbtn":
                        RealPlayerCreation.CreateCharacter(player.channel.owner.playerID.steamID);
                        break;
                }
            }
            else
            {
                switch (buttonName)
                {
                    case "joindiscord_btn":
                        player.sendBrowserRequest("Discord Invite", RealLife.Instance.Configuration.Instance.DiscordInvite);
                        break;
                    case "joinsteam_btn":
                        player.sendBrowserRequest("Steam Group", RealLife.Instance.Configuration.Instance.SteamGroupInvite);
                        break;
                    case "continue_btn":
                        RealPlayerCreation.OpenCreation(player);
                        break;
                }
            }
        }

        private static void onEffectTextCommited(Player player, string inputName, string text)
        {
            if (!RealPlayerCreation.PrePlayers.ContainsKey(player.channel.owner.playerID.steamID))
                return;

            switch (inputName)
            {
                case "input_first":
                    RealPlayerCreation.PrePlayers[player.channel.owner.playerID.steamID].FirstName = text;
                    break;
                case "input_last":
                    RealPlayerCreation.PrePlayers[player.channel.owner.playerID.steamID].LastName = text;
                    break;
                case "input_age":
                    byte? age = Byte.TryParse(text, out byte result) ? (byte?)result : null;
                    RealPlayerCreation.PrePlayers[player.channel.owner.playerID.steamID].Age = age;
                    break;

            }
        }

    }
}
