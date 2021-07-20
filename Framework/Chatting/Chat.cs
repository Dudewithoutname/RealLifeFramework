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

            if (RealPlayer == null) return;

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
                    $"<size=11><color=#de4dff>[{player.Level}]</color><color={player.ChatProfile.NameColor}>{player.RankUser.JobPrefix}</size> <b>|</b>{player.RankUser.DisplayPrefix}<b>| {player.Name}</b> </color>:<color=#d9d9d9> {message}</color>",
                    Color.white, null, null, EChatMode.GLOBAL, player.ChatProfile.Avatar, true);
            }

            return false;
        }

        private static bool SendLocalMessage(RealPlayer player, EChatMode mode, ref Color chatted, ref bool isRich, string text, ref bool isVisible)
        {
            string message = refactorMessage(text, player);

            if (isVisible)
            {
                foreach (SteamPlayer client in Provider.clients)
                {
                    if (!((UnityEngine.Object)client.player == (UnityEngine.Object)null) && (double)(client.player.transform.position - player.Player.transform.position).sqrMagnitude < 16384f)
                        ChatManager.serverSendMessage(
                        $"<size=11><color=#58c45d><Area></color> <color=#de4dff>[{player.Level}]</color><color={player.ChatProfile.NameColor}>{player.RankUser.JobPrefix}</size> <b>|</b>{player.RankUser.DisplayPrefix}<b>| {player.Name}</b> </color>:<color=#d9d9d9> {message}</color>",
                        Color.white, null, client, EChatMode.LOCAL, player.ChatProfile.Avatar, true);
                }
            }

            return false;
        }

        private static bool SendGroupMessage(RealPlayer player, EChatMode mode, ref Color chatted, ref bool isRich, string text, ref bool isVisible)
        {
            string message = refactorMessage(text, player);

            if (isVisible)
            {
                foreach (SteamPlayer client in Provider.clients)
                {
                    if (!((UnityEngine.Object)client.player == (UnityEngine.Object)null) && client.player.quests.isMemberOfSameGroupAs(player.Player))
                        ChatManager.serverSendMessage(
                        $"<size=11><color=#58c45d><Group></color> <color=#de4dff>[{player.Level}]</color><color={player.ChatProfile.NameColor}>{player.RankUser.JobPrefix}</size> <b>|</b>{player.RankUser.DisplayPrefix}<b>| {player.Name}</b> </color>:<color=#d9d9d9> {message}</color>",
                        Color.white, null, client, EChatMode.GROUP, player.ChatProfile.Avatar, true);

                }
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
