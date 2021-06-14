using System;
using System.Collections.Generic;
using System.Text;
using SDG.Unturned;
using RealLifeFramework.Players;
using Rocket.Unturned.Player;
using RealLifeFramework.Chatting;
using RealLifeFramework.Patches;

namespace RealLifeFramework.UserInterface
{
    public class HUD
    {
        public RealPlayer RPlayer { get; set; }
        public short HudKey => 1205;

        public List<Widget> Widgets;
        public bool HasSeatBelt { get; set; }

        public HUD(RealPlayer player)
        {
            RPlayer = player;
            Widgets = new List<Widget>();
            HasSeatBelt = false;
            CreatePlayerUI();
            player.Keyboard.KeyDown += seatBeltAction;
        }

        private void seatBeltAction(Player player, UnturnedKey key)
        {
            if(key == UnturnedKey.CodeHotkey1 && ((Object)player.movement.getVehicle()) != null)
            {
                if(player.movement.getVehicle().asset.engine == EEngine.CAR)
                {
                    if (HasSeatBelt)
                    {
                        HasSeatBelt = false;
                        EffectManager.sendUIEffect(HUDComponent.RemoveBelt, 956, RPlayer.TransportConnection, false);
                        UpdateComponent(HUDComponent.Seatbelt[1], false);
                        UpdateComponent(HUDComponent.Seatbelt[0], true);
                    }
                    else
                    {
                        HasSeatBelt = true;
                        EffectManager.sendUIEffect(HUDComponent.UseBelt, 956, RPlayer.TransportConnection, false);
                        UpdateComponent(HUDComponent.Seatbelt[0], false);
                        UpdateComponent(HUDComponent.Seatbelt[1], true);
                    }
                }
            }
        }

        public void CreatePlayerUI()
        {
            RPlayer.Player.setPluginWidgetFlag(EPluginWidgetFlags.ShowInteractWithEnemy, false);
            RPlayer.Player.setPluginWidgetFlag(EPluginWidgetFlags.ShowLifeMeters, false);
            RPlayer.Player.setPluginWidgetFlag(EPluginWidgetFlags.ShowStatusIcons, false);
            RPlayer.Player.setPluginWidgetFlag(EPluginWidgetFlags.ShowUseableGunStatus, false);

            EffectManager.sendUIEffect(UI.HudID, HudKey, true);

            UpdateComponent(HUDComponent.Health, RPlayer.Player.life.health.ToString());
            UpdateComponent(HUDComponent.Food, RPlayer.Player.life.food.ToString());
            UpdateComponent(HUDComponent.Water, RPlayer.Player.life.water.ToString());
            UpdateComponent(HUDComponent.Stamina, RPlayer.Player.life.stamina.ToString());
            if (RPlayer.Player.life.isBroken)
            {
                SendWidget(EWidgetType.BrokenBone);
            }

            if (RPlayer.Player.life.isBleeding)
            {
                SendWidget(EWidgetType.Bleeding);
            }

            UpdateComponent(HUDComponent.Level);
            UpdateComponent(HUDComponent.Exp);
            // ADD MONEY
        }

        public void UpdateComponent(string component, string value)
        {
            switch (component)
            {
                case HUDComponent.Voice:
                    EffectManager.sendUIEffectImageURL(HudKey, RPlayer.TransportConnection, true, component, value);
                    break;

                case HUDComponent.Time:
                    EffectManager.sendUIEffectText(HudKey, RPlayer.TransportConnection, true, component, value);
                    break;

                default:
                    EffectManager.sendUIEffectText(HudKey, RPlayer.TransportConnection, true, component, value);
                    break;
            }
        }

        public void UpdateComponent(string component, bool value)
        {
            EffectManager.sendUIEffectVisibility(HudKey, RPlayer.TransportConnection, true, component, value);
        }

