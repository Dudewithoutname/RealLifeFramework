using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealLifeFramework.Chatting
{
    public struct NameColor
    {
        public byte Level;
        public string Color;
        
        public NameColor(string color, byte level)
        {
            Level = level;
            Color = color;
        }
    }
}
