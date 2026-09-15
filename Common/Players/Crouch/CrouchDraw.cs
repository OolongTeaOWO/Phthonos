using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Phthonos.Common.Players.Crouch;

public class CrouchDraw : PlayerDrawLayer
{
    private const float MaxCrouchTime = 300f;

    private const string StealthMeterPath = "Phthonos/Assets/UI/StealthMeter";
    private const string StealthBarPath = "Phthonos/Assets/UI/StealthBar";

    private static Asset<Texture2D> stealthMeter;
    private static Asset<Texture2D> stealthBar;

    public override Position GetDefaultPosition()
    {
        return new AfterParent(PlayerDrawLayers.Torso);
    }

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
    {
        Crouch crouch = drawInfo.drawPlayer.GetModPlayer<Crouch>();
        return crouch.IsCrouching && crouch.CrouchTime > 0;
    }

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        Crouch crouch = drawInfo.drawPlayer.GetModPlayer<Crouch>();

        if (!crouch.IsCrouching || crouch.CrouchTime <= 0)
            return;

        LoadAssets();

        Vector2 position = drawInfo.drawPlayer.Bottom + new Vector2(0f, 12f) - Main.screenPosition;
        position.X -= stealthMeter.Width() / 2f;

        DrawMeter(ref drawInfo, position);
        DrawBar(ref drawInfo, position, crouch.CrouchTime);
    }

    private static void LoadAssets()
    {
        stealthMeter ??= ModContent.Request<Texture2D>(StealthMeterPath);
        stealthBar ??= ModContent.Request<Texture2D>(StealthBarPath);
    }

    private static void DrawMeter(ref PlayerDrawSet drawInfo, Vector2 position)
    {
        drawInfo.DrawDataCache.Add(new DrawData(
            stealthMeter.Value,
            position,
            null,
            Color.White,
            0f,
            Vector2.Zero,
            1f,
            SpriteEffects.None,
            0
        ));
    }

    private static void DrawBar(ref PlayerDrawSet drawInfo, Vector2 position, int crouchTime)
    {
        float progress = MathHelper.Clamp(crouchTime / MaxCrouchTime, 0f, 1f);
        int width = (int)(stealthBar.Width() * progress);

        if (width <= 0)
            return;

        Rectangle sourceRectangle = new(0, 0, width, stealthBar.Height());

        drawInfo.DrawDataCache.Add(new DrawData(
            stealthBar.Value,
            position,
            sourceRectangle,
            Color.White,
            0f,
            Vector2.Zero,
            1f,
            SpriteEffects.None,
            0
        ));
    }

    public static bool IsStealthDrawData(DrawData drawData)
    {
        LoadAssets();

        Texture2D texture = drawData.texture;
        return texture == stealthMeter.Value || texture == stealthBar.Value;
    }
}
