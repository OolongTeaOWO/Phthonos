using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace Phthonos.Common.UI.PlayerResources;

/// <summary>
/// Replaces selected pieces of Terraria's horizontal resource bars and records
/// the mana bar position so the stamina bar can follow the active UI scale.
/// </summary>
public sealed class PhthonosResourceOverlay : ModResourceOverlay
{
    private const string VanillaBarsPath = "Images/UI/PlayerResourceSets/HorizontalBars/";
    private const string ModBarsPath = "Phthonos/Assets/UI/HorizontalBars/";

    private readonly Dictionary<string, Asset<Texture2D>> vanillaAssets = new();
    private Asset<Texture2D> manaPanelRight;
    private static ulong manaPositionFrame;

    internal static Vector2 ManaMiddlePosition { get; private set; }
    internal static Vector2 ManaRightPosition { get; private set; }
    internal static int ManaRightWidth { get; private set; }
    internal static bool HasManaMiddlePosition { get; private set; }
    internal static bool HasManaRightPosition { get; private set; }

    public override bool PreDrawResource(ResourceOverlayDrawContext context)
    {
        if (IsVanillaAsset(context.texture, VanillaBarsPath + "MP_Panel_Middle"))
        {
            if (manaPositionFrame != Main.GameUpdateCount)
            {
                manaPositionFrame = Main.GameUpdateCount;
                HasManaMiddlePosition = false;
            }

            if (!HasManaMiddlePosition || context.position.X < ManaMiddlePosition.X)
                ManaMiddlePosition = context.position;

            HasManaMiddlePosition = true;
            return true;
        }

        if (!IsVanillaAsset(context.texture, VanillaBarsPath + "MP_Panel_Right"))
            return true;

        ManaRightPosition = context.position;
        ManaRightWidth = context.source?.Width ?? context.texture.Width();
        HasManaRightPosition = true;

        manaPanelRight ??= ModContent.Request<Texture2D>(ModBarsPath + "MP_Panel_Right");
        context.texture = manaPanelRight;
        context.source = manaPanelRight.Frame();
        context.Draw();
        return false;
    }

    public override void Unload()
    {
        vanillaAssets.Clear();
        manaPanelRight = null;
        ManaMiddlePosition = Vector2.Zero;
        ManaRightPosition = Vector2.Zero;
        ManaRightWidth = 0;
        HasManaMiddlePosition = false;
        HasManaRightPosition = false;
        manaPositionFrame = 0;
    }

    private bool IsVanillaAsset(Asset<Texture2D> asset, string path)
    {
        if (!vanillaAssets.TryGetValue(path, out Asset<Texture2D> comparison))
        {
            comparison = Main.Assets.Request<Texture2D>(path);
            vanillaAssets[path] = comparison;
        }

        return asset == comparison;
    }
}
