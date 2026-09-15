using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Phthonos.Common.Players.Crouch;
using RollPlayer = Phthonos.Common.Players.Roll.Roll;

namespace Phthonos.Common.Players;

public class MovementDraw : ModPlayer
{
    private const float MaxLeanAngle = 9f;
    private float leanRotation;

    public override void TransformDrawData(ref PlayerDrawSet drawInfo)
    {
        if (Player.GetModPlayer<RollPlayer>().IsRolling)
            return;

        float targetRotation = 0f;

        if (Player.velocity.X < -0.1f)
            targetRotation = -MathHelper.ToRadians(MaxLeanAngle);
        else if (Player.velocity.X > 0.1f)
            targetRotation = MathHelper.ToRadians(MaxLeanAngle);

        leanRotation = MathHelper.Lerp(
            leanRotation,
            targetRotation,
            0.2f
        );

        if (Math.Abs(leanRotation) < 0.001f)
            return;

        Vector2 rotationCenter = Player.MountedCenter - Main.screenPosition;

        for (int i = 0; i < drawInfo.DrawDataCache.Count; i++)
        {
            DrawData drawData = drawInfo.DrawDataCache[i];

            if (CrouchDraw.IsStealthDrawData(drawData))
                continue;

            Vector2 offsetFromPlayer = drawData.position - rotationCenter;
            drawData.position = rotationCenter + offsetFromPlayer.RotatedBy(leanRotation);
            drawData.rotation += leanRotation;
            drawInfo.DrawDataCache[i] = drawData;
        }
    }
}
