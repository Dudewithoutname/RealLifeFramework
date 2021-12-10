using System.Collections.Generic;
using System.EnterpriseServices.Internal;
using RealLifeFramework.RealPlayers;
using UnityEngine;

namespace RealLifeFramework.Pointmap
{
    public class PointmapUser
    {
        public RealPlayer Player;
        public Pointmap Map;
        public byte Sequence;
        public float Time;
        
        public PointmapUser(Pointmap map)
        {
            Map = map;
        }
    }
}