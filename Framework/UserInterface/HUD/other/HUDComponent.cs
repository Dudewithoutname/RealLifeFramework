using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealLifeFramework.UserInterface
{
    public static class HUDComponent
    {
        // * Life Stats
        public const string LifeStats = "lifeStats";
        public const string Health = "healthValue";
        public const string Food = "foodValue";
        public const string Water = "waterValue";
        public const string Stamina = "energyValue";

        // * Weapon Stats
        public const string WeaponStats = "ammoContainer";
        public const string Ammo = "ammo";
        public const string FullAmmo = "fullAmmo";
        public const string Firemode = "fireMode";

        // * Vehicle Stats
        public const string SeatBeltStats = "seatbelt";
        public const ushort UseBelt = 0;
        public const ushort RemoveBelt = 0;
        public static readonly string[] Seatbelt = { "offBelt", "onBelt" };

        // * Left Stats
        public const string LeftStats = "leftContainer";
        public const string Time = "timeValue";
        public const string Level = "levelValue";
        public const string Exp = "expValue";
        public const string Wallet = "walletValue";
        public const string Credit = "creditValue";

        // * Voice
        public const string VoiceStats = "voice";
        public const string Voice = "voiceIcon";
    }
}
