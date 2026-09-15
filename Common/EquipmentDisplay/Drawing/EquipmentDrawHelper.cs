using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;

namespace Phthonos.Common.EquipmentDisplay.Drawing;

public static class EquipmentDrawHelper
{
    public static bool TryCreateDrawData(Player player, Item item, Vector2 position, float rotation, float scale, Vector2 originRatio, SpriteEffects effects, out DrawData drawData)
    {
        drawData = default;
        if (player is null || item is null || item.IsAir)
            return false;

        Main.instance.LoadItem(item.type);
        if (TextureAssets.Item[item.type] is null)
            return false;

        Texture2D texture = TextureAssets.Item[item.type].Value;
        Rectangle frame = GetItemFrame(item, texture);
        Vector2 origin = frame.Size() * originRatio;
        Color color = Lighting.GetColor(player.Center.ToTileCoordinates());
        drawData = new DrawData(texture, position.Floor(), frame, color, rotation, origin, scale, effects, 0);
        return true;
    }

    public static SpriteEffects GetSpriteEffects(Player player)
    {
        SpriteEffects effects = SpriteEffects.None;
        if (player.direction == -1)
            effects |= SpriteEffects.FlipHorizontally;
        if (player.gravDir == -1f)
            effects |= SpriteEffects.FlipVertically;
        return effects;
    }

    private static Rectangle GetItemFrame(Item item, Texture2D texture)
    {
        var animation = Main.itemAnimations[item.type];
        return animation is not null && animation.FrameCount > 1
            ? texture.Frame(1, animation.FrameCount, 0, 0)
            : texture.Frame();
    }
}
