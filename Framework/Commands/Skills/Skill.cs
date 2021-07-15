using RealLifeFramework.Ranks;
using RealLifeFramework.RealPlayers;
using RealLifeFramework.Skills;
using Rocket.API;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealLifeFramework.Commands
{
    public class Skill : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "skill";

        public string Help => "skill";
                                               // typ = skill typ = edu
        public string Syntax => "/skill (hrac) (typ) (id) (val)";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string> { RankManager.PlayerPermission };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            // TODO ZAJTRA DOKONCI KOKOTKO
            var rp = RealPlayer.From(caller);
            if (command.Length < 4)
            {
                ChatManager.say(rp.CSteamID, $"Zly syntax {Syntax}!", Palette.COLOR_R, EChatMode.SAY, false);
                return;
            }

            if (rp.RankUser.Admin != null)
            {
                if (rp.RankUser.Admin.Value.Level >= 2)
                {
                    if((object)PlayerTool.getPlayer(command[0]) != null)
                    {
                        if (command[1] == "skill")
                        {

                        }
                        else if(command[1] == "edu" | command[1] == "education")
                        {
                            
                        }
                        else
                        {
                            ChatManager.say(rp.CSteamID, "Typ moze byt iba skill alebo education!", Palette.COLOR_R, EChatMode.SAY, false);
                        }
                    }
                    else
                    {
                        ChatManager.say(rp.CSteamID, "Hrac nebol najdeny!", Palette.COLOR_R, EChatMode.SAY, false);
                    }
                }
                else
                {
                    ChatManager.say(rp.CSteamID, "Nemas dostatocny admin level!", Palette.COLOR_R, EChatMode.SAY, false);
                }
            }
            else
            {
                ChatManager.say(rp.CSteamID, "Nemas permissie!", Palette.COLOR_R, EChatMode.SAY, false);
            }
        }
    }
}
