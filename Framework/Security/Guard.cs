using System;
using System.Collections.Generic;
using SDG.Unturned;
using Steamworks;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using Rocket.Unturned;
using UnityEngine;
using SDG.Framework.IO.Deserialization;
using RealLifeFramework.API.Models;
using Newtonsoft.Json;
using RealLifeFramework.Data;
using RealLifeFramework.Data.Models;

namespace RealLifeFramework.Security
{
    [EventHandler]
    public class Guard : IEventComponent
    {
        public void HookEvents()
        {
            Provider.onBanPlayerRequested += sendBanInfo;
        }

        private void sendBanInfo(CSteamID instigator, CSteamID playerToBan, uint ipToBan, ref string reason, ref uint duration, ref bool shouldVanillaBan)
        {
            var player = DataManager.LoadPlayer(playerToBan);
            var admin = UnturnedPlayer.FromCSteamID(instigator);
            string name = (admin != null)? admin.DisplayName : "Console";

            Api.Send("/info/bans", JsonConvert.SerializeObject(
                new Ban() 
                {
                     characterName = (player != null)? player.name : string.Empty,
                     provider = name,
                     reason = reason,
                     time = (int)duration,
                     steamId = playerToBan.ToString(),
                }
            ));;
        }
    }
}