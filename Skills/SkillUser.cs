using RealLifeFramework.Players;
using System.Collections.Generic;

namespace RealLifeFramework.Skills
{
    public class SkillUser
    {
        public RealPlayer RealPlayer { get; set; }
        public ushort EducationPoints { get; set; }
        // Note: everything should start from level 0

        public IEducation Engineering { get; set; }
        public IEducation Culinary { get; set; }
        public IEducation Crafting { get; set; }
        public IEducation Medicine { get; set; }
        public IEducation Defense { get; set; }

        public ISkill Endurance { get; set; }
        public ISkill Farming { get; set; }
        public ISkill Fishing { get; set; }
        public ISkill Agility { get; set; }

        public List<ISkill> GetSkillById { get; set; }
        public List<IEducation> GetEducationById { get; set; }

        public void AddEducationPoints(ushort amount)
        {
            EducationPoints += amount;
            RealLife.Database.UpdateEducationPoints(RealPlayer.CSteamID, EducationPoints);
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
            var skill = GetSkillById[id];

            if(skill != null)
            {
                skill.AddExp(amount);
                RealLife.Database.UpdateSkill(RealPlayer.CSteamID, id, skill.Level, skill.Exp);
                Logger.Log($"{skill.Name} , {skill.Level}, {skill.Exp}");
            }
        }

        // New
        public SkillUser(RealPlayer player)
        {
            RealPlayer = player;
            EducationPoints = 0;
            Endurance = new Endurance(RealPlayer, 0, 0);
            Farming = new Farming(RealPlayer, 0, 0);
            Fishing = new Fishing(RealPlayer, 0, 0);
            Agility = new Agitily(RealPlayer, 0, 0);

            Engineering = new Engineering(RealPlayer, 0);
            Culinary = new Culinary(RealPlayer, 0);
            Crafting = new Crafting(RealPlayer, 0);
            Medicine = new Medicine(RealPlayer, 0);
            Defense = new Defense(RealPlayer, 0);

            GetSkillById = new List<ISkill>()
            {
                Endurance,
                Farming,
                Fishing,
                Agility,
            };

            GetEducationById = new List<IEducation>()
            {
                Engineering,
                Culinary,
                Crafting,
                Medicine,
                Defense,
            };
        }

        public SkillUser(RealPlayer player, DBSkillsResult skillResult)
        {
            RealPlayer = player;
            EducationPoints = skillResult.EducationPoints;

            Endurance = skillResult.Skills[0];
            Farming = skillResult.Skills[1];
            Fishing = skillResult.Skills[2];
            Agility = skillResult.Skills[3];

            Engineering = skillResult.Educations[0];
            Culinary = skillResult.Educations[1];
            Crafting = skillResult.Educations[2];
            Medicine = skillResult.Educations[3];
            Defense = skillResult.Educations[4];

            GetSkillById = new List<ISkill>()
            {
                Endurance,
                Farming,
                Fishing,
                Agility,
            };

            GetEducationById = new List<IEducation>()
            {
                Engineering,
                Culinary,
                Crafting,
                Medicine,
                Defense,
            };
        }
    }
}
