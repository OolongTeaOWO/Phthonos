using Phthonos.Common.EquipmentDisplay.Profiles;
using Terraria;

namespace Phthonos.Common.EquipmentDisplay.Layouts;

/// <summary>將主、副手的標籤一次解析成不互相重疊的繪製布局。</summary>
public static class EquipmentLayoutResolver
{
    public static ResolvedEquipmentLayout Resolve(EquipmentDisplayState state)
    {
        var layout = new ResolvedEquipmentLayout();
        if (state.Player.dead)
            return layout;

        WeaponProfile mainProfile = null;
        WeaponProfile offhandProfile = null;
        bool hasMain = WeaponProfileResolver.TryResolve(state.MainHand, out mainProfile);
        bool hasOffhand = WeaponProfileResolver.TryResolve(state.Offhand, out offhandProfile);

        if (!hasMain && !hasOffhand)
            return layout;

        WeaponCarryStyle mainStyle = hasMain ? mainProfile.CarryStyle : WeaponCarryStyle.None;
        WeaponCarryStyle offhandStyle = hasOffhand
            ? ResolveOffhandStyle(mainProfile, offhandProfile, hasMain)
            : WeaponCarryStyle.None;

        // 正在攻擊的武器交給 Terraria 原本的 HeldItem 圖層；另一把維持配對後姿勢。
        if (hasMain && !state.IsUsingMainHand)
        {
            WeaponCarryStyle visibleMainStyle = state.IsUsingOffhand && mainStyle == WeaponCarryStyle.DualHands
                ? WeaponCarryStyle.Back
                : mainStyle;
            AddPassiveDraw(layout, state.Player, state.MainHand, visibleMainStyle);
        }

        if (hasOffhand && !state.IsUsingOffhand && offhandStyle != WeaponCarryStyle.None)
        {
            WeaponCarryStyle visibleOffhandStyle = state.IsUsingMainHand && offhandStyle == WeaponCarryStyle.DualHands
                ? WeaponCarryStyle.Back
                : offhandStyle;

            // 沒有攻擊時，兩個雙手姿勢仍由主手優先，避免同時占用雙手。
            if (!(visibleOffhandStyle == WeaponCarryStyle.DualHands && mainStyle == WeaponCarryStyle.DualHands && !state.IsUsingMainHand))
                AddPassiveDraw(layout, state.Player, state.Offhand, visibleOffhandStyle);
        }

        return layout;
    }

    private static WeaponCarryStyle ResolveOffhandStyle(WeaponProfile mainProfile, WeaponProfile offhandProfile, bool hasMain)
    {
        WeaponCarryStyle offhandStyle = offhandProfile.CarryStyle;

        // 腰部只有一個位置：主手保留腰掛，副手僅在本次布局中改用背掛。
        // 不修改副手原始標籤，因此副手單獨存在時仍會恢復為腰掛。
        if (hasMain && mainProfile.CarryStyle == WeaponCarryStyle.Waist && offhandStyle == WeaponCarryStyle.Waist)
            return WeaponCarryStyle.Back;

        if (hasMain && mainProfile.CarryStyle == offhandStyle && offhandProfile.CanUseDragFallback)
            return WeaponCarryStyle.Drag;

        // 每種姿勢只有一個固定位置；可用拖地後備的副手已在上方轉換，其餘衝突先隱藏。
        if (hasMain && mainProfile.CarryStyle == offhandStyle && HasSingleFixedPosition(offhandStyle))
            return WeaponCarryStyle.None;

        return offhandStyle;
    }

    private static bool HasSingleFixedPosition(WeaponCarryStyle style)
    {
        return style == WeaponCarryStyle.Waist ||
            style == WeaponCarryStyle.Back ||
            style == WeaponCarryStyle.Shoulder ||
            style == WeaponCarryStyle.ShieldBack ||
            style == WeaponCarryStyle.Drag;
    }

    private static void AddPassiveDraw(ResolvedEquipmentLayout layout, Player player, Item item, WeaponCarryStyle style)
    {
        if (style == WeaponCarryStyle.Drag)
            layout.HandDraws.Add(EquipmentPoseFactory.Create(player, item, style));
        else
            layout.PassiveDraws.Add(EquipmentPoseFactory.Create(player, item, style));
    }
}
