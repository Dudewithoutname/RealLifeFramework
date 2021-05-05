using System;
using Rocket.Unturned;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System.Collections.Generic;
using RealLifeFramework.Players;
using RealLifeFramework.Skills;
using RealLifeFramework.UserInterface;
using RealLifeFramework.Chatting;
using System.Reflection;

namespace RealLifeFramework
{
    public static class EventManager
    {
        public static List<IEventComponent> EventComponents;

        public static void Load()
        {
            EventComponents = new List<IEventComponent>();

            GetEventComponents();

            foreach (IEventComponent component in EventComponents)
                component.HookEvents();

            Logger.Log("[EventManager] EventComponents were loaded");
        }

        private static void GetEventComponents()
        {
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
                if (type.GetCustomAttributes(typeof(EventHandler), true).Length > 0)
                    EventComponents.Add((IEventComponent)Activator.CreateInstance(type));
        }
    }
}
