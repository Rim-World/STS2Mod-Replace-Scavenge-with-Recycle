using HarmonyLib;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;

namespace RecycleReplacementMod.Patches;

/// <summary>
/// 文本替换补丁：本地化文件新增 RE_RECYCLE.title/.description 键
/// （模组专属前缀，避免与其他 mod 的 RECYCLE.* 键冲突），由这里切换到新键。
/// </summary>
[HarmonyPatch(typeof(CardModel), "TitleLocString", MethodType.Getter)]
public static class ScavengeTitleLocStringPatch
{
    private static void Postfix(CardModel __instance, ref LocString __result)
    {
        if (__instance is Scavenge && ModConfig.IsReplaceScavengeEnabled)
        {
            __result = new LocString("cards", "RE_RECYCLE.title");
        }
    }
}

[HarmonyPatch(typeof(CardModel), "Description", MethodType.Getter)]
public static class ScavengeDescriptionPatch
{
    private static void Postfix(CardModel __instance, ref LocString __result)
    {
        if (__instance is Scavenge && ModConfig.IsReplaceScavengeEnabled)
        {
            __result = new LocString("cards", "RE_RECYCLE.description");
        }
    }
}
