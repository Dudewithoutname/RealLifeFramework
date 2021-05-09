using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealLifeFramework
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class Table : Attribute
    {
        public string Name { get; set; }

        public Table(string name)
        {
            Name = name;
        }
    }
}
