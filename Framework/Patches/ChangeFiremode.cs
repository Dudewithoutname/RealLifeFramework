using System;
using SDG.Unturned;
using HarmonyLib;
using SDG.NetPak;

namespace RealLifeFramework.Patches
{
    [HarmonyPatch(typeof(UseableGun_NetMethods), nameof(UseableGun_NetMethods.ReceiveChangeFiremode_Read))]
    internal class ChangeFiremode
    {
        public static onFireChanged OnFiremodeChanged;

        [HarmonyPrefix]
        private static void Prefix(in ServerInvocationContext context)
        {
            NetPakReader reader = context.reader;
            NetId key;

            if (!reader.ReadNetId(out key))
                return;

            UseableGun useableGun = NetIdRegistry.Get<UseableGun>(key);

            if ((Object)useableGun == (Object)null)
                return;

            // replacement of IsOwnerOf(SteamChannel legacyComponent) because it's internal
            if (!(useableGun.channel.owner != null && useableGun.channel.owner == context.GetCallingPlayer()))
            {
                context.Kick(string.Format("not owner of {0}", (object)useableGun));
            }
            else
            {
                EFiremode newFiremode;
                reader.ReadEnum(out newFiremode);
                useableGun.ReceiveChangeFiremode(newFiremode);
                OnFiremodeChanged.Invoke(useableGun.player, newFiremode);
            }

        }

        public delegate void onFireChanged(Player player, EFiremode firemode);
    }

}
