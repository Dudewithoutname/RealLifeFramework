using System;
using System.Collections.Generic;
using UnityEngine;
using RealLifeFramework.Data;
using RealLifeFramework.RealPlayers;
using SDG.Unturned;
using Steamworks;
using System.Linq;
using RealLifeFramework.Threadding;
using System.Configuration;

namespace RealLifeFramework.Autobazar
{
    [EventHandler]
    public class CarShop : IEventComponent
    {
        private static BuyableCathegories cathegories;
        private static Dictionary<CSteamID, CarSession> sessions;

        private const ushort ui = 41868;
        private const short key = 1149;

        public void HookEvents()
        {
            sessions = new Dictionary<CSteamID, CarSession>();

            if (DataManager.ExistData("Server", "Cars"))
            {
                cathegories = DataManager.LoadData<BuyableCathegories>("Server", "Cars");
            }
            else
            {
                DataManager.CreateData("Server", "Cars", new BuyableCathegories() { 
                    Sports = new BuyableCar[]{ new BuyableCar() { Cost = 2, IconURL ="a", Id = 0, Name = "example", Pallete = "jpepe"} },
                    Hatchbacks = new BuyableCar[] { new BuyableCar() { Cost = 2, IconURL = "a", Id = 0, Name = "example", Pallete = "jpepe" } },
                    OffRoad = new BuyableCar[] { new BuyableCar() { Cost = 2, IconURL = "a", Id = 0, Name = "example", Pallete = "jpepe" } },
                    Sedans = new BuyableCar[] { new BuyableCar() { Cost = 2, IconURL = "a", Id = 0, Name = "example", Pallete = "jpepe" } },
                    SUV = new BuyableCar[] { new BuyableCar() { Cost = 2, IconURL = "a", Id = 0, Name = "example", Pallete = "jpepe" } },
                    Trucks = new BuyableCar[] { new BuyableCar() { Cost = 2, IconURL = "a", Id = 0, Name = "example", Pallete = "jpepe" } },
                    Bikes = new BuyableCar[] { new BuyableCar() { Cost = 2, IconURL = "a", Id = 0, Name = "example", Pallete = "jpepe" } },
                    Special = new BuyableCar[] { new BuyableCar() { Cost = 2, IconURL = "a", Id = 0, Name = "example", Pallete = "jpepe" } },
                });
            }

            EffectManager.onEffectButtonClicked += onButtonClicked;
            PlayerEquipment.OnPunch_Global += (equipment, punch) => onPunch(equipment.player);
            Provider.onEnemyDisconnected += onDisconnected;
        }


        private void onDisconnected(SteamPlayer player)
        {
            if (sessions.ContainsKey(player.playerID.steamID))
                sessions.Remove(player.playerID.steamID);
        }

        private void onPunch(Player player)
        {
            if (Physics.Raycast(player.look.aim.position, player.look.aim.forward, out RaycastHit hit, 3, RayMasks.BARRICADE_INTERACT | RayMasks.BARRICADE))
            {
                var transform = hit.transform;
                if ((object)transform == null) return;

                if (BarricadeManager.tryGetInfo(hit.transform, out _, out _, out _, out var index, out var region))
                {

                    var barricade = region.barricades[index];
                    if (barricade == null) return;

                    if (barricade.instanceID == RealLife.Instance.Configuration.Instance.CarShopSignIID)
                    {
                        OpenShop(RealPlayer.From(player));
                    }
                }
            }
        }

        public static void ReloadCarShop()
        {
            sessions = new Dictionary<CSteamID, CarSession>();

            if (DataManager.ExistData("server", "cars"))
            {
                cathegories = DataManager.LoadData<BuyableCathegories>("server", "cars");
            }
            else
            {
                DataManager.CreateData("server", "cars", new BuyableCathegories()
                {
                    Sports = new BuyableCar[] { new BuyableCar() { Cost = 2, IconURL = "a", Id = 0, Name = "example", Pallete = "vanilla" } },
                    Hatchbacks = new BuyableCar[] { new BuyableCar() { Cost = 2, IconURL = "a", Id = 0, Name = "example", Pallete = "jpepe" } },
                    OffRoad = new BuyableCar[] { new BuyableCar() { Cost = 2, IconURL = "a", Id = 0, Name = "example", Pallete = "none" } },
                    Sedans = new BuyableCar[] { new BuyableCar() { Cost = 2, IconURL = "a", Id = 0, Name = "example", Pallete = "jpepe" } },
                    SUV = new BuyableCar[] { new BuyableCar() { Cost = 2, IconURL = "a", Id = 0, Name = "example", Pallete = "jpepe" } },
                    Trucks = new BuyableCar[] { new BuyableCar() { Cost = 2, IconURL = "a", Id = 0, Name = "example", Pallete = "jpepe" } },
                    Bikes = new BuyableCar[] { new BuyableCar() { Cost = 2, IconURL = "a", Id = 0, Name = "example", Pallete = "jpepe" } },
                    Special = new BuyableCar[] { new BuyableCar() { Cost = 2, IconURL = "a", Id = 0, Name = "example", Pallete = "jpepe" } },

                });
            }
        }

