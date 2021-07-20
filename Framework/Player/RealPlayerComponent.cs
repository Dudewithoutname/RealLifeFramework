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

            var rayCastInfo = DamageTool.raycast(new Ray(Player.Player.look.aim.position, Player.Player.look.aim.forward), 3f, RayMasks.PLAYER | RayMasks.PLAYER_INTERACT);

            if ((object)rayCastInfo.player != null && isHidden)
            {
                if (rayCastInfo.player.channel.owner.playerID.steamID.ToString() != Player.CSteamID.ToString())
                {
                    if ((object)rayCastInfo.player.equipment.asset != null)
                    {
                        if (rayCastInfo.player.equipment.asset.id == CitizenId.ItemId)
                        {
                            var target = RealPlayer.From(rayCastInfo.player);
                            CitizenId.Show(Player, target);
                        }
                    }
                    else
                    {
                        if (!isHidden)
                        {
                            CitizenId.Hide(Player);
                        }
                    }
                }
            }

            if (!isHidden && (object)rayCastInfo.player == null)
            {
                CitizenId.Hide(Player);
            }
        }
    }
}
