using UnityEngine;

[CreateAssetMenu(fileName = "RouletteData", menuName = "BetOrDie/Gambling/RouletteData")]
public class RouletteData : GamblingGameData
{
    [Header("Wheel")]
    public int numberCount = 36;

    [Header("Multipliers")]
    public float singleNumberMultiplier = 35f;
    public float colorMultiplier = 2f;
    public float oddEvenMultiplier = 2f;
    public float halfRangeMultiplier = 2f;
}
