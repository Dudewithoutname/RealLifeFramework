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
            Stats = player.Player.life;

            Player.Player.skills.onExperienceUpdated += updateMoneyUI;
            Stats.onHealthUpdated += updateHealthUI;
            Stats.onFoodUpdated += updateFoodUI;
            Stats.onWaterUpdated += updateWaterUI;
            Stats.onStaminaUpdated += updateStaminaUI;
        }

        public void Destroy()
        {
            Player.Player.skills.onExperienceUpdated += updateMoneyUI;
            Stats.onHealthUpdated -= updateHealthUI;
            Stats.onFoodUpdated -= updateFoodUI;
            Stats.onWaterUpdated -= updateWaterUI;
            Stats.onStaminaUpdated -= updateStaminaUI;
        }

        public void CreatePlayerUI()
        {
            EffectManager.sendUIEffect(HUDId, HUDkey, true);
            EffectManager.sendUIEffectImageURL(HUDkey, Player.TransportConnection, true, "hud_profileicon", UnturnedPlayer.FromPlayer(Player.Player).SteamProfile.AvatarFull.ToString());
            EffectManager.sendUIEffectText(HUDkey, Player.TransportConnection, true, "hud_name", Player.Name);
            EffectManager.sendUIEffectText(HUDkey, Player.TransportConnection, true, "hud_job", "Unemployed");

            EffectManager.sendUIEffectText(HUDkey, Player.TransportConnection, true, "hud_health", Stats.health.ToString());
            EffectManager.sendUIEffectText(HUDkey, Player.TransportConnection, true, "hud_food", Stats.food.ToString());
            EffectManager.sendUIEffectText(HUDkey, Player.TransportConnection, true, "hud_drink", Stats.water.ToString());
            EffectManager.sendUIEffectText(HUDkey, Player.TransportConnection, true, "hud_stamina", Stats.stamina.ToString());

            EffectManager.sendUIEffectText(HUDkey, Player.TransportConnection, true, "hud_money", getFormatedMoney());
            EffectManager.sendUIEffectText(HUDkey, Player.TransportConnection, true, "hud_time", "12:00 AM");

            EffectManager.sendUIEffectText(HUDkey, Player.TransportConnection, true, "hud_lvl", getFormatedLevel());
            EffectManager.sendUIEffectText(HUDkey, Player.TransportConnection, true, "hud_exp", getFormatedExp());
        }

        // format money
        public void UpdateLevel() => EffectManager.sendUIEffectText(HUDkey, Player.TransportConnection, true, "hud_money", getFormatedExp());
        public void UpdateExp() => EffectManager.sendUIEffectText(HUDkey, Player.TransportConnection, true, "hud_exp", getFormatedExp());

        private void updateMoneyUI(uint newExperience) => EffectManager.sendUIEffectText(HUDkey, Player.TransportConnection, true, "hud_money", getFormatedMoney(newExperience));
        private void updateHealthUI(byte newHealth) => EffectManager.sendUIEffectText(HUDkey, Player.TransportConnection, true, "hud_health", newHealth.ToString());
        private void updateFoodUI(byte newFood) => EffectManager.sendUIEffectText(HUDkey, Player.TransportConnection, true, "hud_food", newFood.ToString());
        private void updateWaterUI(byte newWater) => EffectManager.sendUIEffectText(HUDkey, Player.TransportConnection, true, "hud_drink", newWater.ToString());
        private void updateStaminaUI(byte newStamina) => EffectManager.sendUIEffectText(HUDkey, Player.TransportConnection, true, "hud_stamina", newStamina.ToString());

        private string getFormatedLevel() => $"{Player.Level} LvL";
        private string getFormatedExp() => $"{Player.Exp}<color=#D164FF> / {Player.MaxExp}</color>";

        #region Money
        
        private string getFormatedMoney(uint value)
        {
            string money = value.ToString();
            string output = "";

            if (money.Length > 3)
            {
                decimal x = Math.Floor(((decimal)money.Length / 3));
                int remainder = Convert.ToInt32(money.Length - (x * 3));

                if (Math.Round((decimal)money.Length / 3, 2) == (money.Length / 3))
                {
                    for (var i = 0; i < money.Length; i += 3)
                        output += money.Substring(i, 3) + " ";

                    return output + " $";
                }
                else
                {
                    output += money.Substring(0, remainder) + " ";

                    for (var i = 0; i < (money.Length - remainder); i += 3)
                        output += money.Substring(i, 3) + " ";

                    return output + " $";
                }
            }
            else
            {
                return output + " $";
            }
        }
        private string getFormatedMoney()
        {
            string money = Player.Money.ToString();
            string output = "";

            if (money.Length > 3)
            {
                decimal x = Math.Floor(((decimal)money.Length / 3));
                int remainder = Convert.ToInt32(money.Length - (x * 3));

                if ( Math.Round((decimal)money.Length / 3, 2) == (money.Length / 3) )
                {
                    for (var i = 0; i < money.Length; i += 3)
                        output += money.Substring(i, 3) + " ";

                    return output + " $";
                }
                else
                {
                    output += money.Substring(0, remainder) + " ";

                    for (var i = 0; i < (money.Length - remainder); i += 3)
                        output += money.Substring(i, 3) + " ";

                    return output + " $";
                }
            }
            else
            {
                return output + " $";
            }
        }

        #endregion
    }
}
