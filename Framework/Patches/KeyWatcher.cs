using SDG.Unturned;
using System.Collections.Generic;
using System.Threading;

namespace RealLifeFramework.Patches
{
     public class UnturnedKeyWatcher
     {
            public Player Player;
            public int UpdateRate = 200;

            private bool IsRunning = false;
            private Dictionary<int, bool> LastMapping = new Dictionary<int, bool>();

            public delegate void UKeyDown(Player Player, UnturnedKey Key);

            public event UKeyDown KeyChanged;

            public event UKeyDown KeyUp;

            public event UKeyDown KeyDown;

            public bool CodeHotkey1Down { get { return LastMapping[(int)UnturnedKey.CodeHotkey1]; } }
            public bool CodeHotkey2Down { get { return LastMapping[(int)UnturnedKey.CodeHotkey2]; } }
            public bool CodeHotkey3Down { get { return LastMapping[(int)UnturnedKey.CodeHotkey3]; } }
            public bool CodeHotkey4Down { get { return LastMapping[(int)UnturnedKey.CodeHotkey4]; } }
            
            public UnturnedKeyWatcher(Player Player)
            {
                this.Player = Player;
                IsRunning = true;
                Thread RunThread = new Thread(UpdateLoop);
                RunThread.Start();
            }

            public void Stop()
            {
                IsRunning = false;
            }

            private void UpdateLoop()
            {
                while (IsRunning)
                {
                    for (int i = 0; i < Player.input.keys.Length - 1; i++)
                    {
                        bool Current = Player.input.keys[i];
                        if (LastMapping.ContainsKey(i))
                        {
                            bool Last = LastMapping[i];
                            if (Last != Current)
                            {
                                UnturnedKey Key = IntToUKey(i);
                                if (Key != UnturnedKey.Unknown)
                                {
                                    Thread Changed = new Thread(x => KeyChanged?.Invoke(Player, Key));
                                    Changed.Start();
                                    if (Current)
                                    {
                                        Thread KeyD = new Thread(x => KeyDown?.Invoke(Player, Key));
                                        KeyD.Start();
                                    }
                                    else
                                    {
                                        Thread KeyU = new Thread(x => KeyUp?.Invoke(Player, Key));
                                        KeyU.Start();
                                    }
                                }
                                LastMapping[i] = Current;
                            }
                        }
                        else
                        {
                            LastMapping[i] = Current;
                        }
                    }
                    Thread.Sleep(UpdateRate);
                }
            }

            private UnturnedKey IntToUKey(int i)
            {
                if (i == 9) return UnturnedKey.CodeHotkey1;
                if (i == 10) return UnturnedKey.CodeHotkey2;
                if (i == 11) return UnturnedKey.CodeHotkey3;
                if (i == 12) return UnturnedKey.CodeHotkey4;
                return UnturnedKey.Unknown;
            }
     }

     public enum UnturnedKey
     {
        Unknown = -1,
        CodeHotkey1 = 9,
        // Defaults to period
        CodeHotkey2 = 10,
        /// Defaults to forward slash
        CodeHotkey3 = 11,
        /// Defaults to semicolon
        CodeHotkey4 = 12,
     }
}