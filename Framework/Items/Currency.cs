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
            string result = string.Format("{0:C}", uint.Parse(money));
            result = result.Remove(result.Length - 5);
            return $"{result} $";
        }

    }
}
