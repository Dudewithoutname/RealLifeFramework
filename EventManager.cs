using System;
using RealLifeFramework.Players;
using RealLifeFramework.Skills;
using Rocket.Unturned;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;

namespace RealLifeFramework
{
    // tato picovina bola zdekompilovana lebo sa mi dojebal Pc skurvenym unity enginom fixuj kokotko
    public static class EventManager
    {
        public static void Load()
        {
            U.Events.OnPlayerConnected += OnPlayerConnected;
            U.Events.OnPlayerDisconnected += OnPlayerDisconnected;

            DamageTool.damagePlayerRequested += onPlayerDamageRequest;

            ItemManager.onTakeItemRequested = onTakeItemRequested;
            BarricadeManager.onOpenStorageRequested = onOpenStorageRequested;

            EffectManager.onEffectButtonClicked = onEffectButtonClicked;
            EffectManager.onEffectTextCommitted = onEffectTextCommited;

            // * Skills
            Player.onPlayerStatIncremented += SkillManager.HandleStatIncremented;
            UseableConsumeable.onConsumePerformed += SkillManager.HandleConsume;

            Logger.Log("[EventManager] Succesfully added subscriptions to events");
        }

        private static void OnPlayerConnected(UnturnedPlayer player)
        {
            Logger.Log($"[Info] Player Connected : {player.SteamName} ({player.CSteamID}) ({player.Player.channel.GetOwnerTransportConnection().GetAddress()})");
           
            RealPlayerManager.InitializePlayer(player);
            player.Player.setPluginWidgetFlag(EPluginWidgetFlags.ShowInteractWithEnemy, false);

            player.Player.inventory.onInventoryAdded = OnInventoryItemAdded;
        }

        private static void OnPlayerDisconnected(UnturnedPlayer player)
        {
            Logger.Log($"[Info] Player Connected : {player.SteamName} ({player.CSteamID}) ");

            RealPlayerManager.HandleDisconnect(player);
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
                        RealPlayerCreation.TryCreateCharacter(player.channel.owner.playerID.steamID);
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
