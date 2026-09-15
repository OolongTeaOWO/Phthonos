using Phthonos.Common.EquipmentDisplay.Profiles;

namespace Phthonos.Common.Configs;

/// <summary>
/// 所有武器待機姿勢的調整數值集中在這裡。
///
/// 參數調整方式：
/// OffsetX：水平位置。增加會往角色面朝方向移動；減少會往角色背後移動。
/// OffsetY：垂直位置。增加會往腳部移動；減少會往頭部移動，倒轉重力時會自動反向。
/// BaseRotationDegrees：面向右時的基準角度。
/// FacingRotationDegrees：加在基準角度上的調整量（保留原參數名稱）。
/// 面向右的最終角度 = 兩個角度相加；面向左時將整個結果反號。
/// 兩個角度都以面向右、正常重力為基準，不需另外計算左側姿勢。
/// Scale：武器貼圖大小。1 是原尺寸，0.85 是 85%，數值越大武器越大。
/// OriginX：水平定位點，範圍通常是 0 到 1。0 是貼圖左側，0.5 是中央，1 是右側。
/// OriginY：垂直定位點，範圍通常是 0 到 1。0 是貼圖頂端，0.5 是中央，1 是底端。
/// Origin 定位點會固定在 Offset 指定的位置；調整它可將握柄、護手或貼圖中心固定在角色身上。
/// Origin 會隨角色左右與重力方向同步鏡像，不需要另外設定翻轉開關。
/// </summary>
public static class EquipmentPoseConfig
{
    // 【無姿勢／後備值】物品沒有有效姿勢時使用，正常情況不會被繪製。
    public static readonly EquipmentPoseSettings None = new(
        offsetX: 0f,
        offsetY: 0f,
        baseRotationDegrees: 0f,
        facingRotationDegrees: 0f,
        scale: 0.85f,
        originX: 0.5f,
        originY: 0.5f);

    // 【腰掛姿勢】小型刀劍、短刃等近戰武器固定在腰部；定位點預設靠近握柄。
    public static readonly EquipmentPoseSettings Waist = new(
        offsetX: 2f,                   // ScourgeMod 基準：略往角色面朝方向移動。
        offsetY: 6f,                   // 垂直位置：越大越靠近腿部。
        baseRotationDegrees: 0f,       // 腰掛沒有額外固定旋轉。
        facingRotationDegrees: -150f,  // 面向右為 -150 度，面向左為 +150 度。
        scale: 1f,                     // 使用物品貼圖原始大小。
        originX: 0.5f,                 // 腰掛使用貼圖正中央，確保左右露出量一致。
        originY: 0.5f);

    // 【肩扛姿勢】中大型近戰武器、槍械或長武器橫放／斜放在肩膀。
    public static readonly EquipmentPoseSettings Shoulder = new(
        offsetX: 18f,                  // ScourgeMod 基準：定位到角色面前的肩部。
        offsetY: 4f,                   // 略往身體下方調整。
        baseRotationDegrees: 0f,       // 肩扛沒有額外固定旋轉。
        facingRotationDegrees: -90f,   // 面向右為 -90 度，面向左為 +90 度。
        scale: 1f,                     // 使用物品貼圖原始大小。
        originX: 0f,                   // 面向右固定左下角；面向左會自動鏡像到右下角。
        originY: 1f);

    // 【背掛姿勢】大型刀劍、弓與長槍斜掛在角色背後。
    public static readonly EquipmentPoseSettings Back = new(
        offsetX: -10f,                   // 背掛置中。
        offsetY: -3f,                  // 稍微往肩部移動。
        baseRotationDegrees: 180f,     // 固定倒轉貼圖。
        facingRotationDegrees: -90f,   // 左右各增加 10 度的方向傾斜。
        scale: 1f,
        originX: 0.5f,
        originY: 0.5f);

    // 【拖地姿勢】溜溜球、鏈球或肩扛衝突的副武器，由手部反手握住並朝地面拖行。
    public static readonly EquipmentPoseSettings Drag = new(
        offsetX: -7f,
        offsetY: 8f,
        baseRotationDegrees: 0f,
        facingRotationDegrees: -180f,
        scale: 0.7f,
        originX: 0.5f,
        originY: 0.5f); 

    // 【雙手捧持姿勢】投擲物或需要雙手準備的物品放在胸前。
    public static readonly EquipmentPoseSettings DualHands = new(
        offsetX: 8f,                   // 水平位置：越大越靠角色面前。
        offsetY: 2f,                   // 垂直位置：增加往腹部，減少往胸口。
        baseRotationDegrees: 0f,
        facingRotationDegrees: 0f,
        scale: 0.85f,                  // 雙手物品的顯示大小。
        originX: 0.5f,                 // 水平錨點：0.5 使用貼圖中央。
        originY: 0.5f);                // 垂直錨點：0.5 使用貼圖中央。

    // 【盾牌背掛姿勢】盾牌固定在角色背部中央。
    public static readonly EquipmentPoseSettings ShieldBack = new(
        offsetX: -6f,                  // 水平位置：越小越靠角色背後。
        offsetY: 0f,                   // 垂直位置：增加往腰部，減少往肩部。
        baseRotationDegrees: 0f,
        facingRotationDegrees: 8f,     // 左右對稱的 8 度傾斜。
        scale: 0.9f,                   // 盾牌顯示大小。
        originX: 0.5f,                 // 水平錨點：預設為盾牌中心。
        originY: 0.5f);                // 垂直錨點：預設為盾牌中心。

    public static EquipmentPoseSettings Get(WeaponCarryStyle style)
    {
        return style switch
        {
            WeaponCarryStyle.Waist => Waist,
            WeaponCarryStyle.Shoulder => Shoulder,
            WeaponCarryStyle.Back => Back,
            WeaponCarryStyle.Drag => Drag,
            WeaponCarryStyle.DualHands => DualHands,
            WeaponCarryStyle.ShieldBack => ShieldBack,
            _ => None
        };
    }
}

public sealed class EquipmentPoseSettings
{
    public EquipmentPoseSettings(
        float offsetX,
        float offsetY,
        float baseRotationDegrees,
        float facingRotationDegrees,
        float scale,
        float originX,
        float originY)
    {
        OffsetX = offsetX;
        OffsetY = offsetY;
        BaseRotationDegrees = baseRotationDegrees;
        FacingRotationDegrees = facingRotationDegrees;
        Scale = scale;
        OriginX = originX;
        OriginY = originY;
    }

    public float OffsetX { get; }

    public float OffsetY { get; }

    public float BaseRotationDegrees { get; }

    public float FacingRotationDegrees { get; }

    public float Scale { get; }

    public float OriginX { get; }

    public float OriginY { get; }

}
