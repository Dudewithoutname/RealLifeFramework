using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealLifeFramework.Items
{
    public static class StartClothes
    {
        public static List<ushort> MaleShirts = new List<ushort>()
        {
        // Start Unisex
            179,
            180,
            183,
            184,
        // END Unisex
            163,
            164,
            175,
            176,
            211,
        };

        public static List<ushort> FemaleShirts = new List<ushort>()
        {
        // Start Unisex
            179,
            180,
            183,
            184,
        // END Unisex
            171,
            172,
            167,
            168,
            158,
            159,
        };

        public static List<ushort> Pantsu = new List<ushort>()
        {
            2,
            548,
            209,
            212,
            213,
            214,
            226,
            227,
            228,
            229,
            /* Hawai Assets
            1454,
            1455,
            1456,
            1457,
            1458,
            1459,
            1460,
            1461,*/
        };

        public static ushort GetRandomShirt(string gender)
        {
            Random random = new Random();

            if(gender == "Male")
                return MaleShirts[random.Next(0, MaleShirts.Count)];
            else
                return FemaleShirts[random.Next(0, FemaleShirts.Count)]; 
        }

        public static ushort GetPantsu()
        {
            Random random = new Random();

            return Pantsu[random.Next(0, Pantsu.Count)];
        }

    }
}