        private static void onButtonClicked(Player player, string buttonName)
        {
            var rp = RealPlayer.From(player);

            switch (buttonName)
            {
                #region Menu

                case "bazar_btn_sports":
                    OpenCathegory(player, 0);
                    break;

                case "bazar_btn_offroad":
                    OpenCathegory(player, 1);
                    break;

                case "bazar_btn_hatchbacks":
                    OpenCathegory(player, 2);
                    break;

                case "bazar_btn_sedans":
                    OpenCathegory(player, 3);
                    break;

                case "bazar_btn_suv":
                    OpenCathegory(player, 4);
                    break;

                case "bazar_btn_trucks":
                    OpenCathegory(player, 5);
                    break;

                case "bazar_btn_bikes":
                    OpenCathegory(player, 6);
                    break;

                case "bazar_btn_special":
                    OpenCathegory(player, 7);
                    break;

                case "bazar_menu_exit":
                    CloseShop(rp);
                    break;

                #endregion

                #region catalog
                
                case "bazar_btn_zakupit":
                    TryBuyCar(rp);
                    break;

                case "bazar_btn_exit":
                    CloseShop(rp);
                    break;

                case "bazar_btn_back":
                    PrevPage(rp);
                    break;

                case "bazar_btn_next":
                    NextPage(rp);
                    break;

                #endregion

                #region colors

                case "bazar_c_black":
                    SelectColor(rp, "black");
                    break;

                case "bazar_c_white":
                    SelectColor(rp, "white");
                    break;

                case "bazar_c_orange":
                    SelectColor(rp, "orange");
                    break;

                case "bazar_c_green":
                    SelectColor(rp, "green");
                    break;

                case "bazar_c_yellow":
                    SelectColor(rp, "yellow");
                    break;

                case "bazar_c_purple":
                    SelectColor(rp, "purple");
                    break;

                case "bazar_c_red":
                    SelectColor(rp, "red");
                    break;

                case "bazar_c_blue":
                    SelectColor(rp, "blue");
                    break;

                #endregion
            }

            if (buttonName.StartsWith("car[") && buttonName.Contains("].btn"))
            {
                if (int.TryParse(buttonName[4].ToString(), out int result))
                {
                    SelectCar(rp, getRightPosReverse(result));
                }
            }
        }

        public static void OpenShop(RealPlayer player)
        {
            if (sessions.ContainsKey(player.CSteamID)) sessions.Remove(player.CSteamID);

            sessions.Add(player.CSteamID, new CarSession());

            player.Player.setPluginWidgetFlag(EPluginWidgetFlags.ForceBlur, true);
            player.Player.setPluginWidgetFlag(EPluginWidgetFlags.Modal, true);
            EffectManager.sendUIEffect(ui, key, player.TransportConnection, true);
        }

        public static void CloseShop(RealPlayer player)
        {
            player.Player.setPluginWidgetFlag(EPluginWidgetFlags.ForceBlur, false);
            player.Player.setPluginWidgetFlag(EPluginWidgetFlags.Modal, false);

            EffectManager.askEffectClearByID(ui, player.TransportConnection);

            sessions.Remove(player.CSteamID);
        }

        public static void OpenCathegory(Player player, byte id)
        {
            var rp = RealPlayer.From(player);
            sessions[rp.CSteamID].Cathegory = id;
            EffectManager.sendUIEffectVisibility(key, rp.TransportConnection, true, "bazar_menu", false);
            EffectManager.sendUIEffectVisibility(key, rp.TransportConnection, true, "bazar_catalog", true);
            LoadPage(rp, id, 0);
        }

        public static void NextPage(RealPlayer player)
        {
            var session = sessions[player.CSteamID];

            if (session.IsFinalPage) return;

            LoadPage(player, session.Cathegory, session.Page + 1);
        }

        public static void PrevPage(RealPlayer player)
        {
            var session = sessions[player.CSteamID];

            if (session.Page == 0) return;


            LoadPage(player, session.Cathegory, session.Page - 1);
        }

