using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Phthonos.Common.EquipmentDisplay.Profiles;

public static class WeaponProfileResolver
{
    public static bool TryResolve(Item item, out WeaponProfile profile)
    {
        profile = null;
        if (item is null || item.IsAir)
            return false;

        if (WeaponProfileRegistry.TryGet(item.type, out profile))
            return true;

        // 泰拉魔刃與 Arkhalis 透過投射物攻擊，但待機外觀仍是短刃。
        if (item.type == ItemID.Terragrim || item.type == ItemID.Arkhalis)
        {
            profile = new WeaponProfile(WeaponCarryStyle.Waist);
            return true;
        }

        if (item.shieldSlot > 0)
        {
            profile = new WeaponProfile(WeaponCarryStyle.ShieldBack);
            return true;
        }

        if (item.CountsAsClass(DamageClass.Throwing))
        {
            profile = new WeaponProfile(WeaponCarryStyle.DualHands);
            return true;
        }

        if (ItemID.Sets.Yoyo[item.type])
        {
            profile = new WeaponProfile(WeaponCarryStyle.Drag);
            return true;
        }

        // 弓垂直收在背上；槍械以肩扛為預設。個別模組武器仍可用 Registry 覆寫。
        if (item.CountsAsClass(DamageClass.Ranged))
        {
            profile = item.useAmmo == AmmoID.Arrow
                ? new WeaponProfile(WeaponCarryStyle.Back)
                : new WeaponProfile(WeaponCarryStyle.Shoulder, true);
            return true;
        }

        // 迴旋鏢屬於近戰投射物，非攻擊時以背掛處理；需要特殊弧形角度的個體可登錄覆寫。
        if (item.CountsAsClass(DamageClass.Melee) && item.noUseGraphic && item.shoot > ProjectileID.None)
        {
            profile = new WeaponProfile(WeaponCarryStyle.Back);
            return true;
        }

        if (!item.CountsAsClass(DamageClass.Melee) && !item.CountsAsClass(DamageClass.Magic))
            return false;

        int longestSide = item.width > item.height ? item.width : item.height;
        if (longestSide <= 32)
            profile = new WeaponProfile(WeaponCarryStyle.Waist);
        else if (longestSide <= 60)
            profile = new WeaponProfile(WeaponCarryStyle.Shoulder, true);
        else
            profile = new WeaponProfile(WeaponCarryStyle.Back);

        return true;
    }
}
