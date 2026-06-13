using UnityEngine;

[System.Serializable]
public class AnimalEntry
{
    public string animalName;
    public Sprite icon;
    public float odds = 2f;
}

[CreateAssetMenu(fileName = "AnimalRaceData", menuName = "BetOrDie/Gambling/AnimalRaceData")]
public class AnimalRaceData : GamblingGameData
{
    [Header("Animals (4 entries)")]
    public AnimalEntry[] animals = new AnimalEntry[4];

    [Header("Race Duration")]
    public float raceDuration = 3f;
}
