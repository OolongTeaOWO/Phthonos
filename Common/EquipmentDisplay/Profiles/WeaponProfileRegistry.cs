using System.Collections.Generic;

namespace Phthonos.Common.EquipmentDisplay.Profiles;

/// <summary>供本模組與相容模組以物品類型指定顯示標籤。</summary>
public static class WeaponProfileRegistry
{
    private static readonly Dictionary<int, WeaponProfile> Profiles = new();

    public static void Register(int itemType, WeaponCarryStyle carryStyle, bool canUseDragFallback = false)
    {
        Profiles[itemType] = new WeaponProfile(carryStyle, canUseDragFallback);
    }

    public static bool TryGet(int itemType, out WeaponProfile profile) => Profiles.TryGetValue(itemType, out profile);

    internal static void Clear() => Profiles.Clear();
}
