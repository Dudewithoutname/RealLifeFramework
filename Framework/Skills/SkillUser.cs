using RealLifeFramework.Players;
using System.Collections.Generic;

namespace RealLifeFramework.Skills
{
    public class SkillUser
    {
        public RealPlayer RealPlayer { get; set; }
        public ushort EducationPoints { get; set; }
        // Note: everything should start from level 0

        public List<ISkill> Skills { get; set; }
        public List<IEducation> Educations { get; set; }

        public void AddEducationPoints(ushort amount)
        {
            EducationPoints += amount;
            PlayerSkills.UpdateEducationPoints(RealPlayer.CSteamID, EducationPoints);
        }

        public void UpgradeEducation(IEducation education)
        {
            if(EducationPoints >= 1)
            {
                if (education.Level <= education.MaxLevel)
                {
                    EducationPoints--;
                    education.Upgrade(); // forced
                    // TODO: Send Message About Upgrade
                }
                else
                {
                    // TOOD: Send message about max level
                }
            }
            else
            {
                // TOOD: Send Message about low edu points
            }
        }

        public void AddExp(int id, uint amount)
        {
            var skill = Skills[id];

            if(skill != null)
            {
                skill.AddExp(amount);
                PlayerSkills.UpdateSkill(RealPlayer.CSteamID, id, skill.Level, skill.Exp);
                RealPlayer.AddExp(5);

            }
        }

        public void ForceLevelUp(int id)
        {
            var skill = Skills[id];

            if (skill != null && skill.Level >= skill.MaxLevel)
            {
                skill.Upgrade();
                SkillManager.SendLevelUp(RealPlayer, id);
            }

        }

        // New
        public SkillUser(RealPlayer player)
        {
            RealPlayer = player;
            EducationPoints = 0;

            Skills = new List<ISkill>()
            {
                new Endurance(RealPlayer, 0, 0),
                new Farming(RealPlayer, 0, 0),
                new Fishing(RealPlayer, 0, 0),
                new Agitily(RealPlayer, 0, 0),
                new Dexterity(RealPlayer, 0, 0),
            };

            Educations = new List<IEducation>()
            {
                new Engineering(RealPlayer, 0),
                new Culinary(RealPlayer, 0),
                new Crafting(RealPlayer, 0),
                new Medicine(RealPlayer, 0),
                new Defense(RealPlayer, 0),
            };
        }

        public SkillUser(RealPlayer player, DBSkillsResult skillResult)
        {
            RealPlayer = player;
            EducationPoints = skillResult.EducationPoints;

            Skills = new List<ISkill>()
            {
                skillResult.Skills[0],
                skillResult.Skills[1],
                skillResult.Skills[2],
                skillResult.Skills[3],
                skillResult.Skills[4],
            };

            Educations = new List<IEducation>()
            {
                skillResult.Educations[0],
                skillResult.Educations[1],
                skillResult.Educations[2],
                skillResult.Educations[3],
                skillResult.Educations[4],
            };
        }
    }
}
