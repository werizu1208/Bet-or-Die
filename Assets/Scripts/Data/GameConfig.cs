using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "BetOrDie/GameConfig")]
public class GameConfig : ScriptableObject
{
    [Header("Starting Resources")]
    public int startingGold = 500;
    public int startingLifespan = 80;
    public int startingLimbs = 5;

    [Header("Resource Conversion Rates")]
    public int lifespanToGoldRate = 100;
    public int limbToGoldRate = 1000;
    public int limbToLifespanRate = 10;

    [Header("Stage Clear")]
    public int stageClearBaseAmount = 2000;
    public float stageClearMultiplier = 2f;
    public int escapeUnlockStage = 3;
    public int demonBanishmentStage = 7;

    [Header("Lifespan Bet Probability")]
    [Range(0f, 1f)] public float lifespanBetBaseChance = 0.20f;
    [Range(0f, 0.1f)] public float lifespanBetIncreasePerStage = 0.035f;
    [Range(0f, 1f)] public float lifespanBetMaxChance = 0.40f;

    [Header("Pre-Multiplier Win Rate Thresholds")]
    public float[] multiplierThresholds = { 1.5f, 3.0f, 6.0f };
    [Range(0f, 1f)] public float[] winRatePenalties = { 0.05f, 0.10f, 0.15f };
}
