using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;

namespace RecycleReplacementMod.Patches;

/// <summary>
/// 把 Scavenge 的卡图三个来源全部替换为模组内 STS1 回收卡图。
/// </summary>
[HarmonyPatch(typeof(CardModel), "PortraitPngPath", MethodType.Getter)]
public static class ScavengePortraitPngPathPatch
{
    private static bool Prefix(CardModel __instance, ref string __result)
    {
        if (__instance is not Scavenge || !ModConfig.IsReplaceScavengeEnabled)
        {
            return true;
        }

        __result = ModEntry.PortraitPng;
        return false;
    }
}

[HarmonyPatch(typeof(CardModel), "Portrait", MethodType.Getter)]
public static class ScavengePortraitPatch
{
    private static bool Prefix(CardModel __instance, ref Texture2D __result)
    {
        if (__instance is not Scavenge || !ModConfig.IsReplaceScavengeEnabled)
        {
            return true;
        }

        __result = PortraitTextureLoader.Get();
        return false;
    }
}

[HarmonyPatch(typeof(CardModel), "PortraitPath", MethodType.Getter)]
public static class ScavengePortraitPathPatch
{
    private static bool Prefix(CardModel __instance, ref string __result)
    {
        if (__instance is not Scavenge || !ModConfig.IsReplaceScavengeEnabled)
        {
            return true;
        }

        __result = ModEntry.PortraitPng;
        return false;
    }
}
