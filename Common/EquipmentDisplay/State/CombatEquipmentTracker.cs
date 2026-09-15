using Phthonos.Common.EquipmentDisplay.Layouts;
using Phthonos.Common.EquipmentDisplay.Offhand;
using Phthonos.Common.EquipmentDisplay.Profiles;
using Terraria;
using Terraria.ModLoader;

namespace Phthonos.Common.EquipmentDisplay.State;

/// <summary>保留最後一把可顯示的戰鬥武器，喝藥水等暫時切物品時不會讓布局換武器。</summary>
public sealed class CombatEquipmentTracker : ModPlayer
{
    private Item _lastCombatMainHand;

    public override void PostUpdate()
    {
        if (Player.dead)
        {
            _lastCombatMainHand = null;
            return;
        }

        OffhandUseController controller = Player.GetModPlayer<OffhandUseController>();

        // 暫時切到藥水時，原武器仍在背包內，所以保留布局；
        // 若武器已被移出背包，必須讓快取失效，避免留下幽靈貼圖。
        if (!controller.IsUsingOffhand && _lastCombatMainHand is not null && !IsItemStillInInventory(_lastCombatMainHand))
            _lastCombatMainHand = null;

        Item candidate = controller.IsUsingOffhand ? controller.SavedMainHand : Player.HeldItem;
        if (WeaponProfileResolver.TryResolve(candidate, out _))
            _lastCombatMainHand = candidate;
        else if (candidate is null || candidate.IsAir || !candidate.potion)
            _lastCombatMainHand = null;
    }

    public override void UpdateDead() => _lastCombatMainHand = null;

    public ResolvedEquipmentLayout GetResolvedLayout()
    {
        return EquipmentLayoutResolver.Resolve(GetDisplayState());
    }

    /// <summary>取得目前送進布局器的主、副手狀態，供繪製與只讀診斷共用。</summary>
    public EquipmentDisplayState GetDisplayState()
    {
        OffhandUseController controller = Player.GetModPlayer<OffhandUseController>();
        OffhandSlotHelper.TryGetItem(Player, out Item offhand);

        // 繪製時讀取現況，避免 UI 在 PostUpdate 後移走物品仍畫出舊武器。
        Item candidate = controller.IsUsingOffhand ? controller.SavedMainHand : Player.HeldItem;
        Item mainHand = null;
        if (WeaponProfileResolver.TryResolve(candidate, out _))
            mainHand = candidate;
        else if (candidate is not null && !candidate.IsAir && candidate.potion &&
            _lastCombatMainHand is not null && !_lastCombatMainHand.IsAir &&
            IsItemStillInInventory(_lastCombatMainHand))
            mainHand = _lastCombatMainHand;

        // 同一個物品實例移入副手後，不能再當成主手重複繪製。
        if (ReferenceEquals(mainHand, offhand))
            mainHand = null;

        // 只有目前 HeldItem 確實是主武器時，原版攻擊繪製才會取代主武器的收納繪製。
        // 喝藥水時使用的是快取武器，不應被誤判為正在使用主武器。
        bool isUsingMainHand = !controller.IsUsingOffhand && Player.itemAnimation > 0 &&
            ReferenceEquals(mainHand, Player.HeldItem);

        return new EquipmentDisplayState(Player, mainHand, offhand, isUsingMainHand, controller.IsUsingOffhand);
    }

    private bool IsItemStillInInventory(Item trackedItem)
    {
        foreach (Item inventoryItem in Player.inventory)
        {
            if (ReferenceEquals(inventoryItem, trackedItem))
                return true;
        }

        return false;
    }
}
