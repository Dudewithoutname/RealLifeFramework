using RealLifeFramework.RealPlayers;
using Rocket.API;
using Rocket.Unturned.Player;
using System;

namespace RealLifeFramework
{
    public class PrivilegeManager
    {
        public static void InitialiazePrivileges(RealPlayer player)
        {
            foreach (string name in Enum.GetNames(typeof(EPrivilege)))
            {
                var perm = name.ToPermission();

                if (UnturnedPlayer.FromPlayer(player.Player).HasPermission(perm))
                {
                    var privilege = (EPrivilege)Enum.Parse(typeof(EPrivilege), name, true);

                    if (player.PrivilegeLevel < (byte)privilege)
                    {
                        player.PrivilegeLevel = (byte)privilege;
                    }
                }
            }

            switch (player.PrivilegeLevel)
            {

            }
        }

        public static void DetectPoliceRank(RealPlayer rp)
        {
            UnturnedPlayer player = UnturnedPlayer.FromPlayer(rp.Player);

            if (player.HasPermission(""))
            {

            }

            if
        }
    }
}
