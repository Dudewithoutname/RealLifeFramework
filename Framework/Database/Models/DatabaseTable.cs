using System;

namespace RealLifeFramework.Database
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DatabaseTable : Attribute
    {
        public DatabaseTable() { }
    }
}
