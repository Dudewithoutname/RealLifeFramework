using Newtonsoft.Json;
using RealLifeFramework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealLifeFramework.Autobazar
{
    public class BuyableCathegories : ISaveable
    {
        public BuyableCar[] Sports;
        public BuyableCar[] OffRoad;
        public BuyableCar[] Hatchbacks;
        public BuyableCar[] Sedans;

        public BuyableCar[] SUV;
        public BuyableCar[] Trucks;
        public BuyableCar[] Bikes;
        public BuyableCar[] Special;

        public BuyableCar[] GetCathegoryById(byte id)
        {
            switch (id)
            {
                case 0:
                    return Sports;

                case 1:
                    return OffRoad;

                case 2:
                    return Hatchbacks;

                case 3:
                    return Sedans;

                case 4:
                    return SUV;

                case 5:
                    return Trucks;

                case 6:
                    return Bikes;

                case 7:
                    return Special;
            }

            return null;
        }
    }
}
