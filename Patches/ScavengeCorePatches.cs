using System.Collections.Generic;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;

namespace RecycleReplacementMod.Patches;

/// <summary>
/// Scavenge 核心属性替换：EnergyVar(2) → EnergyVar(1)（仅作为卡面能量图标的渲染占位，
/// 实际获得能量仍是动态值，等于被消耗卡当前费用），HoverTip 移除 EnergyHoverTip 只保留 Exhaust。
/// 类型/目标/稀有度/费用一致；两卡均不自身消耗（塔1 Recycle 无 “NL Exhaust.” 尾句），
/// 因此不补 CanonicalKeywords。
/// </summary>
[HarmonyPatch(typeof(Scavenge), "CanonicalVars", MethodType.Getter)]
public static class ScavengeCanonicalVarsPatch
{
    private static void Postfix(Scavenge __instance, ref IEnumerable<DynamicVar> __result)
    {
        if (ModConfig.IsReplaceScavengeEnabled)
        {
            __result = new DynamicVar[] { new EnergyVar(1) };
        }
    }
}

[HarmonyPatch(typeof(Scavenge), "ExtraHoverTips", MethodType.Getter)]
public static class ScavengeExtraHoverTipsPatch
{
    private static void Postfix(Scavenge __instance, ref IEnumerable<IHoverTip> __result)
    {
        if (ModConfig.IsReplaceScavengeEnabled)
        {
            __result = new IHoverTip[] { HoverTipFactory.FromKeyword(CardKeyword.Exhaust) };
        }
    }
}
