using Phthonos.Common.EquipmentDisplay.Profiles;
using Phthonos.Common.EquipmentDisplay.State;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Phthonos.Common.EquipmentDisplay.Drawing;

// 肩扛在頭部之前繪製，身體不會把整段刀身遮住。
public sealed class EquipmentShoulderDrawLayer : PlayerDrawLayer
{
    public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.Head);

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        if (drawInfo.shadow != 0f)
            return;
        var layout = drawInfo.drawPlayer.GetModPlayer<CombatEquipmentTracker>().GetResolvedLayout();
        foreach (var weapon in layout.PassiveDraws)
            if (weapon.Style == WeaponCarryStyle.Shoulder)
                EquipmentPassiveDrawLayer.DrawWeapon(ref drawInfo, weapon);
    }
}
