using System;
using System.Collections.Generic;
using System.Text;
using SDG.Unturned;
using RealLifeFramework.Players;
using Rocket.Unturned.Player;
using RealLifeFramework.Chatting;

namespace RealLifeFramework.UserInterface
{
    public class HUD
    {
        public static ushort HUDId = 41825;
        public RealPlayer RPlayer { get; set; }
        public short HUDkey => 1205;
        public List<Widget> Widgets;

        public HUD(RealPlayer player)
        {
            RPlayer = player;
            Widgets = new List<Widget>();
            CreatePlayerUI();
        }

        public void CreatePlayerUI()
        {
            EffectManager.sendUIEffect(HUDId, HUDkey, true);
            EffectManager.sendUIEffectImageURL(HUDkey, RPlayer.TransportConnection, true, "hud_profileicon", UnturnedPlayer.FromPlayer(RPlayer.Player).SteamProfile.AvatarFull.ToString());
            EffectManager.sendUIEffectText(HUDkey, RPlayer.TransportConnection, true, "hud_name", RPlayer.Name);
            EffectManager.sendUIEffectText(HUDkey, RPlayer.TransportConnection, true, "hud_job", "Unemployed");

            EffectManager.sendUIEffectText(HUDkey, RPlayer.TransportConnection, true, "hud_health", RPlayer.Player.life.health.ToString());
            EffectManager.sendUIEffectText(HUDkey, RPlayer.TransportConnection, true, "hud_food", RPlayer.Player.life.food.ToString());
            EffectManager.sendUIEffectText(HUDkey, RPlayer.TransportConnection, true, "hud_drink", RPlayer.Player.life.water.ToString());
            EffectManager.sendUIEffectText(HUDkey, RPlayer.TransportConnection, false, "hud_stamina", RPlayer.Player.life.stamina.ToString());

            EffectManager.sendUIEffectText(HUDkey, RPlayer.TransportConnection, true, "hud_money", getFormatedMoney(RPlayer.Money));
            EffectManager.sendUIEffectText(HUDkey, RPlayer.TransportConnection, false, "hud_time", "0:00");

            EffectManager.sendUIEffectText(HUDkey, RPlayer.TransportConnection, true, "hud_lvl", getFormatedLevel());
            EffectManager.sendUIEffectText(HUDkey, RPlayer.TransportConnection, true, "hud_exp", getFormatedExp());

        }


        public void SendWidget(string image, EWidgetType type) 
        { 
            if(Widgets.Count < 5)
            {
                // "https://upload.wikimedia.org/wikipedia/commons/thumb/6/65/Circle-icons-car.svg/512px-Circle-icons-car.svg.png"
                Widget widget = new Widget(Widgets.Count, image, type);
                Widgets.Add(widget);

                EffectManager.sendUIEffect(widget.Id, widget.Key, RPlayer.TransportConnection, true);
                EffectManager.sendUIEffectImageURL(widget.Key, RPlayer.TransportConnection, true, $"widget{widget.Index}", widget.Image);
            }
        }

        public void RemoveWidget(int indx)
        {
            // ###S
            // ###
            if (Widgets.Count == indx)
            {
                Widget widget = Widgets[indx];
                EffectManager.askEffectClearByID(widget.Id, RPlayer.TransportConnection);

                Widgets.RemoveAt(widget.Id);
            }
            // ##S#
            // ###
            else if (Widgets.Count == (indx + 1))
            {
                Widget widget = Widgets[indx];
                Widget last = Widgets[Widgets.Count];

                EffectManager.askEffectClearByID(last.Id, RPlayer.TransportConnection);

                last.Index = widget.Index;

                EffectManager.sendUIEffectImageURL(last.Key, RPlayer.TransportConnection, true, $"widget{last.Index}", last.Image);
                Widgets.RemoveAt(widget.Index);
            }
            else
            {
                Widgets.RemoveAt(indx);
                EffectManager.askEffectClearByID((ushort)(Widget.baseId + Widgets.Count + 1), RPlayer.TransportConnection);

                for (ushort i = 0; i < Widgets.Count; i++)
                {
                    Widgets[i].Index = i;
                    EffectManager.sendUIEffectImageURL(Widgets[i].Key, RPlayer.TransportConnection, true, $"widget{Widgets[i].Index}", Widgets[i].Image);
                }
            }
        }

        public void UpdateLevel() => EffectManager.sendUIEffectText(HUDkey, RPlayer.TransportConnection, true, "hud_lvl", getFormatedLevel());
        public void UpdateExp() => EffectManager.sendUIEffectText(HUDkey, RPlayer.TransportConnection, true, "hud_exp", getFormatedExp());
        public void UpdateVoice(EPlayerVoiceMode voicemode) => EffectManager.sendUIEffectText(HUDkey, RPlayer.TransportConnection, true, "voice", VoiceChat.GetVoiceModeName(voicemode));
        
        public void UpdateTime(ushort hours, ushort minutes) => EffectManager.sendUIEffectText(HUDkey, RPlayer.TransportConnection, true, "hud_time", getFormatedTime(hours, minutes));
        public void UpdateMoney(uint newExperience) => EffectManager.sendUIEffectText(HUDkey, RPlayer.TransportConnection, true, "hud_money", getFormatedMoney(newExperience));
        public void UpdateHealth(byte newHealth) => EffectManager.sendUIEffectText(HUDkey, RPlayer.TransportConnection, true, "hud_health", newHealth.ToString());
        public void UpdateFood(byte newFood) => EffectManager.sendUIEffectText(HUDkey, RPlayer.TransportConnection, true, "hud_food", newFood.ToString());
        public void UpdateWater(byte newWater) => EffectManager.sendUIEffectText(HUDkey, RPlayer.TransportConnection, true, "hud_drink", newWater.ToString());
        public void UpdateStamina(byte newStamina) => EffectManager.sendUIEffectText(HUDkey, RPlayer.TransportConnection, true, "hud_stamina", newStamina.ToString());

        #region private

        private string getFormatedTime(ushort hours, ushort minutes)
        {
            if (minutes.ToString().Length < 2)
                return $"{hours}:0{minutes}";
            else
                return $"{hours}:{minutes}";
        }
        private string getFormatedLevel() => $"{RPlayer.Level} LvL";
        private string getFormatedExp() => $"{RPlayer.Exp}<color=#D164FF> / {RPlayer.MaxExp}</color>";
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
                return money + " $";
            }
        }

        #endregion
    }
}
