using Phthonos.Common.Systems;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace Phthonos.Common.EquipmentDisplay.Offhand;

public class OffhandUseController : ModPlayer
{
    private Item _savedMainHand;
    private int _savedSelectedItem = -1;
    private bool _releaseRequested;

    public bool IsUsingOffhand { get; private set; }

    public Item SavedMainHand => _savedMainHand;

    public override void PreUpdate()
    {
        if (Player.dead || !Player.active)
        {
            StopUsingOffhand();
            return;
        }

        if (!IsUsingOffhand)
            return;

        // 快捷欄被玩家切換時必須立即還原原格，避免副手物品留在錯誤的位置。
        if (Player.selectedItem != _savedSelectedItem)
        {
            StopUsingOffhand();
            return;
        }

        // 放開按鍵只停止新的使用輸入；既有動作完成以前仍保持副手為 HeldItem。
        if (!KeybindSystem.Offhand.Current)
            _releaseRequested = true;

        if (!OffhandSlotHelper.TryGetItem(Player, out Item currentOffhand))
        {
            StopUsingOffhand();
            return;
        }

        Player.inventory[_savedSelectedItem] = currentOffhand;

        if (_releaseRequested && Player.itemAnimation <= 0 && Player.itemTime <= 0)
            StopUsingOffhand();
    }

    public override void ProcessTriggers(TriggersSet triggersSet)
    {
        if (Player.dead || !Player.active)
            return;

        if (!IsUsingOffhand && KeybindSystem.Offhand.Current && OffhandSlotHelper.TryGetItem(Player, out Item offhand))
            StartUsingOffhand(offhand);
    }

    // 此方法在原版輸入狀態建立後執行，設定 controlUseItem 不會被滑鼠輸入覆蓋。
    public override void SetControls()
    {
        if (IsUsingOffhand && !_releaseRequested)
            Player.controlUseItem = true;
    }

    public override void UpdateDead() => StopUsingOffhand();

    private void StartUsingOffhand(Item offhand)
    {
        _savedSelectedItem = Player.selectedItem;
        _savedMainHand = Player.inventory[_savedSelectedItem];
        Player.inventory[_savedSelectedItem] = offhand;
        _releaseRequested = false;
        IsUsingOffhand = true;
    }

    private void StopUsingOffhand()
    {
        if (!IsUsingOffhand)
            return;

        if (_savedSelectedItem >= 0 && _savedSelectedItem < Player.inventory.Length)
            Player.inventory[_savedSelectedItem] = _savedMainHand;

        _savedMainHand = null;
        _savedSelectedItem = -1;
        _releaseRequested = false;
        IsUsingOffhand = false;
    }
}

public static class OffhandSlotHelper
{
    public static bool TryGetItem(Player player, out Item item)
    {
        item = null;
        if (player is null)
            return false;

        Item storedItem = LoaderManager.Get<AccessorySlotLoader>()
            .Get(ModContent.GetInstance<OffhandSlot>().Type, player).FunctionalItem;
        if (storedItem is null || storedItem.IsAir)
            return false;

        item = storedItem;
        return true;
    }
}
