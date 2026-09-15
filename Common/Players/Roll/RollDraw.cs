using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Phthonos.Common.Players.Crouch;
using RollPlayer = Phthonos.Common.Players.Roll.Roll;

namespace Phthonos.Common.Players.Roll;

public class RollDraw : ModPlayer
{
    public override void TransformDrawData(ref PlayerDrawSet drawInfo)
    {
        RollPlayer roll = Player.GetModPlayer<RollPlayer>();

        if (!roll.IsRolling)
            return;

        float progress = 1f - roll.rollTime / (float)RollPlayer.RollDuration;
        float rotationDirection = roll.IsRollingBackward ? -1f : 1f;
        float rotation =
            MathHelper.TwoPi *
            progress *
            Player.direction *
            rotationDirection;
        Vector2 rotationCenter = Player.MountedCenter - Main.screenPosition;

        for (int i = 0; i < drawInfo.DrawDataCache.Count; i++)
        {
            DrawData drawData = drawInfo.DrawDataCache[i];

            // 潜行条不参与翻滚旋转
            if (CrouchDraw.IsStealthDrawData(drawData))
                continue;

            Vector2 offsetFromPlayer = drawData.position - rotationCenter;
            drawData.position = rotationCenter + offsetFromPlayer.RotatedBy(rotation);
            drawData.rotation += rotation;
            drawInfo.DrawDataCache[i] = drawData;
        }
    }
}
