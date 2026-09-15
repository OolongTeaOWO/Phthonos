using Terraria;

namespace Phthonos.Common.EquipmentDisplay.Layouts;

public sealed class EquipmentDisplayState
{
    public EquipmentDisplayState(Player player, Item mainHand, Item offhand, bool isUsingMainHand, bool isUsingOffhand)
    {
        Player = player;
        MainHand = mainHand;
        Offhand = offhand;
        IsUsingMainHand = isUsingMainHand;
        IsUsingOffhand = isUsingOffhand;
    }

    public Player Player { get; }

    public Item MainHand { get; }

    public Item Offhand { get; }

    public bool IsUsingMainHand { get; }

    public bool IsUsingOffhand { get; }
}
