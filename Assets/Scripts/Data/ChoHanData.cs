using UnityEngine;

[CreateAssetMenu(fileName = "ChoHanData", menuName = "BetOrDie/Gambling/ChoHanData")]
public class ChoHanData : GamblingGameData
{
    [Header("Multipliers")]
    public float winMultiplier = 2f;

    [Header("Zorome Rule")]
    public bool zoromeConfiscation = true;
}
