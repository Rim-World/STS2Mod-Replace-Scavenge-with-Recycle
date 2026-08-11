namespace RecycleReplacementMod;

/// <summary>
/// 替换功能固定开启（已移除 RitsuLib 实时开关；2026-08-11 按用户要求移除形状守卫）。
/// </summary>
public static class ModConfig
{
    public static bool IsReplaceScavengeEnabled => true;
}
