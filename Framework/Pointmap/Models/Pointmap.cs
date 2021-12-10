using System.Collections.Generic;
using UnityEngine;

namespace RealLifeFramework.Pointmap
{
    public class Pointmap
    {
        public List<Vector3> Positions;
        public ushort EffectId;
        public float Distance;
        public float Time; // tod.o (Time) if time <= 0 infinity time;
        public bool OnlyVehicle;
        
        public Pointmap(ushort effectId, float time, float distance = 5, bool onlyVehicle = false)
        {
            EffectId = effectId;
            Distance = distance;
            Time = time;
            OnlyVehicle = onlyVehicle;
        }
    }
}