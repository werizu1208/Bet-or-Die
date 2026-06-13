using UnityEngine;
using System;
using System.Collections;

public class AnimalRaceGame : MonoBehaviour
{
    public static AnimalRaceGame Instance { get; private set; }

    private AnimalRaceData data;
    private int playerBetIndex;

    public event Action<AnimalRaceData> OnRaceStart;
    public event Action<int> OnRaceEnd;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void Begin(AnimalRaceData d)
    {
        data = d;
        OnRaceStart?.Invoke(d);
    }

    public void PlaceBet(int animalIndex)
    {
        playerBetIndex = animalIndex;
        StartCoroutine(RunRace());
    }

    private IEnumerator RunRace()
    {
        yield return new WaitForSeconds(data.raceDuration);

        // 重み付き抽選でゴール順を決定
        float totalWeight = 0f;
        foreach (var a in data.animals) totalWeight += 1f / a.odds;

        float roll = UnityEngine.Random.Range(0f, totalWeight);
        float cumulative = 0f;
        int winner = 0;
        for (int i = 0; i < data.animals.Length; i++)
        {
            cumulative += 1f / data.animals[i].odds;
            if (roll <= cumulative) { winner = i; break; }
        }

        OnRaceEnd?.Invoke(winner);

        bool won = winner == playerBetIndex;
        float multiplier = won ? data.animals[winner].odds : 0f;
        GamblingManager.Instance.ResolveResult(won, multiplier);
    }
}
