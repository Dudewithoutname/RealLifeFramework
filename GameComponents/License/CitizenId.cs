using RealLifeFramework.RealPlayers;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealLifeFramework.License
{
    public class CitizenId
    {
        public static ushort ItemId = 43888;
        private static ushort uiId = 41839;

        public static void Show(RealPlayer instigator, RealPlayer target)
        {
            var name = target.Name.Split(' ');
            instigator.Component.isHidden = false;

            EffectManager.sendUIEffect(uiId, 1041, instigator.TransportConnection, true);
            EffectManager.sendUIEffectText(1041, instigator.TransportConnection, true, "id_name", name[0]);
            EffectManager.sendUIEffectText(1041, instigator.TransportConnection, true, "id_surname", name[1]);
            EffectManager.sendUIEffectText(1041, instigator.TransportConnection, true, "id_age", target.Age.ToString());
        }

        public static void Hide(RealPlayer player)
        {
            player.Component.isHidden = true;
            EffectManager.askEffectClearByID(uiId, player.TransportConnection);
        }
    }
}
