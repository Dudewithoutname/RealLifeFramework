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
using Rocket.Core.Utils;

namespace RealLifeFramework.ATM
{
    [EventHandler]
    public class ATMManager : IEventComponent
    {
        private static Dictionary<CSteamID, ATMSession> sessions;
        private const ushort idATM = 37101;
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
            if (!Physics.Raycast(player.look.aim.position, player.look.aim.forward, out RaycastHit hit, 2.5f,
                RayMasks.BARRICADE | RayMasks.BARRICADE_INTERACT)) return;
            
            var drop = BarricadeManager.FindBarricadeByRootTransform(hit.transform);

            if (drop == null) return;
                
            var barricade = drop.asset;

            if (barricade.id == idATM)
            {
                openAtm(player);
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

                    case "bank_trans_btn_confirm":
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

                    case "bank_trans_inp_money":
                        session.Data[2] = text;
                        break;

                    case "bank_trans_inp_user":
                        session.Data[3] = text;
                        break;

                }
            }
        }

        #endregion

        private static void openAtm(Player player)
        {
            var rp = RealPlayer.From(player);
            
            if (sessions.ContainsKey(rp.CSteamID)) sessions.Remove(rp.CSteamID);

            sessions.Add(player.channel.owner.playerID.steamID, new ATMSession(player));
            
            player.setPluginWidgetFlag(EPluginWidgetFlags.ForceBlur, true);
            player.setPluginWidgetFlag(EPluginWidgetFlags.Modal, true);

            EffectManager.sendUIEffect(uiATM, keyATM, player.channel.GetOwnerTransportConnection(), true);
            EffectManager.sendUIEffectText(keyATM, player.channel.GetOwnerTransportConnection(), true, "bank_txt_money", Currency.FormatMoney(rp.CreditCardMoney.ToString()));
            
            // maybe in later update
            EffectManager.sendUIEffectVisibility(keyATM, player.channel.GetOwnerTransportConnection(), true, "bank_btn_crypto", false);
            EffectManager.sendUIEffectVisibility(keyATM, player.channel.GetOwnerTransportConnection(), true, "bank_btn_stocks", false);
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

        private static void OpenATMCathegory(Player player, ATMCathegory cathegory)
        {
            var session = sessions[player.channel.owner.playerID.steamID];

            if (session.CurrentCathegory == cathegory) return;
            if (session.DepositAllCycle) session.DepositAllCycle = false;

            EffectManager.sendUIEffectText(keyATM, player.channel.GetOwnerTransportConnection(), true, "bank_header", (cathegory == ATMCathegory.Menu) ? "" : getCatSlovakName(cathegory));

            EffectManager.sendUIEffectVisibility(keyATM, player.channel.GetOwnerTransportConnection(), true, getCatName(session.CurrentCathegory), false);
            EffectManager.sendUIEffectVisibility(keyATM, player.channel.GetOwnerTransportConnection(), true, getCatName(cathegory), true);

            session.CurrentCathegory = cathegory;
        }

        private static void TryConfirm(Player player, ATMCathegory cathegory)
        {
            switch (cathegory)
            {
                case ATMCathegory.Withdraw:
                    Withdraw(player);
                    break;

                case ATMCathegory.Deposit:
                    Deposit(player);
                    break;

                case ATMCathegory.Transfare:
                    transfare(player);
                    break;
            }
        }

        private static void Withdraw(Player player)
        {
            var session = sessions[player.channel.owner.playerID.steamID];
            var rp = RealPlayer.From(player);

            if (session == null || session.IsDoingWork) return;

            session.IsDoingWork = true;

            ThreadHelper.Execute(() =>
            {
                if (!uint.TryParse(session.Data[0], out uint baseAmount) || baseAmount == 0)
                {
                    TaskDispatcher.QueueOnMainThread( () => SendError(player, "Neplatna Hodnota"));
                    session.IsDoingWork = false;
                    return;
                }

                if (rp.CreditCardMoney < baseAmount)
                {
                    TaskDispatcher.QueueOnMainThread( () => SendError(player, "Nedostatok penazi na ucte"));
                    session.IsDoingWork = false;
                    return;
                }

                var amount = baseAmount;
                TaskDispatcher.QueueOnMainThread(() => rp.CreditCardMoney -= baseAmount);

                while (amount > 0)
                {
                    foreach (var note in Currency.Money)
                    {
                        if (amount < note.Value) continue;
                        
                        TaskDispatcher.QueueOnMainThread( () => UnturnedPlayer.FromPlayer(player).GiveItem(note.Key, 1));
                        amount -= note.Value;
                        break;
                    }
                }

                TaskDispatcher.QueueOnMainThread( () => SendError(player, $"<color=#58CD7B>Vybranych {Currency.FormatMoney(baseAmount.ToString())}</color>") );
                session.IsDoingWork = false;
            });
        }

        private static void Deposit(Player player)
        {
            var session = sessions[player.channel.owner.playerID.steamID];
            var rp = RealPlayer.From(player);

            if (session == null || session.IsDoingWork) return;

            session.IsDoingWork = true;

            ThreadHelper.Execute(() =>
            {
                if (!session.Data[1].Contains("vsetko") && !session.Data[1].Contains("all") && !session.Data[1].Contains("vsechno"))
                {
                    ushort noteId = 0;
                    // noteValue

                    if (!uint.TryParse(session.Data[4], out uint noteValue))
                    {
                        TaskDispatcher.QueueOnMainThread( () => SendError(player, "Nespravna bankovka") );
                        session.IsDoingWork = false;
                        return;
                    }

                    if (!uint.TryParse(session.Data[1], out uint count))
                    {
                        TaskDispatcher.QueueOnMainThread( () => SendError(player, "Nespravne mnozstvo") );
                        session.IsDoingWork = false;
                        return;
                    }

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
                        TaskDispatcher.QueueOnMainThread( () => SendError(player, "Nespravna bankovka") );
                        return;
                    }

                    uint amount = 0;
                    var finish = false;
                    var noteItems = new List<ItemCords>();
                                        
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
                                    var index = item.getIndex(w, h);
                                    var itemPage = item.page;

                                    if (index == 255) continue;

                                    if (item.getItem(index).item.id != noteId) continue;
                                    if (amount >= count) finish = true;

                                    amount++;

                                    noteItems.Add(new ItemCords(index, itemPage));
                                }
                                catch
                                {
                                    // ignored
                                }
                            }
                        }
                    }

                    if (noteItems.Count < count)
                    {
                        TaskDispatcher.QueueOnMainThread(() => SendError(player, "Nedostatok bankoviek"));
                        session.IsDoingWork = false;
                        return;
                    }

                    TaskDispatcher.QueueOnMainThread(() => 
                    {
                        foreach(var cord in noteItems)
                        {
                            rp.Player.inventory.removeItem(cord.Page, cord.Index);
                            amount += noteValue;
                        }
                    });

                    if (amount == 0)
                    {
                        TaskDispatcher.QueueOnMainThread(() => SendError(player, "Nedostatok bankoviek"));
                        session.IsDoingWork = false;
                    }
                    else
                    {
                        TaskDispatcher.QueueOnMainThread(() => SendError(player, $"<color=#58CD7B>Vlozil si {Currency.FormatMoney(amount.ToString())}</color>") );
                        TaskDispatcher.QueueOnMainThread(() => rp.CreditCardMoney += amount );
                        session.IsDoingWork = false;
                    }
                }
                else // ALL
                {
                    if (session.DepositAllCycle) return;
                    
                    uint amount = 0;

                    foreach (var note in Currency.Money)
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
                                        var index = item.getIndex(w, h);
                                        var itemPage = item.page;

                                        if (index == 255) continue;
                                        if (item.getItem(index).item.id != note.Key) continue;
                                        
                                        amount += note.Value;
                                        TaskDispatcher.QueueOnMainThread(() => rp.Player.inventory.removeItem(itemPage, index));
                                    }
                                    catch
                                    {
                                        // ignored
                                    }
                                }
                            }
                        }
                    }

                    if (amount == 0)
                    {
                        TaskDispatcher.QueueOnMainThread(() => SendError(player, "Nemas ziadne peniaze"));
                        session.IsDoingWork = false;
                    }
                    else
                    {
                        TaskDispatcher.QueueOnMainThread(() => SendError(player, $"<color=#58CD7B>Vlozil si {Currency.FormatMoney(amount.ToString())}</color>"));
                        TaskDispatcher.QueueOnMainThread(() => rp.CreditCardMoney += amount);
                        session.DepositAllCycle = true;
                        session.IsDoingWork = false;
                    }
                }
            });

        }

        private static void transfare(Player player)
        {
            var session = sessions[player.channel.owner.playerID.steamID];
            var rp = RealPlayer.From(player);

            if (session == null || session.IsDoingWork) return;

            session.IsDoingWork = true;

            ThreadHelper.Execute(() =>
            {
                if (!uint.TryParse(session.Data[2], out uint moneyToSend) || moneyToSend == 0)
                {
                    TaskDispatcher.QueueOnMainThread(() => SendError(player, "Nespravna hodnota"));
                    session.IsDoingWork = false;
                    return;
                }
                
                if (rp.CreditCardMoney < moneyToSend)
                {
                    TaskDispatcher.QueueOnMainThread(() => SendError(player, "Nedostatok penazi"));
                    session.IsDoingWork = false;
                    return;
                }

                RealPlayer target = null;

                foreach (var tr in RealLife.Instance.RealPlayers)
                {
                    if (tr.Value == rp) continue;
                    
                    if (tr.Value.Name.Contains(session.Data[3]) | tr.Value.CSteamID.ToString() == session.Data[3])
                    {
                        target = tr.Value;
                        break;
                    }
                }

                if (target == null)
                {
                    TaskDispatcher.QueueOnMainThread(() => SendError(player, "Osoba sa nenasla"));
                    session.IsDoingWork = false;
                    return;
                }

                TaskDispatcher.QueueOnMainThread( () =>
                {
                    rp.CreditCardMoney -= moneyToSend;
                    target.CreditCardMoney += moneyToSend;
                    SendError(player, $"<color=#58CD7B>Poslal si {Currency.FormatMoney(moneyToSend.ToString())} na ucet {target.Name}</color>");
                });
                session.IsDoingWork = false;
            });
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
