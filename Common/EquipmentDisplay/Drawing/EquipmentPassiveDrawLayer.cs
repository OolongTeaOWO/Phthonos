using Microsoft.Xna.Framework;
using Phthonos.Common.EquipmentDisplay.Layouts;
using Phthonos.Common.EquipmentDisplay.State;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Phthonos.Common.EquipmentDisplay.Profiles;

namespace Phthonos.Common.EquipmentDisplay.Drawing;

public sealed class EquipmentPassiveDrawLayer : PlayerDrawLayer
{
    // 背掛位於腰掛前方，但仍由角色皮膚覆蓋。
    public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.Skin);

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        if (drawInfo.shadow != 0f)
            return;
        ResolvedEquipmentLayout layout = drawInfo.drawPlayer.GetModPlayer<CombatEquipmentTracker>().GetResolvedLayout();
        foreach (ResolvedWeaponDraw weaponDraw in layout.PassiveDraws)
            if (weaponDraw.Style != WeaponCarryStyle.Waist && weaponDraw.Style != WeaponCarryStyle.Shoulder)
                DrawWeapon(ref drawInfo, weaponDraw);
    }

    internal static void DrawWeapon(ref PlayerDrawSet drawInfo, ResolvedWeaponDraw weaponDraw)
    {
        Player player = drawInfo.drawPlayer;
        Vector2 position = drawInfo.Center - Main.screenPosition + weaponDraw.Offset;
        float rotation = MathHelper.ToRadians(weaponDraw.RotationDegrees);
        if (EquipmentDrawHelper.TryCreateDrawData(player, weaponDraw.Item, position, rotation, weaponDraw.Scale, weaponDraw.OriginRatio, EquipmentDrawHelper.GetSpriteEffects(player), out DrawData drawData))
            drawInfo.DrawDataCache.Add(drawData);
    }
}
