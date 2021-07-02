using UnityEngine;
using SDG.Unturned;

namespace RealLifeFramework.RealPlayers
{
    public class RealPlayerComponent : MonoBehaviour
    {
        public RealPlayer Player;
        private byte oldAmmo;

        private void FixedUpdate()
        {
            if (Player.Player.equipment.asset != null && Player.Player.equipment.asset.type == EItemType.GUN && Player.Player.equipment.state[10] != oldAmmo)
            {
                oldAmmo = Player.Player.equipment.state[10];
                RealPlayerManager.OnAmmoLowered.Invoke(Player, oldAmmo);
            }
        }
    }
}
