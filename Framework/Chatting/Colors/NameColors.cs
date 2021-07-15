using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealLifeFramework.Chatting
{
    public static class ChatColors
    {
        public static readonly Dictionary<string, NameColor> NameColors = new Dictionary<string, NameColor>()
        {
            { "old_discord", new NameColor("#7289DA", 255) }, // Exclusive for veterans

            { "default", new NameColor("#FFFFFF", 0) },
            { "lavender", new NameColor("#D8B4E2", 0) },
            { "pink_perl", new NameColor("#C989B8", 0) },
            { "wisteria", new NameColor("#BC96E6", 0) },
            { "turquoise", new NameColor("#4CEBD6", 0) },
            { "isabelline", new NameColor("#F5EEEB", 0) },
            { "canary", new NameColor("#F8F991", 0) },
            { "indepedence", new NameColor("#444B6E", 0) },
            { "xanadu", new NameColor("#708B75", 0) },
            { "olivine", new NameColor("#9AB87A", 0) },
            { "snow", new NameColor("#EBFBFF", 0) },
            { "light_gray", new NameColor("#CED3DC", 0) },
            { "pale_cerulean", new NameColor("#90C2E7", 0) },
            { "android_green", new NameColor("#A7C957", 0) },
            { "eggshell", new NameColor("#F2E8CF", 0) },
            { "black_coral", new NameColor("#595F72", 0) },
            { "cafe_au_lait", new NameColor("#A38560", 0) },
            { "corn", new NameColor("#F2E86D", 0) },
            { "yzomandias_zuby", new NameColor("#A78F7B", 0) },
            { "satin_sheen_gold", new NameColor("#C6A15B", 0) },
            { "tea_green", new NameColor("#D3DFB8", 0) },
            { "light_steel_blue", new NameColor("#BCD3F2", 0) },
            { "little_boy_blue", new NameColor("#80A4ED", 0) },
            { "royal_purple", new NameColor("#7D5BA6", 0) },
            { "powder_blue", new NameColor("#ADE1E5", 0) },
            { "mellow_apricot", new NameColor("#FFBF69", 0) },
            { "light_cyan", new NameColor("#CBF3F0", 0) },
            { "rajah", new NameColor("#F2A65A", 0) },
            { "wealth_sea", new NameColor("#08BDBD", 0) },
            { "doctor_paradise", new NameColor("#f55f6f", 0) },

            { "untuned_zombie", new NameColor("#13AD16", 2) },
            { "dark_chef", new NameColor("#5E5E5E", 2) },
            { "diamond", new NameColor("#AOE1F5", 2) },

            { "deep_chestnut", new NameColor("#F05659", 3) },
            { "violet_wheel", new NameColor("#751499", 3) },
            { "dollar_prince", new NameColor("#13AD16", 3) },
            { "hot_pink", new NameColor("#FF007F", 3) },
        };
    }
}
