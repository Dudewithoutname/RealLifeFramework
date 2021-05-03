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
            1,
            // TOOD RANDOM SHIRT IDS
        };

        public static List<ushort> FemaleShirts = new List<ushort>()
        {
            1,
            // TOOD RANDOM SHIRT IDS
        };

        public static List<ushort> Pantsu = new List<ushort>()
        {
            1,
            //  TODO RANDOM PANTS
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
