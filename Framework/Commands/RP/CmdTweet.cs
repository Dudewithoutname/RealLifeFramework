using RealLifeFramework.Ranks;
using RealLifeFramework.RealPlayers;
using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections.Generic;
using UnityEngine;

namespace RealLifeFramework.Commands
{
    public class CmdTweet : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "tweet";

        public string Help => "tweet";

        public string Syntax => "/tweet";

        public List<string> Aliases => new List<string>() { "twitter" };

        public List<string> Permissions => new List<string> { RankManager.PlayerPermission };

        public void Execute(IRocketPlayer caller, string[] args)
        {
            var txt = string.Join(" ", args);
            if (txt.Length < 2) return;
            if (txt.Contains("<")) txt.Replace("<", "(");

            var player = RealPlayer.From(((UnturnedPlayer)caller).CSteamID);

            ChatManager.serverSendMessage($"<color=#1DA1F2><b>Twitter</b> | {player.Name} : </color><color=#ffffff>{txt}</color>", Color.white, null, null, EChatMode.GLOBAL, "https://brexit.eu.sk/wp-content/uploads/2020/09/Twitter-Inc.-31.08.2020.png", true);
        }
    }
}
