﻿using System;
using System.Linq;
using BepInEx.Logging;
using HarmonyLib;
using JetBrains.Annotations;
using ZeepSDK.Utilities;

namespace ZeepSDK.ChatCommands.Patches;

[HarmonyPatch(typeof(OnlineChatUI), nameof(OnlineChatUI.SendChatMessage))]
internal class OnlineChatUI_SendChatMessage
{
    private static readonly ManualLogSource logger = LoggerFactory.GetLogger(typeof(OnlineChatUI_SendChatMessage));

    [UsedImplicitly]
    private static bool Prefix(string message)
    {
        try
        {
            bool executedCustomCommand = false;

            foreach (ILocalChatCommand localChatCommand in ChatCommandRegistry.LocalChatCommands)
            {
                if (ProcessLocalChatCommand(localChatCommand, message))
                    executedCustomCommand = true;
            }

            return !executedCustomCommand;
        }
        catch (Exception e)
        {
            logger.LogError($"Unhandled exception in {nameof(Prefix)}: " + e);
            return true;
        }
    }

    private static bool ProcessLocalChatCommand(ILocalChatCommand localChatCommand, string message)
    {
        if (localChatCommand == null)
            return false;

        if (!ChatCommandUtilities.MatchesCommand(message, localChatCommand))
            return false;

        string arguments = ChatCommandUtilities.GetArguments(message, localChatCommand);

        try
        {
            localChatCommand.Handle(arguments);
        }
        catch (Exception e)
        {
            ManualLogSource manualLogSource = LoggerFactory.GetLogger(localChatCommand);
            manualLogSource.LogError($"Unhandled exception in {localChatCommand.GetType().Name}: " + e);
            return false;
        }

        return true;
    }
}
