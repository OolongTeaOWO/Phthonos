using Microsoft.Xna.Framework;
using Phthonos.Common.Configs;
using Phthonos.Common.EquipmentDisplay.Profiles;
using Terraria;

namespace Phthonos.Common.EquipmentDisplay.Layouts;

/// <summary>把姿勢設定轉換成符合玩家面向與重力方向的實際繪製資料。</summary>
public static class EquipmentPoseFactory
{
    public static ResolvedWeaponDraw Create(Player player, Item item, WeaponCarryStyle style)
    {
        EquipmentPoseSettings settings = EquipmentPoseConfig.Get(style);
        float facing = player.direction;
        float gravity = player.gravDir;
        Vector2 offset = new(settings.OffsetX * facing, settings.OffsetY * gravity);
        // 先組成面向右、正常重力的完整角度，再鏡像整個姿勢。
        // 只反轉其中一段角度，會使自訂基準角度下的刀尖與護手不再對稱。
        float rightFacingRotation = settings.BaseRotationDegrees + settings.FacingRotationDegrees;
        float rotationDegrees = rightFacingRotation * facing * gravity;
        float originX = facing == 1f ? settings.OriginX : 1f - settings.OriginX;
        float originY = gravity == 1f ? settings.OriginY : 1f - settings.OriginY;
        Vector2 originRatio = new(originX, originY);

        return new ResolvedWeaponDraw(item, offset, rotationDegrees, settings.Scale, originRatio, style);
    }
}
