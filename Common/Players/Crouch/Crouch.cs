using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using Phthonos.Common.Systems;

namespace Phthonos.Common.Players.Crouch;

public class Crouch : ModPlayer
{
    private const int MaxCrouchTime = 300;
    private const int CrouchCooldown = 240;

    // 潛行狀態
    public bool IsCrouching;
    // 潛行剩餘時間
    internal int CrouchTime;
    // 潛行冷卻時間
    internal int cooldown;

    // 警戒音效
    private static readonly SoundStyle WarningSound =
        new SoundStyle("Phthonos/Assets/UI/Sound_effects/warning");

    public override void PreUpdateMovement()
    {
        if (cooldown > 0)
            cooldown--;

        if (IsCrouching)
        {
            CrouchTime--;

            if (CrouchTime <= 0)
            {
                CrouchTime = 0;
                IsCrouching = false;
                cooldown = CrouchCooldown;
            }

            return;
        }

        if (cooldown > 0)
            return;

        if (KeybindSystem.Crouch.JustPressed)
        {
            IsCrouching = true;
            CrouchTime = MaxCrouchTime;

            SoundEngine.PlaySound(
                WarningSound,
                Player.Center
            );
        }
    }
}
