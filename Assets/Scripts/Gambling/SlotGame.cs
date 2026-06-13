using UnityEngine;
using System;
using System.Collections;

public class SlotGame : MonoBehaviour
{
    public static SlotGame Instance { get; private set; }

    private SlotData data;

    public event Action OnSpinStart;
    public event Action<int[]> OnReelsSettled;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void Begin(SlotData d)
    {
        data = d;
    }

    public void Spin()
    {
        OnSpinStart?.Invoke();
        StartCoroutine(SpinCoroutine());
    }

    private IEnumerator SpinCoroutine()
    {
        yield return new WaitForSeconds(data.spinDuration);

        int[] reels = new int[3];
        for (int i = 0; i < 3; i++)
            reels[i] = RollSymbol();

        OnReelsSettled?.Invoke(reels);

        bool allSame = reels[0] == reels[1] && reels[1] == reels[2];
        bool twoSame = reels[0] == reels[1] || reels[1] == reels[2] || reels[0] == reels[2];

        float multiplier;
        bool won;

        if (allSame)
        {
            won = true;
            multiplier = reels[0] == data.rareSymbolIndex
                ? data.tripleRareMultiplier
                : data.tripleNormalMultiplier;
        }
        else if (twoSame)
        {
            won = true;
            multiplier = data.doubleMultiplier;
        }
        else
        {
            won = false;
            multiplier = 0f;
        }

        GamblingManager.Instance.ResolveResult(won, multiplier);
    }

    private int RollSymbol()
    {
        float total = 0f;
        foreach (var s in data.symbols) total += s.weight;
        float roll = UnityEngine.Random.Range(0f, total);
        float cumulative = 0f;
        for (int i = 0; i < data.symbols.Length; i++)
        {
            cumulative += data.symbols[i].weight;
            if (roll <= cumulative) return i;
        }
        return 0;
    }
}