        public static void LoadPage(RealPlayer player, byte cathegoryId, int page)
        {
            if (sessions[player.CSteamID].IsFinalPage && sessions[player.CSteamID].Page == page) return;
            if (sessions[player.CSteamID].Page < 0) return;

            var cathegory = cathegories.GetCathegoryById(cathegoryId);
            var offset = page * 8;
            sessions[player.CSteamID].Page = page;

            if (cathegory.Length > 8)
            {
                if ((cathegory.Length - (offset + 1)) <= 0) return;
            }

            if (cathegory.Length == 0) return;

            EffectManager.sendUIEffectText(key, player.TransportConnection, true, "bazar_txt_strana", $"Strana {page+1}");

            var isLast = false;

            for (int i = 0; i < 8; i++)
            {
                var index = offset + i;

                try
                {
                    var uiPos = getRightPos(i);
                    var car = cathegory[index];

                    EffectManager.sendUIEffectVisibility(key, player.TransportConnection, true, $"car[{uiPos}]", true);
                    EffectManager.sendUIEffectText(key, player.TransportConnection, true, $"car[{uiPos}].name", car.Name);
                    EffectManager.sendUIEffectImageURL(key, player.TransportConnection, true, $"car[{uiPos}].image", car.IconURL);

                }
                catch 
                {
                    var uiPos = getRightPos(i);

                    EffectManager.sendUIEffectVisibility(key, player.TransportConnection, true, $"car[{uiPos}]", false);

                    if (i == 7) isLast = true;
                    continue;
                }
            }

            if (8 == cathegory.Length && offset == 0) isLast = true;

            sessions[player.CSteamID].IsFinalPage = isLast;

            return;
        }

        // Why Cuz I Am Fucking Dumb :) and i've wrongly named UI ....
        private static int getRightPos(int index)
        {
            if (index >= 4)
            {
                switch (index)
                {
                    case 4:
                        return 6;

                    case 5:
                        return 4;

                    case 6:
                        return 7;

                    case 7:
                        return 5;
                }
            }

            return index;
        }

        private static int getRightPosReverse(int index)
        {
            if (index >= 4)
            {
                switch (index)
                {
                    case 6:
                        return 4;

                    case 4:
                        return 5;

                    case 7:
                        return 6;

                    case 5:
                        return 7;
                }
            }

            return index;
        }

        public static void SelectCar(RealPlayer player, int uiIndex)
        {
            var session = sessions[player.CSteamID];
            var cathegory = cathegories.GetCathegoryById(session.Cathegory);
            var index = session.Offset + uiIndex;

            var car = cathegory[index];
            session.SelectedCar = car;
            session.ColorOffset = 0;

                
            var vehicle = (VehicleAsset)Assets.find(EAssetType.VEHICLE, car.Id);

            if (vehicle == null) return;

            EffectManager.sendUIEffectVisibility(key, player.TransportConnection, true, $"bazar_catalog_sidebar", true);
            EffectManager.sendUIEffectImageURL(key, player.TransportConnection, true, $"bazar_ins_image", car.IconURL);
            EffectManager.sendUIEffectText(key, player.TransportConnection, true, $"bazar_ins_name", car.Name);

            EffectManager.sendUIEffectText(key, player.TransportConnection, true, $"bazar_ins_speed", $": {MeasurementTool.speedToKPH(vehicle.speedMax).ToString()} km/h");
            EffectManager.sendUIEffectText(key, player.TransportConnection, true, $"bazar_ins_fuel", (vehicle.fuelMax > 0) ? $": {vehicle.fuelMax / 10} l" : ": ");
            EffectManager.sendUIEffectText(key, player.TransportConnection, true, $"bazar_ins_cost", $": {Currency.FormatMoney(car.Cost.ToString())}");

            if (session.SelectedCar.Pallete == "none" || CarPalletes.GetPallete(session.SelectedCar.Pallete) == null)
            {
                EffectManager.sendUIEffectVisibility(key, player.TransportConnection, true, $"bazar_ins_colors", false);
            }
            else
            {
                EffectManager.sendUIEffectVisibility(key, player.TransportConnection, true, $"bazar_ins_colors", true);
                EffectManager.sendUIEffectText(key, player.TransportConnection, true, $"bazar_ins_name", $"<color={CarPalletes.Hex["black"]}>{car.Name}</color>");
            }

            EffectManager.sendUIEffectVisibility(key, player.TransportConnection, true, "bazar_c_gray", false); // later remove
        }

        public static void SelectColor(RealPlayer player, string colorId)
        {
            var session = sessions[player.CSteamID];
            var pal = CarPalletes.GetPallete(session.SelectedCar.Pallete);

            if (session.SelectedCar.Pallete == "none") return;
            if (pal == null) return;

            session.ColorOffset = pal[colorId];
            EffectManager.sendUIEffectText(key, player.TransportConnection, true, $"bazar_ins_name", $"<color={CarPalletes.Hex[colorId]}>{session.SelectedCar.Name}</color>");
        }

        public static void TryBuyCar(RealPlayer player)
        {
            var session = sessions[player.CSteamID];
            var car = session.SelectedCar;

            if (car != null)
            {
                if (car.Cost <= player.CreditCardMoney)
                {
                    player.CreditCardMoney -= car.Cost;
                    VehicleManager.spawnVehicleV2((ushort)(car.Id + session.ColorOffset), RealLife.Instance.Configuration.Instance.CarSpawnPoint, Quaternion.identity);
                    CloseShop(player);
                }
            }
        }
    }
}
