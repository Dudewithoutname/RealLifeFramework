using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RealLifeFramework.Taser
{
    [EventHandler]
    public class TaserController : IEventComponent
    {
        public void HookEvents()
        {
            RealLife.Instance.gameObject.AddComponent<Taser>();
        }
    }
}
