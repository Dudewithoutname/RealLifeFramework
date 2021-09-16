using System;

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
