using Microsoft.Xna.Framework;
using Terraria;
using Phthonos.Common.EquipmentDisplay.Profiles;

namespace Phthonos.Common.EquipmentDisplay.Layouts;

public sealed class ResolvedWeaponDraw
{
    public ResolvedWeaponDraw(Item item, Vector2 offset, float rotationDegrees, float scale, Vector2 originRatio, WeaponCarryStyle style)
    {
        Item = item;
        Style = style;
        Offset = offset;
        RotationDegrees = rotationDegrees;
        Scale = scale;
        OriginRatio = originRatio;
    }

    public Item Item { get; }
    public WeaponCarryStyle Style { get; }

    public Vector2 Offset { get; }

    public float RotationDegrees { get; }

    public float Scale { get; }

    /// <summary>貼圖內的定位點；(0, 0) 是左上，(1, 1) 是右下。</summary>
    public Vector2 OriginRatio { get; }
}
