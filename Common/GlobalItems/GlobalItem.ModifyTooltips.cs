using System.Collections.Generic;
using Phthonos.Common.EquipmentDisplay.Profiles;
using Terraria;
using Terraria.ModLoader;

namespace Phthonos.Common.GlobalItems;

public class EquipmentDisplayGlobalItem : GlobalItem
{
    public override void ModifyTooltips(
        Item item,
        List<TooltipLine> tooltips)
    {
        if (item == null || item.IsAir)
            return;

        if (WeaponProfileResolver.TryResolve(item, out WeaponProfile profile))
        {
            tooltips.Add(
                new TooltipLine(
                    Mod,
                    "EquipmentDisplayDebug",
                    $"顯示分類：{profile.CarryStyle}"
                )
            );
        }
        else
        {
            tooltips.Add(
                new TooltipLine(
                    Mod,
                    "EquipmentDisplayDebug",
                    "顯示分類：未支援"
                )
            );
        }
    }
}
