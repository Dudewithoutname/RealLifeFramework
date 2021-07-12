using RealLifeFramework.Chatting;
using RealLifeFramework.Ranks;
using RealLifeFramework.RealPlayers;
using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections.Generic;
using UnityEngine;

namespace RealLifeFramework.Commands
{
    public class CmdColorName : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "namecolor";

        public string Help => "namecolor";

        public string Syntax => "/namecolor (colorid)";

        public List<string> Aliases => new List<string>() { "farbamena", "ncolor", "farbajmena" };

        public List<string> Permissions => new List<string> { RankManager.PlayerPermission };

        public void Execute(IRocketPlayer caller, string[] args)
        {
            var player = RealPlayer.From(((UnturnedPlayer)caller).CSteamID);

            if (player.RankUser.Vip != null)
            {
                if (args.Length < 1)
                {
                    ChatManager.say(player.CSteamID, "Musis zadat farbu!", Palette.COLOR_R, EChatMode.SAY, false);
                    return;
                }
                
                var colorId = args[0].ToLower();

                if (!ChatColors.NameColors.ContainsKey(colorId))
                {
                    ChatManager.say(player.CSteamID, "Nezname id farby!", Palette.COLOR_R, EChatMode.SAY, false);
                }
                else
                {
                    var namecolor = ChatColors.NameColors[colorId];

                    if (player.Player.channel.owner.isAdmin)
                    {
                        ChatManager.say(player.CSteamID, $"Uspesne si si nastavil farbu <b><color={namecolor.Color}>{colorId}</color></b>", Palette.COLOR_W, true);
                        player.ChatProfile.NameColor = namecolor.Color;
                        return;
                    }
                    
                    if (namecolor.Level == 255 && player.RankUser.Vip.Value.Level != 0)
                    {
                        ChatManager.say(player.CSteamID, $"Farba <b><color={namecolor.Color}>{colorId}</color></b> je exkluzivna iba pre veteranov", Palette.COLOR_R, true);
                        return;
                    }

                    if (namecolor.Level != 255 && namecolor.Level > player.RankUser.Vip.Value.Level)
                    {
                        ChatManager.say(player.CSteamID, $"Farba <b><color={namecolor.Color}>{colorId}</color></b> je dostupna iba pre {RankManager.VIPs[namecolor.Level].Prefix} a vyssie ranky", Palette.COLOR_R, true);
                        return;
                    }

                    ChatManager.say(player.CSteamID, $"Uspesne si si nastavil farbu <b><color={namecolor.Color}>{colorId}</color></b>", Palette.COLOR_W, true);
                    player.ChatProfile.NameColor = namecolor.Color;
                }
            }
            else
            {
                ChatManager.say(player.CSteamID, "Nemas permissie na zmenu farby mena!", Palette.COLOR_R, EChatMode.SAY, false);
            }
        }
    }
}
