using UnityEngine;
using SDG.Unturned;
using RealLifeFramework.License;

namespace RealLifeFramework.RealPlayers
{
    public class RealPlayerComponent : MonoBehaviour
    {
        public RealPlayer Player;

        public bool isHidden = true;
        private byte oldAmmo;

        private void FixedUpdate()
        {
            if (Player.Player.equipment.asset != null && Player.Player.equipment.asset.type == EItemType.GUN && Player.Player.equipment.state[10] != oldAmmo)
            {
                oldAmmo = Player.Player.equipment.state[10];
                RealPlayerManager.OnAmmoLowered.Invoke(Player, oldAmmo);
            }

            var rayCastInfo = DamageTool.raycast(new Ray(Player.Player.look.aim.position, Player.Player.look.aim.forward), 25f, RayMasks.PLAYER | RayMasks.PLAYER_INTERACT);

            if ((object)rayCastInfo.player != null && isHidden && rayCastInfo.player.equipment.asset.id == CitizenId.ItemId)
            {
                var target = RealPlayer.From(rayCastInfo.player);
                CitizenId.Show(Player, target);
            }

            if (!isHidden)
            {
                CitizenId.Hide(Player);
            }
        }
    }
}
