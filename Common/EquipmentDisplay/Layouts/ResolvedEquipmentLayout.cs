using System.Collections.Generic;

namespace Phthonos.Common.EquipmentDisplay.Layouts;

public sealed class ResolvedEquipmentLayout
{
    public List<ResolvedWeaponDraw> PassiveDraws { get; } = new();

    public List<ResolvedWeaponDraw> HandDraws { get; } = new();
}
