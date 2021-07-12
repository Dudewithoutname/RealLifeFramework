using System;
using SDG.Unturned;
using Rocket.API;
using System.Collections.Generic;
using RealLifeFramework.RealPlayers;
using RealLifeFramework.Skills;
using Rocket.Unturned.Player;
using RealLifeFramework.UserInterface;
using RealLifeFramework.Chatting;
using RealLifeFramework.Ranks;

namespace RealLifeFramework.Commands
{
    public class CmdRefresh : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "prefixrefresh";

        public string Help => "prefixrefresh";

        public string Syntax => "/prefixrefresh";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string> { RankManager.PlayerPermission };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (caller == null)
            {
                if (command.Length < 1)
                {
                    Logger.Log("Definuj hraca : /prefixrefresh (meno) !");
                    return;
                }

                var tempTar = PlayerTool.getPlayer(command[0]);

                if ((object)tempTar != null)
                {
                    var target = RealPlayer.From(tempTar);

                    if (target != null)
                    {
                        target.RankUser.Refresh();
                    }
                    else
                    {
                        Logger.Log("Hrac neni inicializovany");
                    }
                }
                else
                {
                    Logger.Log("Hrac nebol najdeny");
                }

                return;
            }

            var player = RealPlayer.From(((UnturnedPlayer)caller).CSteamID);
            
            if (player.RankUser.Admin != null | player.Player.channel.owner.isAdmin)
            {
                if (command.Length < 1)
                {
                    ChatManager.say(player.CSteamID, "Definuj hraca : /prefixrefresh (meno) !", Palette.COLOR_R, EChatMode.SAY, false);
                    return;
                }

                var tempTar = PlayerTool.getPlayer(command[0]);

                if ((object)tempTar != null)
                {
                    var target = RealPlayer.From(tempTar);

                    if(target != null)
                    {
                        target.RankUser.Refresh();
                    }
                    else
                    {
                        ChatManager.say(player.CSteamID, "Hrac neni inicializovany", Palette.COLOR_R, EChatMode.SAY, false);
                    }
                }
                else
                {
                    ChatManager.say(player.CSteamID, "Hrac nebol najdeny", Palette.COLOR_R, EChatMode.SAY, false);
                }
            }
            else
            {
                ChatManager.say(player.CSteamID, "Nemas permissie na prefixrefresh!", Palette.COLOR_R, EChatMode.SAY, false);
            }
        }
    }
}
