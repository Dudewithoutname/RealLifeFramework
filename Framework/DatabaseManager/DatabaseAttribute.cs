using System;
using System.Reflection;

namespace RealLifeFramework
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class DatabaseComponent : Attribute
    {
        public string Name { get; set; }

        public DatabaseComponent(string name)
        {
            Name = name;
        }
    }
}
