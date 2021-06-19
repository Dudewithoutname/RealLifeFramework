using Newtonsoft.Json;
using RealLifeFramework.Data;
using RealLifeFramework.Data.Models;
using RealLifeFramework.RealPlayers;
using System.Collections.Generic;
using System.Linq;

namespace RealLifeFramework.Skills
{
    public class SkillUser
    {
        [JsonIgnore]
        public RealPlayer RealPlayer { get; set; }
        public ushort EducationPoints { get; set; }
        // Note: everything should start from level 0

        public List<ISkill> Skills { get; set; }
        public List<IEducation> Educations { get; set; }

        public void AddEducationPoints(ushort amount)
        {
            EducationPoints += amount;
            TPlayerSkills.UpdateEducationPoints(RealPlayer.CSteamID, EducationPoints);
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
                TPlayerSkills.UpdateSkill(RealPlayer.CSteamID, id, skill.Level, skill.Exp);

                if(id != Agitily.Id)
                    RealPlayer.AddExp(2);

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

        public SkillUser(RealPlayer player, SkillUserData data)
        {
            RealPlayer = player;
            EducationPoints = data.EducationPoints;

            Skills = new List<ISkill>()
            {
                new Endurance(RealPlayer, data.Skills),
                new Farming(RealPlayer,   ),
                new Fishing(RealPlayer,   ),
                new Agitily(RealPlayer,   ),
                new Dexterity(RealPlayer, ),
            };

            Educations = new List<IEducation>()
            {
                new Engineering(RealPlayer, ),
                new Culinary(RealPlayer,    ),
                new Crafting(RealPlayer,    ),
                new Medicine(RealPlayer,    ),
                new Defense(RealPlayer,     ),
            };
        }

    }
}
