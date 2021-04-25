using RealLifeFramework.Players;
using Rocket.Unturned;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;

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

            Logger.Log("[EventManager] Succesfully added subscriptions to events");
        }

        public static void OnPlayerConnected(UnturnedPlayer player)
        {
            Logger.Log($"[Info] Player Connected : {player.SteamName} ({player.CSteamID}) ({player.Player.channel.GetOwnerTransportConnection().GetAddress()})");
           
            RealPlayerManager.InitializePlayer(player);

            player.Player.inventory.onInventoryAdded = OnInventoryItemAdded;
        }

        public static void OnPlayerDisconnected(UnturnedPlayer player)
        {
            Logger.Log($"[Info] Player Connected : {player.SteamName} ({player.CSteamID}) ");

            RealPlayerManager.HandleDisconnect(player);
        }

        public static void OnInventoryItemAdded(byte page, byte index, ItemJar jar) => Logger.Log("kokot");

        public static void onTakeItemRequested(Player player, byte x, byte y, uint instanceID, byte to_x, byte to_y, byte to_rot, byte to_page, ItemData itemData, ref bool shouldAllow)
        {
            RealPlayerManager.GetRealPlayer(player);
            shouldAllow = false;
        }

        public static void onOpenStorageRequested(CSteamID instigator, InteractableStorage storage, ref bool shouldAllow)
        {
        }

        public static void onPlayerDamageRequest(ref DamagePlayerParameters parameters, ref bool shouldAllow)
        {
        }

        public static void onEffectButtonClicked(Player player, string buttonName)
        {

            /*if (RealPlayerCreation.PrePlayers.ContainsKey(((SteamPlayer)player.channel.owner).playerID.steamID))
            {
                string str = buttonName;
                if (!(str == "g_male"))
                {
                    if (!(str == "g_female"))
                    {
                        if (!(str == "createCharacterbtn"))
                            return;
                        RealPlayerCreation.ValidateCharacter(((SteamPlayer)player.channel.owner).playerID.steamID);
                    }
                    else
                        RealPlayerCreation.SetGender(((SteamPlayer)player.channel.owner).playerID.steamID, (byte)1);
                }
                else
                    RealPlayerCreation.SetGender(((SteamPlayer)player.channel.owner).playerID.steamID, (byte)0);
            }
            else
            {
                string str = buttonName;
                if (!(str == "joindiscord_btn"))
                {
                    if (!(str == "joinsteam_btn"))
                    {
                        if (str == "continue_btn" && !RealLife.Instance.RealPlayers.ContainsKey(((SteamPlayer)player.channel.owner).playerID.steamID))
                            RealPlayerCreation.OpenCreation(player);
                    }
                    else
                        player.sendBrowserRequest("Steam Group", RealLife.Instance.Configuration.Instance.SteamGroupInvite);
                }
                else
                    player.sendBrowserRequest("Discord Invite", RealLife.Instance.Configuration.Instance.DiscordInvite);
            }*/
        }

        public static void onEffectTextCommited(SDG.Unturned.Player player, string inputName, string text)
        {
            /*if (!RealPlayerCreation.PrePlayers.ContainsKey(player.channel.owner.playerID.steamID))
                return;

            if (!(str == "input_first"))
            {
                if (!(str == "input_last"))
                {
                    if (str == "input_age")
                        RealPlayerCreation.PrePlayers[player.channel.owner.playerID.steamID].Age = Convert.ToByte(text);
                }
                else
                    RealPlayerCreation.PrePlayers[player.channel.owner.playerID.steamID].LastName = text;
            }
            else
                RealPlayerCreation.PrePlayers[player.channel.owner.playerID.steamID].FirstName = text;*/
        }
    }
}
