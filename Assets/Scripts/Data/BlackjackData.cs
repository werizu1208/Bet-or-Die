using UnityEngine;

[CreateAssetMenu(fileName = "BlackjackData", menuName = "BetOrDie/Gambling/BlackjackData")]
public class BlackjackData : GamblingGameData
{
    [Header("Multipliers")]
    public float normalWinMultiplier = 1f;
    public float blackjackMultiplier = 1.5f;

    [Header("Dealer Rules")]
    public int dealerHitThreshold = 16;
}
