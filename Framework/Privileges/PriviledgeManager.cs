using RealLifeFramework.RealPlayers;
using Rocket.API;
using Rocket.Unturned.Player;
using System;
using System.Collections.Generic;

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
                    player.Privileges = new List<EPrivilege>();
                    player.Privileges.Add((EPrivilege)Enum.Parse(typeof(EPrivilege), name, true));
                }
            }
        }
    }
}
