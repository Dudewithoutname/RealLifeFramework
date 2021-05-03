﻿using RealLifeFramework.Players;

namespace RealLifeFramework.Skills
{
    public interface IISkill
    {
        RealPlayer Player { get; set; }
        byte Level { get; set; }
        uint Exp { get; set; }


        string Name { get; }
        byte MaxLevel { get; }

        void Upgrade();
        void AddExp(uint exp); 
        uint GetExpToNextLevel();
    }
}