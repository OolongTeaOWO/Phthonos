using Terraria;
using Terraria.ModLoader;
using Phthonos.Common.Systems;

namespace Phthonos.Common.Players.Roll;

public class Roll : ModPlayer
{
    public bool IsRolling;
    public bool IsRollingBackward;

    internal int rollTime;
    internal int cooldown;

    internal const int RollDuration = 22;
    internal const int RollCooldown = 70;
    internal const float RollSpeed = 7f;

    private float rollDirection;

    public override void PreUpdateMovement()
    {
        if (Player.dead)
        {
            IsRolling = false;
            rollTime = 0;
            cooldown = 0;
            return;
        }

        if (cooldown > 0)
            cooldown--;

        if (IsRolling)
        {
            Player.velocity.X = rollDirection * RollSpeed;
            rollTime--;

            if (rollTime <= 0)
                IsRolling = false;

            return;
        }

        if (Player.whoAmI == Main.myPlayer && KeybindSystem.Roll.JustPressed && cooldown <= 0 &&
            Player.GetModPlayer<StaminaPlayer>().TryConsume(StaminaPlayer.RollCost))
        {
            IsRolling = true;
            rollTime = RollDuration;
            cooldown = RollCooldown;

            SetRollDirection();
            Player.velocity.X = rollDirection * RollSpeed;
        }
    }

    private void SetRollDirection()
    {
        if (Player.controlLeft)
        {
            rollDirection = -1f;
            IsRollingBackward = Player.direction != -1;
        }
        else if (Player.controlRight)
        {
            rollDirection = 1f;
            IsRollingBackward = Player.direction != 1;
        }
        else
        {
            rollDirection = Player.direction;
            IsRollingBackward = false;
        }
    }
}
