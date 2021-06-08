using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealLifeFramework.UserInterface
{
    public class Widget
    {
        public string Image { get; set; }
        public int Index { get; set; }
        public EWidgetType Type { get; set; }

        public Widget(int index, string image, EWidgetType type)
        {
            Index = index;
            Image = image;
            Type = type;
        }
    }
}
