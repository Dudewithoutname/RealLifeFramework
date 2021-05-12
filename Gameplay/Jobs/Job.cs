using Rocket.Unturned.Player;
using System;

namespace RealLifeFramework.Jobs
{
    public abstract class Job
    {
        public static ushort MaxLevel = 8;
        public abstract ushort Id { get; }
        public abstract string Name { get; }
       // public abstract bool isRestricted { get; }
        public virtual uint BaseSalary { get; }

        public virtual uint Salary(uint Level)
        {
            switch (Level)
            {
                case 1:
                    return BaseSalary;
                case 2:
                    return (uint)Math.Round(BaseSalary * 1.25);
                case 3:
                    return (uint)Math.Round(BaseSalary * 1.5);
                case 4:
                    return (uint)Math.Round(BaseSalary * 1.75);
                case 5:
                    return (uint)Math.Round(BaseSalary * 2.0);
                case 6:
                    return (uint)Math.Round(BaseSalary * 2.5);
                case 7:
                    return (uint)Math.Round(BaseSalary * 3.0);
                case 8:
                    return (uint)Math.Round(BaseSalary * 3.5);
                default:
                    return BaseSalary;
            }
        }

    }
}
