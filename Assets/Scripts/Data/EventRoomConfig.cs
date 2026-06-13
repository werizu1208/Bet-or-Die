using UnityEngine;

[CreateAssetMenu(fileName = "EventRoomConfig", menuName = "BetOrDie/EventRoomConfig")]
public class EventRoomConfig : ScriptableObject
{
    [Header("Limb Repair Cost Multipliers by Stage")]
    public float stage1To2Multiplier = 1.5f;
    public float stage3To5Multiplier = 2.0f;
    public float stage6PlusMultiplier = 3.0f;

    [Header("Captured Human Cost")]
    public int capturedHumanLifespanCost = 5;

    [Header("Skill Grant Options Count")]
    public int skillGrantChoiceCount = 3;
}
