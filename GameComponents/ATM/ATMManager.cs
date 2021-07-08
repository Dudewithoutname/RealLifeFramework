/*using RealLifeFramework.RealPlayers;
using RealLifeFramework.SecondThread;
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
        private const ushort idATM = 42568;
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
            if (Physics.Raycast(player.look.aim.position, player.look.aim.forward, out RaycastHit hit, 5f, RayMasks.BARRICADE_INTERACT | RayMasks.BARRICADE))
            {
                if (hit.transform != null && BarricadeManager.tryGetInfo(hit.transform,out byte x, out byte y, out ushort plant, out ushort index, out BarricadeRegion region))
                {
                    if (region.barricades[index].barricade.id == idATM)
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

            EffectManager.sendUIEffect(uiATM, keyATM, player.channel.GetOwnerTransportConnection(), true);
            EffectManager.sendUIEffectText(keyATM, player.channel.GetOwnerTransportConnection(), true, "bank_txt_money", Currency.FormatMoney(rp.CreditCardMoney.ToString()));
        }

        public static void OpenATMCathegory(Player player, ATMCathegory cathegory)
        {
            var session = sessions[player.channel.owner.playerID.steamID];

            if (session.CurrentCathegory == cathegory) return;

            EffectManager.sendUIEffectText(keyATM, player.channel.GetOwnerTransportConnection(), true, "bank_txt_money", (cathegory == ATMCathegory.Menu) ? "" : cathegory.ToString());

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

            SecondaryThread.Execute(() =>
            {
                uint amount = baseAmount;
                rp.CreditCardMoney -= baseAmount;

                while (amount > 0)
                {
                    foreach (KeyValuePair<ushort, uint> note in Currency.Money)
                    {
                        if (amount >= note.Value)
                        {
                            UnturnedPlayer.FromPlayer(player).GiveItem(note.Key, 1);
                            amount -= note.Value;
                            break;
                        }
                    }
                }

                SendError(player, $"<color=#58CD7B>Vybranych {Currency.FormatMoney(baseAmount.ToString())} $</color>");
            });
        }

        private static void deposit(Player player)
        {
            var session = sessions[player.channel.owner.playerID.steamID];
            var rp = RealPlayer.From(player);

            if (session == null) return;

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

            foreach (KeyValuePair<ushort, uint> note in Currency.Money)
            {
                if(note.Value == noteValue)
                {
                    noteId = note.Key;
                    break;
                }
            }

            if(noteId == 0)
            {
                SendError(player, "Nespravna bankovka");
                return;
            }


            var search = player.inventory.search(noteId, true, true);

            if (search != null)
            {
                if(search.Count < count)
                {
                    SendError(player, "Nedostatok bankoviek");
                    return;
                }

                search[0].deleteAmount(player, count);
                rp.CreditCardMoney += count;
            }
            else
            {
                SendError(player, "Nedostatok bankoviek");
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

        public static void ClearError(Player player, string text)
        {
            EffectManager.sendUIEffectText(keyATM, player.channel.GetOwnerTransportConnection(), true, "bank_error_text", "");
            EffectManager.sendUIEffectVisibility(keyATM, player.channel.GetOwnerTransportConnection(), true, "bank_error", false);
        }

        private static string getCatName(ATMCathegory cathegory) => $"bank_nav_{cathegory.ToString().ToLower()}";
    }
}
-*/