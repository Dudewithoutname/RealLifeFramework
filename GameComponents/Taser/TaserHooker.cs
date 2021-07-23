using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealLifeFramework.Taser
{
    // Taser's hooker a.k.a kurva je to xd
    [EventHandler]
    public class TaserHooker : IEventComponent
    {
        public void HookEvents()
        {
            RealLife.Instance.gameObject.AddComponent<UseableTasers>();
        }
    }
}
