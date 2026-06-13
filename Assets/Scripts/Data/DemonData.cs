using UnityEngine;

[CreateAssetMenu(fileName = "DemonData", menuName = "BetOrDie/DemonData")]
public class DemonData : ScriptableObject
{
    [Header("Identity")]
    public string demonName;
    public DemonPersonality personality;
    public Sprite portrait;

    [Header("Gambling")]
    public GamblingGameType[] gamblingGames;

    [Header("Multiplier Bias (0=lower half, 1=upper half)")]
    [Range(0f, 1f)] public float multiplierRangeBias = 0.5f;

    [Header("Mammon Special Rule")]
    public bool hasLifespanBetSpike = false;
    public float lifespanBetSpikeMinMultiplier = 2f;
}
