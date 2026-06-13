using UnityEngine;

[System.Serializable]
public class SlotSymbol
{
    public string symbolName;
    public Sprite icon;
    [Range(0f, 1f)] public float weight = 0.2f;
}

[CreateAssetMenu(fileName = "SlotData", menuName = "BetOrDie/Gambling/SlotData")]
public class SlotData : GamblingGameData
{
    [Header("Symbols")]
    public SlotSymbol[] symbols;

    [Header("Multipliers")]
    public float tripleRareMultiplier = 10f;
    public float tripleNormalMultiplier = 3f;
    public float doubleMultiplier = 1.5f;

    [Header("Rare Symbol Index")]
    public int rareSymbolIndex = 0;

    [Header("Spin Duration")]
    public float spinDuration = 2f;
}
