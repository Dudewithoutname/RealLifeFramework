using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealLifeFramework
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EventHandler : Attribute
    {

        [Obsolete]
        public EventHandler(string name)
        {
        }

        public EventHandler()
        {
        }
    }

}
