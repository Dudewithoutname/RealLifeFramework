using RealLifeFramework.Threadding;
using RealLifeFramework.RealPlayers;
using RealLifeFramework.UserInterface;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RealLifeFramework.ATM
{
    [EventHandler]
    public class ATMManager : IEventComponent
    {
        private static Dictionary<CSteamID, ATMSession> sessions;
        private const ushort idATM = 1070; //42568;
        private const ushort uiATM = 41866;
        private const short keyATM = 1138;

        public void HookEvents()
        {
            sessions = new Dictionary<CSteamID, ATMSession>();

            PlayerEquipment.OnPunch_Global += (equipment, punch) => onPunch(equipment.player);
            EffectManager.onEffectButtonClicked += onButtonClicked;
            EffectManager.onEffectTextCommitted += onTextCommitted;
        }

        #region events
        private void onPunch(Player player)
        {
            if (Physics.Raycast(player.look.aim.position, player.look.aim.forward, out RaycastHit hit, 25f, RayMasks.BARRICADE | RayMasks.BARRICADE_INTERACT))
            {
                if (BarricadeManager.tryGetInfo(hit.transform, out _, out _, out _, out var index, out var region))
                {
                    var barricade = region.barricades[index].barricade;

                    if (barricade.id == idATM)
                    {
                        OpenATM(player);
                    }
                }
            }
        }

        private void onButtonClicked(Player player, string buttonName)
        {
            if (buttonName.StartsWith("bank"))
            {
                switch (buttonName)
                {
                    case "bank_btn_withdraw":
                        OpenATMCathegory(player, ATMCathegory.Withdraw);
                        break;

                    case "bank_btn_deposit":
                        OpenATMCathegory(player, ATMCathegory.Deposit);
                        break;

                    case "bank_btn_transfare":
                        OpenATMCathegory(player, ATMCathegory.Transfare);
                        break;

                    case "bank_btn_exit":
                        CloseATM(player);
                        break;
                    
                    // * withdraw

                    case "bank_with_btn_confirm":
                        TryConfirm(player, ATMCathegory.Withdraw);
                        break;

                    // * deposit

                    case "bank_depo_btn_confirm":
                        TryConfirm(player, ATMCathegory.Deposit);
                        break;

                    // * transfare

                    case "bank_tran_btn_confirm":
                        TryConfirm(player, ATMCathegory.Transfare);
                        break;
                }
            }
        }

        private void onTextCommitted(Player player, string inputName, string text)
        {
            if (inputName.StartsWith("bank"))
            {
                var session = sessions[player.channel.owner.playerID.steamID];

                if (session == null) return;
                
                switch (inputName)
                {
                    // * withdraw

                    case "bank_with_inp_money":
                        session.Data[0] = text;
                        break;

                    // * deposit

                    case "bank_depo_inp_money":
                        session.Data[1] = text;
                        break;

                    case "bank_depo_inp_note":
                        session.Data[4] = text;
                        break;

                    // * transfare

                    case "bank_tran_inp_money":
                        session.Data[2] = text;
                        break;

                    case "bank_tran_inp_user":
                        session.Data[3] = text;
                        break;

                }
            }
        }

        #endregion

        public static void OpenATM(Player player)
        {
            var rp = RealPlayer.From(player);
            sessions.Add(player.channel.owner.playerID.steamID, new ATMSession(player));
            
            player.setPluginWidgetFlag(EPluginWidgetFlags.ForceBlur, true);
            player.setPluginWidgetFlag(EPluginWidgetFlags.Modal, true);

            EffectManager.sendUIEffect(uiATM, keyATM, player.channel.GetOwnerTransportConnection(), true);
            EffectManager.sendUIEffectText(keyATM, player.channel.GetOwnerTransportConnection(), true, "bank_txt_money", Currency.FormatMoney(rp.CreditCardMoney.ToString()));
        }

        public static void CloseATM(Player player)
        {
            var session = sessions[player.channel.owner.playerID.steamID];

            if (session == null) return;

            if (session.CurrentCathegory == ATMCathegory.Menu)
            {
                player.setPluginWidgetFlag(EPluginWidgetFlags.ForceBlur, false);
                player.setPluginWidgetFlag(EPluginWidgetFlags.Modal, false);

                EffectManager.askEffectClearByID(uiATM, player.channel.GetOwnerTransportConnection());

                sessions.Remove(player.channel.owner.playerID.steamID);
            }
            else
            {

                ClearError(player);
                OpenATMCathegory(player, ATMCathegory.Menu);

                EffectManager.sendUIEffectText(keyATM, player.channel.GetOwnerTransportConnection(), true, "bank_txt_money", Currency.FormatMoney(RealPlayer.From(player).CreditCardMoney.ToString()));
            }
        }

        public static void OpenATMCathegory(Player player, ATMCathegory cathegory)
        {
            var session = sessions[player.channel.owner.playerID.steamID];

            if (session.CurrentCathegory == cathegory) return;
            if (session.depositAllCycle) session.depositAllCycle = false;

            EffectManager.sendUIEffectText(keyATM, player.channel.GetOwnerTransportConnection(), true, "bank_header", (cathegory == ATMCathegory.Menu) ? "" : getCatSlovakName(cathegory));

            EffectManager.sendUIEffectVisibility(keyATM, player.channel.GetOwnerTransportConnection(), true, getCatName(session.CurrentCathegory), false);
            EffectManager.sendUIEffectVisibility(keyATM, player.channel.GetOwnerTransportConnection(), true, getCatName(cathegory), true);

            session.CurrentCathegory = cathegory;
        }

        public static void TryConfirm(Player player, ATMCathegory cathegory)
        {
            switch (cathegory)
            {
                case ATMCathegory.Withdraw:
                    withdraw(player);
                    break;

                case ATMCathegory.Deposit:
                    deposit(player);
                    break;

                case ATMCathegory.Transfare:
                    transfare(player);
                    break;
            }
        }

        private static void withdraw(Player player)
        {
            var session = sessions[player.channel.owner.playerID.steamID];
            var rp = RealPlayer.From(player);

            if (session == null) return;

            uint baseAmount = Convert.ToUInt32(session.Data[0]);

            if(baseAmount == 0)
            {
                SendError(player, "Neplatna Hodnota");
                return;
            }

            if(rp.CreditCardMoney < baseAmount)
            {
                SendError(player, "Nedostatok penazi na ucte");
                return;
            }

            HelperThread.Execute(() =>
            {
                uint amount = baseAmount;
                rp.CreditCardMoney -= baseAmount;

                while (amount > 0)
                {
                    foreach (var note in Currency.Money)
                    {
                        if (amount >= note.Value)
                        {
                            UnturnedPlayer.FromPlayer(player).GiveItem(note.Key, 1);
                            amount -= note.Value;
                            break;
                        }
                    }
                }

                SendError(player, $"<color=#58CD7B>Vybranych {Currency.FormatMoney(baseAmount.ToString())}</color>");
            });
        }

        private static void deposit(Player player)
        {
            var session = sessions[player.channel.owner.playerID.steamID];
            var rp = RealPlayer.From(player);

            if (session == null) return;

            if (session.Data[1] != "all" && !session.depositAllCycle)
            {
                if (!uint.TryParse(session.Data[4], out uint noteValue))
                {
                    SendError(player, "Nespravna bankovka");
                    return;
                }

                if (!uint.TryParse(session.Data[1], out uint count))
                {
                    SendError(player, "Nespravne mnozstvo");
                    return;
                }

                ushort noteId = 0;
                // noteValue

                foreach (var note in Currency.Money)
                {
                    if (note.Value == noteValue)
                    {
                        noteId = note.Key;
                        break;
                    }
                }

                if (noteId == 0)
                {
                    SendError(player, "Nespravna bankovka");
                    return;
                }

                HelperThread.Execute(() =>
                {
                    uint amount = 0;
                    bool finish = false;

                    foreach (var item in player.inventory.items)
                    {
                        if (finish) break;
                        if (item == null) continue;
                        for (byte w = 0; w < item.width; w++)
                        {
                            for (byte h = 0; h < item.height; h++)
                            {
                                try
                                {
                                    byte index = item.getIndex(w, h);

                                    if (index == 255) continue;

                                    if (item.getItem(index).item.id == noteId)
                                    {
                                        if (!finish)
                                        {
                                            amount += noteValue;
                                            item.removeItem(index);

                                            if (count != 0 && amount == noteValue * count) finish = true;
                                        }
                                    }
                                }
                                catch { }
                            }
                        }
                    }

                    if (amount == 0)
                    {
                        SendError(player, "Nedostatok bankoviek");
                    }
                    else
                    {
                        SendError(player, $"<color=#58CD7B>Vlozil si {Currency.FormatMoney(amount.ToString())}</color>");
                        rp.CreditCardMoney += amount;
                    }
                });
            }
            else // ALL
            {
                HelperThread.Execute(() =>
                {
                    uint amount = 0;

                    foreach(var note in Currency.Money)
                    {
                        foreach (var item in player.inventory.items)
                        {
                            if (item == null) continue;

                            for (byte w = 0; w < item.width; w++)
                            {
                                for (byte h = 0; h < item.height; h++)
                                {
                                    try
                                    {
                                        byte index = item.getIndex(w, h);

                                        if (index == 255) continue;

                                        if (item.getItem(index).item.id == note.Key)
                                        {
                                            amount += note.Value;
                                            item.removeItem(index);
                                        }
                                    }
                                    catch { }
                                }
                            }
                        }
                    }

                    if (amount == 0)
                    {
                        SendError(player, "Nemas ziadne peniaze");
                    }
                    else
                    {
                        SendError(player, $"<color=#58CD7B>Vlozil si {Currency.FormatMoney(amount.ToString())}</color>");
                        rp.CreditCardMoney += amount;
                        session.depositAllCycle = true;
                    }
                });
            }
        }

        private static void transfare(Player player)
        {

        }


        public static void SendError(Player player, string text)
        {
            EffectManager.sendUIEffectVisibility(keyATM, player.channel.GetOwnerTransportConnection(), true, "bank_error", true);
            EffectManager.sendUIEffectText(keyATM, player.channel.GetOwnerTransportConnection(), true, "bank_error_text", text);
        }

        public static void ClearError(Player player)
        {
            EffectManager.sendUIEffectText(keyATM, player.channel.GetOwnerTransportConnection(), true, "bank_error_text", "");
            EffectManager.sendUIEffectVisibility(keyATM, player.channel.GetOwnerTransportConnection(), true, "bank_error", false);
        }

        private static string getCatName(ATMCathegory cathegory) => $"bank_nav_{cathegory.ToString().ToLower()}";

        private static string getCatSlovakName(ATMCathegory cathegory)
        {
            switch (cathegory)
            {
                case ATMCathegory.Withdraw:
                    return "Vyber";

                case ATMCathegory.Deposit:
                    return "Vklad";

                case ATMCathegory.Transfare:
                    return "Prenos";
            }

            return "";
        }
    }
}