        public void UpdateComponent(string component)
        {
            switch (component)
            {
                case HUDComponent.Level:
                    EffectManager.sendUIEffectText(HudKey, RPlayer.TransportConnection, true, component, formatLevel());
                    break;

                case HUDComponent.Exp:
                    EffectManager.sendUIEffectText(HudKey, RPlayer.TransportConnection, true, component, formatExp());
                    break;

                case HUDComponent.Wallet:
                    //EffectManager.sendUIEffectText(HudKey, RPlayer.TransportConnection, true, component, ""); // tODO getFormatedMoney()
                    break;

                case HUDComponent.Credit:
                    //EffectManager.sendUIEffectText(HudKey, RPlayer.TransportConnection, true, component, ""); // tODO getFormatedMoney()
                    break;
            }
        }

       

        #region widgets
        public void SendWidget(EWidgetType type)
        {
            bool isAlreadyActive = false;
            string image = WidgetInfo.GetImage(type);

            for (int i = 0; i < Widgets.Count; i++)
            {
                if(Widgets[i].Type == type)
                {
                    isAlreadyActive = true;
                    break;
                }
            }

            if (Widgets.Count < 5 && !isAlreadyActive)
            {
                Widget widget = new Widget(Widgets.Count, image, type);
                Widgets.Add(widget);
                EffectManager.sendUIEffectVisibility(HudKey, RPlayer.TransportConnection, true, $"widget{widget.Index}", true);
                EffectManager.sendUIEffectImageURL(HudKey, RPlayer.TransportConnection, true, $"widget{widget.Index}Icon", widget.Image);
            }
        }

        public void RemoveWidget(EWidgetType type)
        {
            Widget widget = null;

            for (int i = 0; i < Widgets.Count; i++)
            {
                if (Widgets[i].Type == type)
                {
                    widget = Widgets[i];
                    break;
                }
            }

            if (widget != null)
            {
                if((widget.Index + 1) != Widgets.Count)
                {
                    for (ushort i = 0; i < Widgets.Count; i++)
                        EffectManager.sendUIEffectVisibility(HudKey, RPlayer.TransportConnection, true, $"widget{i}", false);

                    Widgets.ForEach((wg) =>
                    {
                        if (wg.Index > widget.Index)
                            wg.Index--;
                    });

                    Widgets.RemoveAt(widget.Index);

                    for (int i = 0; i < Widgets.Count; i++)
                    {
                        EffectManager.sendUIEffectVisibility(HudKey, RPlayer.TransportConnection, true, $"widget{i}", true);
                        EffectManager.sendUIEffectImageURL(HudKey, RPlayer.TransportConnection, true, $"widget{i}Icon", Widgets[i].Image);
                    }
                }
                else
                {
                    EffectManager.sendUIEffectImageURL(HudKey, RPlayer.TransportConnection, true, $"widget{widget.Index}Icon", Widgets[widget.Index].Image);
                    EffectManager.sendUIEffectVisibility(HudKey, RPlayer.TransportConnection, true, $"widget{widget.Index}", false);
     
                    Widgets.RemoveAt(widget.Index);
                }
            }
        }

        #endregion

        #region formating

        public static string FormatTime(ushort hours, ushort minutes)
        {
            if (minutes.ToString().Length < 2)
                return $"{hours}:0{minutes}";
            else
                return $"{hours}:{minutes}";
        }

        private string formatLevel() => $"<color=#FC3A8C>LVL</color> <color=#FC8EBD>{RPlayer.Level}</color>";
        private string formatExp() => $"<color=#FC8EBD>{formatBigNums(RPlayer.Exp)} / {formatBigNums(RPlayer.MaxExp)}</color> <color=#FC3A8C>XP</color>";
        
        private string formatBigNums(uint value)
        {
            string output = "";

            if(value >= 1000)
            {
                decimal k = (decimal)value / 20;
                output = $"{Math.Round(k, 1)}K";
            }
            else
            {
                return ((int)value).ToString();
            }

            return output;
        }

        private string formatMoney(string money)
        {
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
