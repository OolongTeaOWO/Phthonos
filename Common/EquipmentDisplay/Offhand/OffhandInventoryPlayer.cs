using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Phthonos.Common.EquipmentDisplay.Offhand;

/// <summary>保存唯一的副手物品；不再借用飾品欄，所以沒有外觀欄與染料欄。</summary>
public sealed class OffhandInventoryPlayer : ModPlayer
{
    private const string SaveKey = "OffhandItem";
    private Item _offhandItem = new();

    public Item OffhandItem => _offhandItem;

    public override void Initialize()
    {
        _offhandItem = new Item();
        _offhandItem.TurnToAir();
    }

    public override void SaveData(TagCompound tag)
    {
        if (!_offhandItem.IsAir)
            tag[SaveKey] = _offhandItem;
    }

    public override void LoadData(TagCompound tag)
    {
        _offhandItem = tag.ContainsKey(SaveKey) ? tag.Get<Item>(SaveKey) : new Item();
        if (_offhandItem is null)
        {
            _offhandItem = new Item();
            _offhandItem.TurnToAir();
        }
    }

    // 保留舊獨立欄存檔；目的欄已有物品時，暫存物品繼續保存，絕不覆蓋。
    public override void PostUpdate()
    {
        if (_offhandItem is null || _offhandItem.IsAir)
            return;
        var slot = LoaderManager.Get<AccessorySlotLoader>().Get(ModContent.GetInstance<OffhandSlot>().Type, Player);
        if (!slot.FunctionalItem.IsAir)
            return;
        slot.FunctionalItem = _offhandItem;
        _offhandItem = new Item();
    }
}
