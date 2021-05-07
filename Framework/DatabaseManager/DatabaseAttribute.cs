using System;
using System.Reflection;

namespace RealLifeFramework
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class EventHandler : Attribute
    {
        public string Name { get; set; }

        public EventHandler(string name)
        {
            Name = name;
        }
    }
}
