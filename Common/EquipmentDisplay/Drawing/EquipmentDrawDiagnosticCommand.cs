using Phthonos.Common.EquipmentDisplay.Layouts;
using Phthonos.Common.EquipmentDisplay.Profiles;
using Phthonos.Common.EquipmentDisplay.State;
using Terraria;
using Terraria.ModLoader;

namespace Phthonos.Common.EquipmentDisplay.Drawing;

/// <summary>只讀取實際布局，不改變姿勢或物品。用於確認遊戲載入的繪製版本。</summary>
public sealed class EquipmentDrawDiagnosticCommand : ModCommand
{
    public override string Command => "equipdraw";
    public override CommandType Type => CommandType.Chat;
    public override string Usage => "/equipdraw";
    public override string Description => "顯示武器繪製版本、方向與實際姿勢";

    public override void Action(CommandCaller caller, string input, string[] args)
    {
        Player player = caller.Player;
        caller.Reply($"繪製診斷 MIRROR-20260910-A｜方向={player.direction} 重力={player.gravDir} 動作={player.itemAnimation}");
        CombatEquipmentTracker tracker = player.GetModPlayer<CombatEquipmentTracker>();
        EquipmentDisplayState state = tracker.GetDisplayState();
        ResolvedEquipmentLayout layout = EquipmentLayoutResolver.Resolve(state);

        ReportSlot(caller, "主手", state.MainHand, layout);
        ReportSlot(caller, "副手", state.Offhand, layout);

        caller.Reply("—— 實際繪製參數 ——");
        foreach (ResolvedWeaponDraw weapon in layout.PassiveDraws)
            Report(caller, player, weapon, "收納");
        foreach (ResolvedWeaponDraw weapon in layout.HandDraws)
            Report(caller, player, weapon, "手持");
        if (layout.PassiveDraws.Count + layout.HandDraws.Count == 0)
            caller.Reply("目前沒有自訂武器繪製項目。若仍有武器圖像，來源不在本次布局。");
    }

    private static void ReportSlot(CommandCaller caller, string slotName, Item item, ResolvedEquipmentLayout layout)
    {
        if (item is null || item.IsAir)
        {
            caller.Reply($"{slotName}：空");
            return;
        }

        if (!WeaponProfileResolver.TryResolve(item, out WeaponProfile profile))
        {
            caller.Reply($"{slotName}：{item.Name}（{item.type}）｜無可用分類");
            return;
        }

        ResolvedWeaponDraw resolved = FindResolvedDraw(layout, item);
        string appliedPose = resolved is null ? "未繪製" : resolved.Style.ToString();
        caller.Reply($"{slotName}：{item.Name}（{item.type}）｜原始={profile.CarryStyle}｜套用={appliedPose}");
    }

    private static ResolvedWeaponDraw FindResolvedDraw(ResolvedEquipmentLayout layout, Item item)
    {
        foreach (ResolvedWeaponDraw weapon in layout.PassiveDraws)
            if (ReferenceEquals(weapon.Item, item))
                return weapon;

        foreach (ResolvedWeaponDraw weapon in layout.HandDraws)
            if (ReferenceEquals(weapon.Item, item))
                return weapon;

        return null;
    }

    private static void Report(CommandCaller caller, Player player, ResolvedWeaponDraw weapon, string layer)
    {
        caller.Reply($"{layer}：{weapon.Item.Name}｜{weapon.Style}｜角度={weapon.RotationDegrees:0.##}");
        caller.Reply($"偏移={weapon.Offset} 錨點={weapon.OriginRatio} 翻面={EquipmentDrawHelper.GetSpriteEffects(player)}");
    }
}
