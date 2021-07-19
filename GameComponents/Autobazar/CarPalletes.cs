using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealLifeFramework.Autobazar
{
    public static class CarPalletes
    {
        public static Dictionary<string, string> Hex = new Dictionary<string, string>()
        {
            { "black", "#404040" },
            { "blue", "#364dff" },
            { "green", "#27d600" },
            { "orange", "#ff7b00" },
            { "purple", "#b927f2" },
            { "red", "#e31b1b" },
            { "white", "#adadad" },
            { "yellow", "#eddd00" },
        };

        // Pallete none is always default
        public static Dictionary<string, int> JPepe = new Dictionary<string, int>()
        {
            { "black", 0 },
            { "blue", 1 },
            { "green", 2 },
            { "orange", 3 },
            { "purple", 4 },
            { "red", 5 },
            { "white", 6 },
            { "yellow", 7 },
        };

        public static Dictionary<string, int> Vanilla = new Dictionary<string, int>()
        {
            { "black", 0 },
            { "blue", 1 },
            { "green", 2 },
            { "orange", 3 },
            { "purple", 4 },
            { "red", 5 },
            { "white", 6 },
            { "yellow", 7 },
        };

        public static Dictionary<string, int> GetPallete(string palleteId)
        {
            switch (palleteId)
            {
                case "jpepe":
                    return JPepe;

                case "vanilla":
                    return Vanilla;
            }


            return null;
        }
    }
}
