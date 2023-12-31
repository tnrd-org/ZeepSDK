﻿using System;
using HarmonyLib;
using ZeepkistClient;

namespace ZeepSDK.Multiplayer.Patches;

[HarmonyPatch(typeof(PhotonZeepkist), nameof(PhotonZeepkist.OnPlayerEnteredRoom))]
internal class PhotonZeepkist_OnPlayerEnteredRoom
{
    public static event Action<ZeepkistNetworkPlayer> PlayerEnteredRoom;

    private static void Postfix(ZeepkistNetworkPlayer other)
    {
        PlayerEnteredRoom?.Invoke(other);
    }
}
