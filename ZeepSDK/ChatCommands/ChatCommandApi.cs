﻿using System;
using BepInEx.Logging;
using JetBrains.Annotations;
using UnityEngine;
using ZeepSDK.ChatCommands.Commands;
using ZeepSDK.Utilities;

namespace ZeepSDK.ChatCommands;

/// <summary>
/// An API that allows you to register chat commands
/// </summary>
[PublicAPI]
public static class ChatCommandApi
{
    private static ManualLogSource logger = LoggerFactory.GetLogger(typeof(ChatCommandApi));

    internal static void Initialize(GameObject gameObject)
    {
        gameObject.AddComponent<RemoteChatMessageHandler>();

        RegisterLocalChatCommand<HelpLocalChatCommand>();
        RegisterLocalChatCommand<ClearChatLocalChatCommand>();

        RegisterRemoteChatCommand<HelpRemoteChatCommand>();
    }

    /// <summary>
    /// Allows you to register a local chat command
    /// </summary>
    /// <param name="prefix">The prefix to use for the command</param>
    /// <param name="command">The keyword for the command</param>
    /// <param name="description">The description for the command</param>
    /// <param name="callback">The callback to invoke whenever the command gets used</param>
    public static void RegisterLocalChatCommand(
        string prefix,
        string command,
        string description,
        LocalChatCommandCallbackDelegate callback
    )
    {
        try
        {
            ChatCommandRegistry.RegisterLocalChatCommand(new LocalChatCommandWrapper(prefix,
                command,
                description,
                callback));
        }
        catch (Exception e)
        {
            logger.LogError($"Unhandled exception in {nameof(RegisterLocalChatCommand)}: " + e);
        }
    }

    /// <summary>
    /// Allows you to register a local chat command
    /// </summary>
    /// <typeparam name="TChatCommand">The type of the chat command to create a new instance of</typeparam>
    public static void RegisterLocalChatCommand<TChatCommand>()
        where TChatCommand : ILocalChatCommand, new()
    {
        try
        {
            ChatCommandRegistry.RegisterLocalChatCommand(new TChatCommand());
        }
        catch (Exception e)
        {
            logger.LogError($"Unhandled exception in {nameof(RegisterLocalChatCommand)}: " + e);
        }
    }

    /// <summary>
    /// Allows you to register a remote chat command
    /// </summary>
    /// <param name="prefix">The prefix for the command. Note: this should not start with a / as this does not work</param>
    /// <param name="command">The keyword for the command</param>
    /// <param name="description">The description for the command</param>
    /// <param name="callback">The callback to invoke whenever the command gets used</param>
    public static void RegisterRemoteChatCommand(
        string prefix,
        string command,
        string description,
        RemoteChatCommandCallbackDelegate callback
    )
    {
        try
        {
            ChatCommandRegistry.RegisterRemoteChatCommand(new RemoteChatCommandWrapper(prefix,
                command,
                description,
                callback));
        }
        catch (Exception e)
        {
            logger.LogError($"Unhandled exception in {nameof(RegisterRemoteChatCommand)}: " + e);
        }
    }

    /// <summary>
    /// Allows you to register a remote chat command
    /// </summary>
    /// <typeparam name="TChatCommand">The type of the chat command to create a new instanc eof</typeparam>
    public static void RegisterRemoteChatCommand<TChatCommand>()
        where TChatCommand : IRemoteChatCommand, new()
    {
        try
        {
            ChatCommandRegistry.RegisterRemoteChatCommand(new TChatCommand());
        }
        catch (Exception e)
        {
            logger.LogError($"Unhandled exception in {nameof(RegisterRemoteChatCommand)}: " + e);
        }
    }
}
