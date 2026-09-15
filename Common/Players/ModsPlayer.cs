using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Phthonos.Common.Players;

public class ModsPlayer : ModPlayer
{
    public Vector2 MouseDirection { get; private set; }

    public override void PreUpdateMovement()
    {
        Vector2 direction = Main.MouseWorld - Player.Center;

        if (direction == Vector2.Zero)
            return;

        MouseDirection = Vector2.Normalize(direction);
        Player.direction = MouseDirection.X >= 0f ? 1 : -1;
    }
}