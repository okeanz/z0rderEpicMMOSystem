using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Rendering;

namespace EpicMMOSystem;

public static class TerminalCommands
{
    private static bool isServer => SystemInfo.graphicsDeviceType == GraphicsDeviceType.Null;
    private static string modName => EpicMMOSystem.ModName;
    private static Localizationold local => EpicMMOSystem.localizationold;

    [HarmonyPatch(typeof(ZNetScene), nameof(ZNetScene.Awake))]
    private static class ZrouteMethodsServerFeedback
    {
        private static void Postfix()
        {
            if (isServer) return;
            //Пришло приглашение в друзья
            ZRoutedRpc.instance.Register($"{modName} terminal_SetLevel",
                new Action<long, int>(RPC_SetLevel));
            //Приняли приглашение в друзья
            ZRoutedRpc.instance.Register($"{modName} terminal_ResetPoints",
                new Action<long>(RPC_ResetPoints));
        }
    }

    //Установка уровня
    private static void RPC_SetLevel(long sender, int level)
    {
        LevelSystem.LevelSystem.Instance.terminalSetLevel(level);
        Chat.instance.RPC_ChatMessage(200, Vector3.zero, 0, UserInfo.GetLocalUser(),
            String.Format(local["$terminal_set_level"], level), PrivilegeManager.GetNetworkUserId());
    }

    //Сброс поинтов
    private static void RPC_ResetPoints(long sender)
    {
        LevelSystem.LevelSystem.Instance.ResetAllParameter();
        Chat.instance.RPC_ChatMessage(200, Vector3.zero, 0, UserInfo.GetLocalUser(), local["$terminal_reset_points"],
            PrivilegeManager.GetNetworkUserId());
    }

    [HarmonyPatch(typeof(Terminal), nameof(Terminal.InitTerminal))]
    public class AddChatCommands
    {
        private static void Postfix()
        {
            long? getPlayerId(string name)
            {
                var clearName = name.Replace('&', ' ');
                var players = ZNet.instance.GetPlayerList();
                foreach (var playerInfo in players)
                {
                    if (playerInfo.m_name == clearName)
                    {
                        return playerInfo.m_characterID.UserID;
                    }
                }

                return null;
            }

            _ = new Terminal.ConsoleCommand("EpicMMOSystem", "Manages the EpicMMOSystem commands.",
                (Terminal.ConsoleEvent) (
                    args =>
                    {
                        if (!EpicMMOSystem.ConfigSync.IsAdmin)
                        {
                            if (ZNet.instance.IsServer())
                            {
                            }
                            else
                            {
                                args.Context.AddString("You are not an admin on this server.");
                                return;
                            }
                        }

                        if (args[1] == "level")
                        {
                            int level = Int32.Parse(args[2]);
                            string name = args[3];
                            if (args.Length > 4)
                            {
                                for (var i = 4; i < args.Length; i++)
                                {
                                    name += " " + args[i];
                                }
                            }

                            var userId = getPlayerId(name);
                            if (userId == null)
                            {
                                EpicMMOSystem.print("Player is not found");
                            }

                            ZRoutedRpc.instance.InvokeRoutedRPC(userId ?? 200, $"{modName} terminal_SetLevel", level);
                        }
                        else if (args[1] == "reset_points")
                        {
                            string name = args[2];
                            if (args.Length > 3)
                            {
                                for (var i = 3; i < args.Length; i++)
                                {
                                    name += " " + args[i];
                                }
                            }

                            var userId = getPlayerId(name);
                            if (userId == null)
                            {
                                EpicMMOSystem.print("Player is not found");
                            }

                            ZRoutedRpc.instance.InvokeRoutedRPC(userId ?? 200, $"{modName} terminal_ResetPoints");
                        }

                        args.Context.AddString("level [value] [name] - set level for player name");
                        args.Context.AddString("reset_points [name] - reset points from attribute for player name");
                    }),
                optionsFetcher: () => new List<string>
                    {"level", "reset_points"});
        }
    }
}