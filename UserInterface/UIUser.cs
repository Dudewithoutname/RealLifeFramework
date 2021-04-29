using System;
using System.Collections.Generic;
using System.Text;
using SDG.Unturned;
using RealLifeFramework.Players;
using Rocket.Unturned.Player;

namespace RealLifeFramework.UserInterface
{
    public class UIUser
    {
        public static ushort HUDId = 41825;

        public RealPlayer Player { get; set; }
        public PlayerLife Stats { get; set; }
        public short HUDkey => 1205;

        public UIUser(RealPlayer player)
        {
            Player = player;
        }

        public void CreatePlayerUI()
        {
            EffectManager.sendUIEffect(HUDId, HUDkey, true);
            EffectManager.sendUIEffectImageURL(HUDkey, Player.TransportConnection, true, "hud_profileicon", UnturnedPlayer.FromPlayer(Player.Player).SteamProfile.AvatarFull.ToString() );
            EffectManager.sendUIEffectText(HUDkey, Player.TransportConnection, true, "hud_name", Player.Name);
            EffectManager.sendUIEffectText(HUDkey, Player.TransportConnection, true, "hud_job", Player.Name);

            EffectManager.sendUIEffectText(HUDkey, Player.TransportConnection, true, "hud_health", Player.Name);
            EffectManager.sendUIEffectText(HUDkey, Player.TransportConnection, true, "hud_food", Player.Name);
            EffectManager.sendUIEffectText(HUDkey, Player.TransportConnection, true, "hud_drink", Player.Name);
            EffectManager.sendUIEffectText(HUDkey, Player.TransportConnection, true, "hud_stamina", Player.Name);

            EffectManager.sendUIEffectText(HUDkey, Player.TransportConnection, true, "hud_money", $"{Player.Exp} ");
            EffectManager.sendUIEffectText(HUDkey, Player.TransportConnection, true, "hud_time", "12:00 AM");

            EffectManager.sendUIEffectText(HUDkey, Player.TransportConnection, true, "hud_lvl", $"{Player.Level} LvL");
            EffectManager.sendUIEffectText(HUDkey, Player.TransportConnection, true, "hud_exp", $"{Player.Exp}<color=#D164FF> / {Player.MaxExp}</color>");

        }

        private string GetFormatedMoney()
        {
            string input = Player.Money.ToString();
            string output = "";

        }


    }
}
