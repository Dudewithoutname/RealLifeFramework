using Rocket.API;
using Rocket.Unturned;
using Rocket.Unturned.Player;
using System;

namespace RealLifeFramework.Privileges
{
    [EventHandler]
    public class PrivilegeManager : IEventComponent
    {
        public void HookEvents()
        {
            U.Events.OnPlayerConnected += initialiazePrivileges;
        }

        private void initialiazePrivileges(UnturnedPlayer player)
        {
            if (player.HasPermission(EPrivilege.ADMIN.ToPermission()))
            {
                return; // dokonci
            }
        }
    }
}
