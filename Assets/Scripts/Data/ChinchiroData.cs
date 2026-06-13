using UnityEngine;

[CreateAssetMenu(fileName = "ChinchiroData", menuName = "BetOrDie/Gambling/ChinchiroData")]
public class ChinchiroData : GamblingGameData
{
    [Header("Hand Multipliers")]
    public float pinzoroMultiplier = 5f;
    public float shigoroMultiplier = 3f;
    public float zoroMultiplier = 2f;
    public float normalMultiplier = 1f;

    [Header("Losing Hands")]
    public bool hifumiIsLoss = true;
    public bool meshiIsLoss = true;
}
