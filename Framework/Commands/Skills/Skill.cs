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
                        var target = RealPlayer.From(PlayerTool.getPlayer(command[0]));

                        if (!uint.TryParse(command[4], out uint amount))
                        {
                            ChatManager.say(rp.CSteamID, "Nespravna hodnota!", Palette.COLOR_R, EChatMode.SAY, false);
                            return;
                        }

                        if (command[1] == "skill")
                        {
                            if (int.TryParse(command[2], out int id) && target.SkillUser.Skills[id] != null)
                            {

                                switch (command[3].ToLower())
                                {
                                    case "exp":
                                        if (target.SkillUser.Skills[id].Level == target.SkillUser.Skills[id].MaxLevel) return;

                                        target.SkillUser.AddExp(id, amount);
                                        ChatManager.say(rp.CSteamID, $"Uspesne si pridal {target.Name} {amount} exp do zrucnosti {target.SkillUser.Skills[id].Name}!", Palette.COLOR_G, EChatMode.SAY, false);
                                        ChatManager.say(target.CSteamID, $"{rp.Name} ti dal {amount} exp do zrucnosti {target.SkillUser.Skills[id].Name}!", Palette.COLOR_G, EChatMode.SAY, false);
                                        break;

                                    case "lvl":
                                        if (target.SkillUser.Skills[id].Level == target.SkillUser.Skills[id].MaxLevel) return;

                                        for(int i = 0; i < amount; i++)
                                        {
                                            if (target.SkillUser.Skills[id].Level == target.SkillUser.Skills[id].MaxLevel) break;
                                            target.SkillUser.ForceLevelUp(id);
                                        }
                                        ChatManager.say(rp.CSteamID, $"Uspesne si pridal {target.Name} {amount} levelov do zrucnosti {target.SkillUser.Skills[id].Name}!", Palette.COLOR_G, EChatMode.SAY, false);
                                        ChatManager.say(target.CSteamID, $"{rp.Name} ti dal {amount} levelov do zrucnosti {target.SkillUser.Skills[id].Name}!", Palette.COLOR_G, EChatMode.SAY, false);
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
                            if (int.TryParse(command[2], out int id) && target.SkillUser.Educations[id] != null)
                            {
                                switch (command[3].ToLower())
                                {
                                    case "lvl":
                                        if (target.SkillUser.Educations[id].Level == target.SkillUser.Educations[id].MaxLevel) return;

                                        for (int i = 0; i < amount; i++)
                                        {
                                            if (target.SkillUser.Educations[id].Level == target.SkillUser.Educations[id].MaxLevel) break;
                                            target.SkillUser.UpgradeEducation(id);
                                        }
                                        ChatManager.say(rp.CSteamID, $"Uspesne si pridal {target.Name} {amount} levelov do vylepsenia {target.SkillUser.Educations[id].Name}!", Palette.COLOR_G, EChatMode.SAY, false);
                                        ChatManager.say(target.CSteamID, $"{rp.Name} ti dal {amount} levelov do vylepsenia {target.SkillUser.Educations[id].Name}!", Palette.COLOR_G, EChatMode.SAY, false);

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
