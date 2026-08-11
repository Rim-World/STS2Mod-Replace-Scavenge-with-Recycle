using System.Linq;
using System.Threading.Tasks;
using HarmonyLib;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;

namespace RecycleReplacementMod.Patches;

/// <summary>
/// 原版 Scavenge.OnPlay：消耗 1 张手牌 → 下回合获得 EnergyVar(2)。
/// 替换为 Recycle：消耗 1 张手牌 → 立即获得与其当前费用相等的能量。
/// 0 费卡得 0 能量；X 费卡按塔1 口径获得玩家当前能量（即翻倍）；
/// 未选牌时不结算（塔1 打出时必须消耗一张牌）。
/// </summary>
[HarmonyPatch(typeof(Scavenge), "OnPlay")]
public static class ScavengeOnPlayPatch
{
    private static bool Prefix(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay,
        Scavenge __instance,
        ref Task __result)
    {
        if (!ModConfig.IsReplaceScavengeEnabled)
        {
            return true;
        }

        __result = RecycleOnPlay(choiceContext, cardPlay, __instance);
        return false;
    }

    private static async Task RecycleOnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay cardPlay,
        Scavenge instance)
    {
        CardModel? selected = (await CardSelectCmd.FromHand(
            choiceContext,
            instance.Owner,
            new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, 1),
            null,
            instance)).FirstOrDefault();

        if (selected is null)
        {
            // 塔1 口径：未选择卡牌则不结算能量
            return;
        }

        int energy = selected.EnergyCost.CostsX
            ? instance.Owner!.PlayerCombatState!.Energy
            : selected.EnergyCost.GetWithModifiers(CostModifiers.All);

        await CardCmd.Exhaust(choiceContext, selected);

        if (energy > 0)
        {
            await PlayerCmd.GainEnergy(energy, instance.Owner);
        }
    }
}

/// <summary>
/// 原版升级：下回合能量 +1（2→3）；替换为 Recycle 升级：费用 1→0。
/// </summary>
[HarmonyPatch(typeof(Scavenge), "OnUpgrade")]
public static class ScavengeOnUpgradePatch
{
    private static bool Prefix(Scavenge __instance)
    {
        if (!ModConfig.IsReplaceScavengeEnabled)
        {
            return true;
        }

        __instance.EnergyCost.UpgradeBy(-1);
        return false;
    }
}
