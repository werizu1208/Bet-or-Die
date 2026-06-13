using UnityEngine;

[CreateAssetMenu(fileName = "BaccaratData", menuName = "BetOrDie/Gambling/BaccaratData")]
public class BaccaratData : GamblingGameData
{
    [Header("Multipliers")]
    public float playerWinMultiplier = 2f;
    public float bankerWinMultiplier = 2f;
    public float tieMultiplier = 8f;
}
