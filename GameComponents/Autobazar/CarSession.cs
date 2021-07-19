using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealLifeFramework.Autobazar
{
    public class CarSession
    {
        public BuyableCar SelectedCar;
        public byte Cathegory; // 255 == menu
        public int Page;
        public int ColorOffset;
        public bool IsFinalPage;

        public int Offset => Page*7;

        public CarSession()
        {
            Cathegory = 255;
            SelectedCar = null;
            Page = 0;
            ColorOffset = 0;
            IsFinalPage = false;
        }
    }
}
