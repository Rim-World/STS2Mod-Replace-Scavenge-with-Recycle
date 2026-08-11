using System.Reflection;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;

namespace RecycleReplacementMod;

[ModInitializer("ModLoaded")]
public static class ModEntry
{
    public const string ModId = "RecycleReplacementMod";

    public static readonly string PortraitPng =
        $"res://{ModId}/images/card_portraits/big/recycle.png";

    private static Harmony? _harmony;

    public static void ModLoaded()
    {
        try
        {
            Log.Info($"{ModId}: loading...");
            _harmony = new Harmony(ModId);
            _harmony.PatchAll(Assembly.GetExecutingAssembly());
            Log.Info($"{ModId}: Harmony patches applied (Scavenge -> Recycle)");
        }
        catch (Exception e)
        {
            Log.Error($"{ModId}: failed to apply patches: {e}");
        }
    }
}
