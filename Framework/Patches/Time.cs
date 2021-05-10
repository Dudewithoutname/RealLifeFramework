using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDG.Unturned;
using HarmonyLib;

namespace RealLifeFramework.Patches
{
    [HarmonyPatch(typeof(LightingManager))]
    [HarmonyPatch("updateLighting")]
    internal class Time
    {
        public static onTimeUpdate onTimeUpdated;
        private static ushort prevMinutes = 0;
        [HarmonyPrefix]
        private static void TimeUpdate()
        {
            var uhours = (decimal)LightingManager.cycle / 24;
            var uminutes = (decimal)LightingManager.cycle / 1440;

            var defaultUTime = LightingManager.time - (decimal)LightingManager.cycle / 36;

            var hours = (defaultUTime / uhours + 5) % 24;
            var minutes = (defaultUTime % uhours / uminutes + 60) % 60;
            if((ushort)minutes != prevMinutes)
            {
                prevMinutes = (ushort)minutes;
                onTimeUpdated.Invoke((ushort)hours, (ushort)minutes);
            }

        }

        public delegate void onTimeUpdate(ushort hours, ushort minutes);
    }
}
