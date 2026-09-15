using Terraria;
using Terraria.ModLoader;

namespace Phthonos.Common.EquipmentDisplay.Offhand;

public sealed class OffhandSlot : ModAccessorySlot
{
    public override string FunctionalTexture => "Phthonos/Assets/UI/ShieldSlot";
    public override bool DrawVanitySlot => false;
    public override bool DrawDyeSlot => false;
    public override bool HasEquipmentLoadoutSupport => false;
    public override bool CanAcceptItem(Item item, AccessorySlotType context) =>
        context == AccessorySlotType.FunctionalSlot && item is not null && !item.IsAir &&
        (item.damage > 0 || item.shieldSlot > 0) &&
        !Player.GetModPlayer<OffhandUseController>().IsUsingOffhand;
    public override void ApplyEquipEffects() { }
    public override void OnMouseHover(AccessorySlotType context) => Main.hoverItemName = "副手";
}
