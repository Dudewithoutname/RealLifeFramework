using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealLifeFramework.UserInterface
{
    public class Widget
    {
        public static ushort baseId = 49130;
        public string Image { get; set; }
        public ushort Id => (ushort)(baseId + Index);
        public short Key => (short)(1210 + Index);
        public int Index { get; set; }
        public EWidgetType Type { get; set; }

        public Widget(int activeCount, string image, EWidgetType type)
        {
            Image = image;
            Index = activeCount;
            Type = type;
        }
    }
}
