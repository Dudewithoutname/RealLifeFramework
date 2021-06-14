using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealLifeFramework.UserInterface
{
    public static class WidgetInfo
    {
        public static readonly string[] Icons = new string[]
        {
            "https://i.ibb.co/6NTQnRd/broken-Bone.png",
            "https://i.ibb.co/51k6Cqt/blood.png",
            "https://i.ibb.co/jwVTsZ4/toxic.png",
            "https://i.ibb.co/zZYfRLP/xp.png",
        };

        public static string GetImage(EWidgetType wtype) => Icons[(int)wtype];
    }

    public enum EWidgetType : int
    {
        BrokenBone = 0,
        Bleeding,
        LowVirus,
        DoubleExp,
    }
}
