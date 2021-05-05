using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealLifeFramework
{
    [AttributeUsage(AttributeTargets.Class |AttributeTargets.Struct)]
    public class EventHandler : Attribute
    {
        public string Name { get; set; }

        public EventHandler(string name)
        {
            Name = name;
        }
    }

}
