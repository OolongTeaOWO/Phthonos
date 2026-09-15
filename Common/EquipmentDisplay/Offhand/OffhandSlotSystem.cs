using System.Reflection;
using MonoMod.RuntimeDetour;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace Phthonos.Common.EquipmentDisplay.Offhand;

// 只允許本模組副手欄接收武器，不影響其他模組的飾品欄。
public sealed class OffhandSlotSystem : ModSystem
{
    private Hook _movementHook;
    private delegate int OriginalMovement(Item[] inventory, int context, int slot, Item item);
    public override void Load()
    {
        MethodInfo method = typeof(ItemSlot).GetMethod("PickItemMovementAction", BindingFlags.Public | BindingFlags.Static);
        if (method is not null)
            _movementHook = new Hook(method, MoveItem);
    }
    public override void Unload()
    {
        _movementHook?.Dispose();
        _movementHook = null;
    }
    private static int MoveItem(OriginalMovement original, Item[] inventory, int context, int slot, Item item)
    {
        if (context == -10 && slot == ModContent.GetInstance<OffhandSlot>().Type &&
            item is not null && !item.IsAir &&
            LoaderManager.Get<AccessorySlotLoader>().CanAcceptItem(slot, item, context))
            return 1;
        return original(inventory, context, slot, item);
    }
}
