using Microsoft.Xna.Framework;
using Phthonos.Common.EquipmentDisplay.Layouts;
using Phthonos.Common.EquipmentDisplay.State;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Phthonos.Common.EquipmentDisplay.Drawing;

public sealed class EquipmentHandDrawLayer : PlayerDrawLayer
{
    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.HeldItem);

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        if (drawInfo.shadow != 0f)
            return;
        ResolvedEquipmentLayout layout = drawInfo.drawPlayer.GetModPlayer<CombatEquipmentTracker>().GetResolvedLayout();
        foreach (ResolvedWeaponDraw weaponDraw in layout.HandDraws)
            DrawWeapon(ref drawInfo, weaponDraw);
    }

    private static void DrawWeapon(ref PlayerDrawSet drawInfo, ResolvedWeaponDraw weaponDraw)
    {
        Player player = drawInfo.drawPlayer;
        Vector2 position = drawInfo.Center - Main.screenPosition + weaponDraw.Offset;
        float rotation = MathHelper.ToRadians(weaponDraw.RotationDegrees);
        if (EquipmentDrawHelper.TryCreateDrawData(player, weaponDraw.Item, position, rotation, weaponDraw.Scale, weaponDraw.OriginRatio, EquipmentDrawHelper.GetSpriteEffects(player), out DrawData drawData))
            drawInfo.DrawDataCache.Add(drawData);
    }
}
