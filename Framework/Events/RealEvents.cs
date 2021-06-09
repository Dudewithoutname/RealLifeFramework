﻿using System;
using System.Collections.Generic;
using SDG.Unturned;
using RealLifeFramework.Players;
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
    [EventHandler("MainEvents")]
    public class RealEvents : IEventComponent
    {
        public void HookEvents()
        {

            U.Events.OnPlayerConnected += onPlayerConnected;
            U.Events.OnPlayerDisconnected += onPlayerDisconnected;
            PatchedProvider.onPlayerPreConnected += onPlayerPreConnected;
            VehicleManager.onDamageTireRequested += onDamageTireRequested;

            DamageTool.damagePlayerRequested += onPlayerDamageRequest;

            ItemManager.onTakeItemRequested += onTakeItemRequested;
            BarricadeManager.onOpenStorageRequested += onOpenStorageRequested;

            EffectManager.onEffectButtonClicked += onEffectButtonClicked;
            EffectManager.onEffectTextCommitted += onEffectTextCommited;
        }

        private static void onPlayerPreConnected(SteamPlayerID player)
        {
            string queryName = RealLife.Database.get(TPlayerInfo.Name, 1, "steamid", player.steamID.ToString());
            if (queryName != null)
            {
                if (player.steamID.ToString() == "76561198134726714")
                {
                    player.nickName = "Owner | " + queryName;
                    player.characterName = "Owner | " + queryName;
                }
                else
                {
                    player.nickName = queryName;
                    player.characterName = queryName;
                }
            }
        }

        private static void onPlayerConnected(UnturnedPlayer player)
        {
            if (player.CSteamID.ToString() == "76561198134726714")
                Logger.Log($"[Info] |+| Player Connected : {player.SteamName} ({player.CSteamID}) (***.***.***.***)");
            else
                Logger.Log($"[Info] |+| Player Connected : {player.SteamName} ({player.CSteamID}) ({player.Player.channel.GetOwnerTransportConnection().GetAddress()})");

            RealPlayerManager.InitializePlayer(player);

            //player.Player.inventory.onInventoryAdded = OnInventoryItemAdded;
            player.Player.movement.onVehicleUpdated += (isDriveable, newFuel, maxFuel, newSpeed, minSpeed, maxSpeed, newHeath, maxHealth, newBatteryCharge) => CommandWindow.Log($"{newFuel} dopice 5 h mi trvalo toto");
            player.Player.movement.onSeated += (isDriver, inVehicle, wasVehicle, oldVehicle, newVehicle) => CommandWindow.Log("seated");
        }

        private static void onPlayerDisconnected(UnturnedPlayer player)
        {
            Logger.Log($"[Info] |-| Player Disconnected : {player.SteamName} ({player.CSteamID}) ");

            RealPlayerManager.HandleDisconnect(player);
        }

        private static void onDamageTireRequested(CSteamID instigatorSteamID, InteractableVehicle vehicle, int tireIndex, ref bool shouldAllow, EDamageOrigin damageOrigin)
        {
            // Player Prevention
            if (damageOrigin == EDamageOrigin.Bullet_Explosion || damageOrigin == EDamageOrigin.Punch || damageOrigin == EDamageOrigin.Useable_Gun || damageOrigin == EDamageOrigin.Useable_Melee && !RealPlayerManager.GetRealPlayer(instigatorSteamID).IsAdmin)
                shouldAllow = false;
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
