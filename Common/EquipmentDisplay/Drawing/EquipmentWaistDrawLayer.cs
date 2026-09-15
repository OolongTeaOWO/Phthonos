using Phthonos.Common.EquipmentDisplay.Profiles;
using Phthonos.Common.EquipmentDisplay.State;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Phthonos.Common.EquipmentDisplay.Drawing;

// 腰掛先於背掛繪製，使背掛位於腰掛前方；兩者仍在角色身體後方。
public sealed class EquipmentWaistDrawLayer : PlayerDrawLayer
{
    public override Position GetDefaultPosition() => new BeforeParent(ModContent.GetInstance<EquipmentPassiveDrawLayer>());

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        if (drawInfo.shadow != 0f)
            return;
        var layout = drawInfo.drawPlayer.GetModPlayer<CombatEquipmentTracker>().GetResolvedLayout();
        foreach (var weapon in layout.PassiveDraws)
            if (weapon.Style == WeaponCarryStyle.Waist)
                EquipmentPassiveDrawLayer.DrawWeapon(ref drawInfo, weapon);
    }
}
