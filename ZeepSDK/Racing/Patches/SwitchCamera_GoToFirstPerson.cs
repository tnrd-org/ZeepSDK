﻿using System;
using HarmonyLib;
using JetBrains.Annotations;

namespace ZeepSDK.Racing.Patches;

[HarmonyPatch(typeof(SwitchCamera), nameof(SwitchCamera.GoToFirstPerson))]
internal class SwitchCamera_GoToFirstPerson
{
    public static event Action EnteredFirstPerson;

    [UsedImplicitly]
    private static void Postfix()
    {
        EnteredFirstPerson?.Invoke();
    }
}
