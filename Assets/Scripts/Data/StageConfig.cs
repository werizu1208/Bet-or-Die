using UnityEngine;

[CreateAssetMenu(fileName = "StageConfig", menuName = "BetOrDie/StageConfig")]
public class StageConfig : ScriptableObject
{
    [Header("Stage Info")]
    public int stageNumber = 1;

    [Header("Room Count")]
    public int[] roomCountOptions = { 8, 12, 16, 20 };

    [Header("Pre-Multiplier Range")]
    public float preMultiplierMin = 1.1f;
    public float preMultiplierMax = 1.5f;

    [Header("Bet Settings")]
    public int minimumBetGold = 50;
    public int minimumBetLifespan = 1;

    [Header("DIE Settings")]
    public int dieCostLifespan = 10;

    [Header("Event Room Probability")]
    [Range(0f, 1f)] public float eventRoomBaseChance = 0.15f;
    [Range(0f, 1f)] public float eventRoomIncreaseStep = 0.15f;
    [Range(0f, 1f)] public float eventRoomMaxChance = 0.70f;

    [Header("Limb Repair Cost")]
    public float limbRepairCostMultiplier = 1.5f;
}
