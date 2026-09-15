namespace Phthonos.Common.EquipmentDisplay.Profiles;

/// <summary>武器的顯示標籤；先辨識標籤，之後才由布局器決定是否繪製。</summary>
public sealed class WeaponProfile
{
    public WeaponProfile(WeaponCarryStyle carryStyle, bool canUseDragFallback = false)
    {
        CarryStyle = carryStyle;
        CanUseDragFallback = canUseDragFallback;
    }

    public WeaponCarryStyle CarryStyle { get; }

    public bool CanUseDragFallback { get; }
}
