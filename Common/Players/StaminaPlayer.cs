using System;
using Terraria.ModLoader;

namespace Phthonos.Common.Players;

public sealed class StaminaPlayer : ModPlayer
{
    // 耐力手感：上限、翻滾消耗、每秒恢復量、消耗後等待的遊戲刻數（60刻=1秒）。
    public const float Maximum = 100f;
    public const float RollCost = 25f;
    public const float RecoveryPerSecond = 15f;
    public const int RecoveryDelayTicks = 48;

    public float Current { get; private set; } = Maximum;
    public float Fraction => Math.Clamp(Current / Maximum, 0f, 1f);
    private int recoveryDelay;

    public override void Initialize() => Restore();
    public override void UpdateDead() => Restore();
    public override void OnRespawn() => Restore();

    public override void PreUpdate()
    {
        if (Player.dead)
            return;

        if (recoveryDelay > 0)
        {
            recoveryDelay--;
            return;
        }

        Current = Math.Min(Maximum, Current + RecoveryPerSecond / 60f);
    }

    public bool TryConsume(float amount)
    {
        if (amount <= 0f || !float.IsFinite(amount) || Current < amount)
            return false;

        Current -= amount;
        recoveryDelay = RecoveryDelayTicks;
        return true;
    }

    private void Restore()
    {
        Current = Maximum;
        recoveryDelay = 0;
    }
}
