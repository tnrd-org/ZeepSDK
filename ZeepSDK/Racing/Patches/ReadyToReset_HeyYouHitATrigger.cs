﻿using System;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;

namespace ZeepSDK.Racing.Patches;

[HarmonyPatch(typeof(ReadyToReset), nameof(ReadyToReset.HeyYouHitATrigger))]
internal class ReadyToReset_HeyYouHitATrigger
{
    public static event Action<float> TriggerFinish;
    public static event Action<float> TriggerCheckpoint;

    [UsedImplicitly]
    private static void Prefix(
        ReadyToReset __instance,
        out bool __state // __state is used to check whether the player has crossed the finish line before or not
    )
    {
        __state = __instance.actuallyFinished;
    }

    [UsedImplicitly]
    private static void Postfix(
        ReadyToReset __instance,
        bool isFinish,
        float timeOffset,
        bool __state // __state is used to check whether the player has crossed the finish line before or not
    )
    {
        float time = Mathf.Max(0, __instance.ticker.what_ticker + timeOffset);

        if (isFinish)
        {
            if (!__state && __instance.master.countFinishCrossing && __instance.actuallyFinished)
                TriggerFinish?.Invoke(time);
        }
        else
        {
            TriggerCheckpoint?.Invoke(time);
        }
    }
}
