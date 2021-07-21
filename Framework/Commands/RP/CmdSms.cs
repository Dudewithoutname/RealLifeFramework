using RealLifeFramework.Ranks;
using RealLifeFramework.RealPlayers;
using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections.Generic;
using UnityEngine;

namespace RealLifeFramework.Commands
{
    public class CmdSms : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "sms";

        public string Help => "sms";

        public string Syntax => "/sms";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string> { RankManager.PlayerPermission };

        public void Execute(IRocketPlayer caller, string[] args)
        {
            var player = RealPlayer.From(((UnturnedPlayer)caller).CSteamID);

            if (args.Length < 2) return;

            RealPlayer target = null;

            try
            {
                target = RealPlayer.From(PlayerTool.getPlayer(args[0]));
            }
            catch 
            {
                ChatManager.serverSendMessage($"<color=#2196F3>SMS | <color=red>Prijmatel nebol najdeny</color></color>", Color.white, null, player.Player.channel.owner, EChatMode.SAY, "https://i.ibb.co/g6s9sYj/sms.png", true);
            }

            if ((object)target != null) 
            {
                var message = args;
                message[0] = "";

                var txt = string.Join(" ", message);
                if (txt.Length < 2) return;
                if (txt.Contains("<")) txt.Replace("<", "(");


                EffectManager.sendUIEffect(5071, 1751, player.TransportConnection, false);
                ChatManager.serverSendMessage($"<color=#2196F3><b>SMS</b> | pre {target.Name} : </color><color=#ffffff>{txt}</color>", Color.white, null, player.Player.channel.owner, EChatMode.SAY, "https://i.ibb.co/g6s9sYj/sms.png", true);
                
                ChatManager.serverSendMessage($"<color=#2196F3><b>SMS</b> | od {player.Name} : </color><color=#ffffff>{txt}</color>", Color.white, null, target.Player.channel.owner, EChatMode.SAY, "https://i.ibb.co/g6s9sYj/sms.png", true);
                EffectManager.sendUIEffect(5072, 1752, target.TransportConnection, false);

            }
            else
            {
                ChatManager.serverSendMessage($"<color=#2196F3>SMS | <color=red>Prijmatel nebol najdeny</color></color>", Color.white, null, player.Player.channel.owner, EChatMode.SAY, "https://i.ibb.co/g6s9sYj/sms.png", true);
            }
        }
    }
}
