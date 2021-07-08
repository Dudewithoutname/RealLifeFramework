using System;
using System.Collections.Generic;

namespace RealLifeFramework
{
    public static class Currency
    {
        public static Dictionary<ushort, uint> Money = new Dictionary<ushort, uint>()
        {
        //     ID      Value
            { 24307 , 100000 },
            { 24306 , 50000  },
            { 24305 , 10000  },
            { 24304 , 5000   },
            { 24303 , 2500   },
            { 24302 , 1000   },
            { 24301 , 500    },
            { 1055  , 100    },
            { 1054  , 50     },
            { 1053  , 20     },
            { 1051  , 5      },
            { 1057  , 2      },
            { 1056  , 1      },
        };

        public static ushort GetIdByValue(uint value)
        {
            foreach(var obj in Money)
            {
                if (obj.Value == value)
                {
                    return obj.Key;
                }
            }

            return 0;
        }

        public static string FormatMoney(string money)
        {
            string output = "";

            if (money.Length > 3)
            {
                decimal x = Math.Floor(((decimal)money.Length / 3));
                int remainder = Convert.ToInt32(money.Length - (x * 3));

                if (Math.Round((decimal)money.Length / 3, 2) == (money.Length / 3))
                {
                    for (var i = 0; i < money.Length; i += 3)
                        output += money.Substring(i, 3) + " ";

                    return output + " $";
                }
                else
                {
                    output += money.Substring(0, remainder) + " ";

                    for (var i = 0; i < (money.Length - remainder); i += 3)
                        output += money.Substring(i, 3) + " ";

                    return output + " $";
                }
            }
            else
            {
                return money + " $";
            }
        }

    }
}
