using RealLifeFramework.RealPlayers;
using RealLifeFramework.Threadding;
using RealLifeFramework.UserInterface;
using SDG.Unturned;
using Steamworks;
using System.Collections.Generic;

namespace RealLifeFramework.Skills
{
    [EventHandler]
    public class SkillDisplay : IEventComponent
    {
        private static Dictionary<CSteamID, byte> pageIndex;
        private const ushort idUI = 41793;
        private const short keyUI = 8162;

        public void HookEvents()
        {
            pageIndex = new Dictionary<CSteamID, byte>();

            EffectManager.onEffectButtonClicked += onButtonClicked;
            Provider.onEnemyDisconnected += (player) => pageIndex.Remove(player.playerID.steamID);
        }

        public static void SendOpenUI(RealPlayer player)
        {
            if (!pageIndex.ContainsKey(player.CSteamID))
            {
                ThreadHelper.Execute(() =>
                {
                    player.Player.setPluginWidgetFlag(EPluginWidgetFlags.ForceBlur, true);
                    player.Player.setPluginWidgetFlag(EPluginWidgetFlags.Modal, true);

                    EffectManager.sendUIEffect(idUI, keyUI, player.TransportConnection, true);

                    EffectManager.sendUIEffectText(keyUI, player.TransportConnection, true, "skills_txt_edupoints", player.SkillUser.EducationPoints.ToString());
                    EffectManager.sendUIEffectText(keyUI, player.TransportConnection, true, "skills_txt_lvl", player.Level.ToString());
                    EffectManager.sendUIEffectText(keyUI, player.TransportConnection, true, "skills_txt_exp", $"{HUD.FormatBigNums(player.Exp)}/{HUD.FormatBigNums(player.MaxExp)}");
                    SendPage(player, 0);
                });
            }
        }

        public static void SendPage(RealPlayer player, byte index)
        {
            if (pageIndex.ContainsKey(player.CSteamID))
            {
                if (index == pageIndex[player.CSteamID]) return;

                switch (index)
                {
                    case 0:
                        EffectManager.sendUIEffectVisibility(keyUI, player.TransportConnection, true, "edu_head", false);

                        EffectManager.sendUIEffectText(keyUI, player.TransportConnection, true, "skills_h1", "Zrucnosti");
                        EffectManager.sendUIEffectVisibility(keyUI, player.TransportConnection, true, "skills_head", true);
                        for (int i = 0; i < player.SkillUser.Skills.Count; i++)
                        {
                            loadSkill(player, (byte)i);
                        }
                        pageIndex[player.CSteamID] = 0;
                        break;
                    case 1:
                        EffectManager.sendUIEffectVisibility(keyUI, player.TransportConnection, true, "skills_head", false);

                        EffectManager.sendUIEffectText(keyUI, player.TransportConnection, true, "skills_h1", "Vylepsenia");
                        EffectManager.sendUIEffectVisibility(keyUI, player.TransportConnection, true, "edu_head", true);
                        for (int i = 0; i < player.SkillUser.Educations.Count; i++)
                        {
                            loadEdu(player, (byte)i);
                        }
                        pageIndex[player.CSteamID] = 1;
                        break;
                }
            }
            else
            {
                pageIndex.Add(player.CSteamID, 0);

                EffectManager.sendUIEffectText(keyUI, player.TransportConnection, true, "skills_h1", "Zrucnosti");
                EffectManager.sendUIEffectVisibility(keyUI, player.TransportConnection, true, "skills_head", true);
                for (int i = 0; i < player.SkillUser.Skills.Count; i++)
                {
                    loadSkill(player, (byte)i);
                }
            }
        }

        private static void loadSkill(RealPlayer player, byte index)
        {
            var skill = player.SkillUser.Skills[index];

            EffectManager.sendUIEffectVisibility(keyUI, player.TransportConnection, true, $"skill[{index}]", true);

            EffectManager.sendUIEffectText(keyUI, player.TransportConnection, true, $"skill[{index}].name", $"<color={skill.Color}>{skill.Name}</color>");
            EffectManager.sendUIEffectText(keyUI, player.TransportConnection, true, $"skill[{index}].lvlhead", $"<color={skill.Color}>LVL</color>");
            EffectManager.sendUIEffectText(keyUI, player.TransportConnection, true, $"skill[{index}].lvl", (skill.Level == skill.MaxLevel) ? "MAX" : $"{skill.Level}");
            EffectManager.sendUIEffectText(keyUI, player.TransportConnection, true, $"skill[{index}].exphead", $"<color={skill.Color}>Exp</color>");
            EffectManager.sendUIEffectText(keyUI, player.TransportConnection, true, $"skill[{index}].exp", $"{HUD.FormatBigNums(skill.Exp)}/{HUD.FormatBigNums(skill.GetExpToNextLevel())}");
        }

        private static void loadEdu(RealPlayer player, byte index)
        {
            var edu = player.SkillUser.Educations[index];

            EffectManager.sendUIEffectVisibility(keyUI, player.TransportConnection, true, $"edu[{index}]", true);

            EffectManager.sendUIEffectText(keyUI, player.TransportConnection, true, $"edu[{index}].name", $"<color={edu.Color}>{edu.Name}</color>");
            EffectManager.sendUIEffectText(keyUI, player.TransportConnection, true, $"edu[{index}].lvlhead", $"<color={edu.Color}>LVL</color>");
            EffectManager.sendUIEffectText(keyUI, player.TransportConnection, true, $"edu[{index}].lvl", (edu.Level == edu.MaxLevel)? "MAX" :$"{edu.Level}");
        }

        private static void onButtonClicked(Player player, string buttonName)
        {
            var rp = RealPlayer.From(player);

            switch (buttonName)
            {
                case "skills_skills":
                    SendPage(rp, 0);
                    break;

                case "skills_educations":
                    SendPage(rp, 1);
                    break;

                case "skills_exit":
                    rp.Player.setPluginWidgetFlag(EPluginWidgetFlags.ForceBlur, false);
                    rp.Player.setPluginWidgetFlag(EPluginWidgetFlags.Modal, false);
                    EffectManager.askEffectClearByID(idUI, player.channel.GetOwnerTransportConnection());
                    if (pageIndex.ContainsKey(rp.CSteamID)) pageIndex.Remove(rp.CSteamID);
                    break;
            }

            if (buttonName.StartsWith("edu[") && buttonName.Contains("upgrade"))
            {
                if(byte.TryParse(buttonName[4].ToString(), out byte index))
                {
                    rp.SkillUser.UpgradeEducation(index);
                    loadEdu(rp, index);
                    EffectManager.sendUIEffectText(keyUI, rp.TransportConnection, true, "skills_txt_edupoints", rp.SkillUser.EducationPoints.ToString());
                }
            }
        }
    }
}
