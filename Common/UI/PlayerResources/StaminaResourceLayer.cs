using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Phthonos.Common.Players;
using System;

namespace Phthonos.Common.UI.PlayerResources;

/// <summary>
/// Draws the player's current stamina beneath Terraria's horizontal mana bar.
/// </summary>
public sealed class StaminaResourceLayer : ModSystem
{
    private const string VanillaBarsPath = "Images/UI/PlayerResourceSets/HorizontalBars/";
    private const string ModBarsPath = "Phthonos/Assets/UI/HorizontalBars/";
    // Horizontal Bars compress 100 points into three visible panels.
    private const int StaminaPerSegment = 40;
    private const int SegmentWidth = 12;
    private const int PanelHeight = 24;
    private const int FillVerticalOffset = 6;
    // SP_Panel_Right: bottle starts at x=26; the lower rail starts at y=24.
    private const int RightPanelFillEnd = 26;
    private const int RightPanelFillBottom = 24;
    private const int StaminaHorizontalOffset = 3;
    private const int StaminaVerticalOffset = 30;
    // 藥瓶內圖示相對 SP_Panel_Right 左上角的位置（原尺寸繪製）。
    private static readonly Vector2 EnergyFillOffset = new(30f, 4f);

    private Asset<Texture2D> panelLeft;
    private Asset<Texture2D> panelMiddle;
    private Asset<Texture2D> panelRight;
    private Asset<Texture2D> fill;
    private Asset<Texture2D> energyFill;

    public override void Load()
    {
        if (Main.dedServ)
            return;

        panelLeft = Main.Assets.Request<Texture2D>(VanillaBarsPath + "Panel_Left");
        panelMiddle = ModContent.Request<Texture2D>(ModBarsPath + "SP_Panel_Middle");
        panelRight = ModContent.Request<Texture2D>(ModBarsPath + "SP_Panel_Right");
        fill = ModContent.Request<Texture2D>(ModBarsPath + "SP_Fill");
        energyFill = ModContent.Request<Texture2D>(ModBarsPath + "Energy_Fill");
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        int resourceBarIndex = layers.FindIndex(layer => layer.Name == "Vanilla: Resource Bars");
        if (resourceBarIndex < 0)
            return;

        layers.Insert(resourceBarIndex + 1, new LegacyGameInterfaceLayer(
            "Phthonos: Stamina Resource",
            DrawStaminaBar,
            InterfaceScaleType.UI));
    }

    public override void Unload()
    {
        panelLeft = null;
        panelMiddle = null;
        panelRight = null;
        fill = null;
        energyFill = null;
    }

    private bool DrawStaminaBar()
    {
        if (Main.gameMenu || Main.LocalPlayer is null || !Main.LocalPlayer.active)
            return true;

        if (Main.ResourceSetsManager.ActiveSetKeyName is not
            ("HorizontalBars" or "HorizontalBarsWithText" or "HorizontalBarsWithFullText"))
            return true;

        if (!PhthonosResourceOverlay.HasManaMiddlePosition ||
            !PhthonosResourceOverlay.HasManaRightPosition ||
            panelLeft is null || panelMiddle is null || panelRight is null || fill is null || energyFill is null)
        {
            return true;
        }

        Texture2D leftTexture = panelLeft.Value;
        Texture2D middleTexture = panelMiddle.Value;
        Texture2D rightTexture = panelRight.Value;
        Texture2D fillTexture = fill.Value;

        StaminaPlayer stamina = Main.LocalPlayer.GetModPlayer<StaminaPlayer>();
        int segmentCount = (int)Math.Ceiling(StaminaPlayer.Maximum / StaminaPerSegment);
        int filledPixels = (int)Math.Round(segmentCount * SegmentWidth * stamina.Fraction);
        Vector2 firstMiddle = PhthonosResourceOverlay.ManaMiddlePosition;
        float vanillaRightEdge = PhthonosResourceOverlay.ManaRightPosition.X +
            PhthonosResourceOverlay.ManaRightWidth + StaminaHorizontalOffset;
        Vector2 rightPosition = new(
            vanillaRightEdge - rightTexture.Width,
            firstMiddle.Y + StaminaVerticalOffset - (rightTexture.Height - PanelHeight) / 2f);

        // Align the full fill strip with the opening before the bottle.
        // Keep the bottle anchored while deriving the panels from its actual rail.
        Vector2 middleStart = new(
            rightPosition.X + RightPanelFillEnd - segmentCount * SegmentWidth,
            rightPosition.Y + RightPanelFillBottom - fillTexture.Height - FillVerticalOffset);
        Vector2 barStart = new(middleStart.X - leftTexture.Width, middleStart.Y);
        SpriteBatch spriteBatch = Main.spriteBatch;

        spriteBatch.Draw(leftTexture, barStart, Color.White);

        for (int segment = 0; segment < segmentCount; segment++)
        {
            Vector2 panelPosition = new(middleStart.X + segment * SegmentWidth, middleStart.Y);
            spriteBatch.Draw(middleTexture, panelPosition, Color.White);
            // 消耗從左側尾端開始，剩餘耐力固定貼齊右側藥瓶。
            int width = Math.Clamp(filledPixels - (segmentCount - 1 - segment) * SegmentWidth, 0, SegmentWidth);
            int sourceX = SegmentWidth - width;
            if (width > 0)
                spriteBatch.Draw(fillTexture, panelPosition + new Vector2(sourceX, FillVerticalOffset),
                    new Rectangle(sourceX, 0, width, fillTexture.Height), Color.White);
        }

        spriteBatch.Draw(rightTexture, rightPosition, Color.White);
        // 瓶框含不透明底色，因此內圖示必須畫在瓶框之後。
        spriteBatch.Draw(energyFill.Value, rightPosition + EnergyFillOffset, Color.White);
        return true;
    }
}
