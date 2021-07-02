using System.Collections.Generic;

namespace RealLifeFramework.UserInterface
{
    public static class Currency
    {
        public static Dictionary<ushort, uint> Money = new Dictionary<ushort, uint>()
        {
        //     ID      Value
            { 1056  , 1      },
            { 1057  , 2      },
            { 1051  , 5      },
            { 1052  , 10     },
            { 1053  , 20     },
            { 1054  , 50     },
            { 1055  , 100    },
            { 24301 , 500    },
            { 24302 , 1000   },
            { 24303 , 2500   },
            { 24304 , 5000   },
            { 24305 , 10000  },
            { 24306 , 50000  },
            { 24307 , 100000 },
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
    }
}
