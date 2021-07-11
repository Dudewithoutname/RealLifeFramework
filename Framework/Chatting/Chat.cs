using SDG.Unturned;
using UnityEngine;
using RealLifeFramework.RealPlayers;
using Rocket.API;
using Rocket.Unturned.Player;
using RealLifeFramework.Ranks;

namespace RealLifeFramework.Chatting
{
    [EventHandler]
    public class Chat : IEventComponent
    {
        public void HookEvents()
        {
            ChatManager.onChatted += onPlayerChatted;
        }

        private static void onPlayerChatted(SteamPlayer player, EChatMode mode, ref Color chatted, ref bool isRich, string text, ref bool isVisible)
        {
            RealPlayer RealPlayer = RealPlayer.From(player.player);
            // TODO : Mute System , Administration system and this obviously
            if (text.StartsWith("/")) 
            {
                isVisible = false;
                return;
            }

            switch (mode)
            {
                case EChatMode.GLOBAL:
                    isVisible = SendGlobalMessage(RealPlayer, mode, ref chatted, ref isRich, text, ref isVisible);
                    break;
                case EChatMode.LOCAL:
                    isVisible = SendLocalMessage(RealPlayer, mode, ref chatted, ref isRich, text, ref isVisible);
                    break;
                case EChatMode.GROUP:
                    isVisible = SendGroupMessage(RealPlayer, mode, ref chatted, ref isRich, text, ref isVisible);
                    break;
            }
        }

        private static bool SendGlobalMessage(RealPlayer player, EChatMode mode, ref Color chatted, ref bool isRich, string text, ref bool isVisible)
        {
            string message = refactorMessage(text, player);

            if (isVisible)
            {
                ChatManager.serverSendMessage(
                    $"<size=11><color=#de4dff>[{player.Level}]</color>{player.RankUser.JobPrefix}</size><color={player.ChatProfile.NameColor}> |<b>{player.RankUser.DisplayPrefix}</b>| {player.Name} </color>:<color=#d9d9d9> {message}</color>",
                    Color.white, player.Player.channel.owner, null, EChatMode.GLOBAL, player.ChatProfile.Avatar, true);
            }

            return false;
        }

        private static bool SendLocalMessage(RealPlayer player, EChatMode mode, ref Color chatted, ref bool isRich, string text, ref bool isVisible)
        {
            string message = refactorMessage(text, player);

            if(isVisible)
            {
                ChatManager.serverSendMessage(
                    $"<size=11><color=#b3babd>(local)</color> <color=#de4dff>[{player.Level}]</color>{player.RankUser.JobPrefix}</size><color={player.ChatProfile.NameColor}> |<b>{player.RankUser.DisplayPrefix}</b>| {player.Name} </color>:<color=#d9d9d9> {message}</color>",
                    Color.white, player.Player.channel.owner, null, EChatMode.LOCAL, player.ChatProfile.Avatar, true);
            }

            return false;
        }

        private static bool SendGroupMessage(RealPlayer player, EChatMode mode, ref Color chatted, ref bool isRich, string text, ref bool isVisible)
        {
            string message = refactorMessage(text, player);

            if(isVisible)
            {
                ChatManager.serverSendMessage(
                    $"<size=11><color=#b3babd>(group)</color> <color=#de4dff>[{player.Level}]</color>{player.RankUser.JobPrefix}</size><color={player.ChatProfile.NameColor}> |<b>{player.RankUser.DisplayPrefix}</b>| {player.Name} </color>:<color=#d9d9d9> {message}</color>",
                    Color.white, player.Player.channel.owner, null, EChatMode.GROUP, player.ChatProfile.Avatar, true);
            }

            return false;
        }

        private static string refactorMessage(string message, RealPlayer player)
        {
            string output = message;

            if ((message.Contains("<") || message.Contains(">")) && player.RankUser != null && player.RankUser.Admin.Value.Level >= RankManager.Admins[2].Level)
            {
                output.Replace("<", "(");
                output.Replace(">", ")");
            }

            return output;
        }
    }
}
