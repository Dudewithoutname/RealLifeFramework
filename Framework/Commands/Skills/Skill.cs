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
                                               // typ = skill typ = edu | typ2 = lvl exp
        public string Syntax => "/skill (hrac) (typ) (id) (typ2) (val)";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string> { RankManager.PlayerPermission };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            // TODO ZAJTRA DOKONCI KOKOTKO
            var rp = RealPlayer.From(caller);
            if (command.Length < 5)
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
                        if (!uint.TryParse(command[4], out uint amount))
                        {
                            ChatManager.say(rp.CSteamID, "Nespravna hodnota!", Palette.COLOR_R, EChatMode.SAY, false);
                            return;
                        }

                        if (command[1] == "skill")
                        {
                            if (int.TryParse(command[2], out int id) && rp.SkillUser.Skills[id] != null)
                            {

                                switch (command[3].ToLower())
                                {
                                    case "exp":

                                        rp.SkillUser.AddExp(id, amount);
                                        break;

                                    case "lvl":
                                        if (rp.SkillUser.Skills[id].Level == rp.SkillUser.Skills[id].MaxLevel) return;

                                        for(int i = 0; i < amount; i++)
                                        {
                                            if (rp.SkillUser.Skills[id].Level == rp.SkillUser.Skills[id].MaxLevel) break;
                                            rp.SkillUser.ForceLevelUp(id);
                                        }
                                        break;

                                    default:
                                        ChatManager.say(rp.CSteamID, "Typ2 moze byt iba exp a lvl pre skill!", Palette.COLOR_R, EChatMode.SAY, false);
                                        return;
                                }
                            }
                            else 
                            { 
                                ChatManager.say(rp.CSteamID, $"Nespravne ID moze iba (0-{rp.SkillUser.Skills.Count-1})!", Palette.COLOR_R, EChatMode.SAY, false);
                            }
                            return;
                        }
                        else if (command[1] == "edu" | command[1] == "education")
                        {
                            if (int.TryParse(command[2], out int id) && rp.SkillUser.Educations[id] != null)
                            {
                                switch (command[3].ToLower())
                                {
                                    case "lvl":
                                        if (rp.SkillUser.Educations[id].Level == rp.SkillUser.Educations[id].MaxLevel) return;

                                        for (int i = 0; i < amount; i++)
                                        {
                                            if (rp.SkillUser.Educations[id].Level == rp.SkillUser.Educations[id].MaxLevel) break;
                                            rp.SkillUser.UpgradeEducation(id);
                                        }
                                        break;

                                    default:
                                        ChatManager.say(rp.CSteamID, "Typ2 moze byt iba lvl pre education!", Palette.COLOR_R, EChatMode.SAY, false);
                                        return;
                                }
                            }
                            else 
                            { 
                                ChatManager.say(rp.CSteamID, $"Nespravne ID moze iba (0-{rp.SkillUser.Educations.Count-1})!", Palette.COLOR_R, EChatMode.SAY, false);
                            }
                            return;
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
