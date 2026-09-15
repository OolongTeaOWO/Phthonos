using Terraria.ModLoader;
using Phthonos.Common.EquipmentDisplay.Profiles;
using Phthonos.Common.Systems;

namespace Phthonos;

public class Phthonos : Mod
{
    public override void Load()
    {
        KeybindSystem.Load(this);
    }

    public override void Unload()
    {
        WeaponProfileRegistry.Clear();
        KeybindSystem.Unload();
    }
}
