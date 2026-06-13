using UnityEngine;
using System;

public enum ChinchiroHand { Pinzoro, Shigoro, Zoro, Normal, Hifumi, Meshi }

public class ChinchiroGame : MonoBehaviour
{
    public static ChinchiroGame Instance { get; private set; }

    [SerializeField] private DiceController playerDice;
    [SerializeField] private DiceController demonDice;

    private ChinchiroData data;
    private int[] playerResult;
    private int[] demonResult;

    public event Action OnPlayerRoll;
    public event Action<int[]> OnPlayerResult;
    public event Action<int[]> OnDemonResult;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void Begin(ChinchiroData d)
    {
        data = d;
        playerResult = null;
        demonResult = null;
        playerDice.OnDiceSettled += HandlePlayerDice;
        OnPlayerRoll?.Invoke();
        playerDice.ThrowDice();
    }

    private void HandlePlayerDice(int[] results)
    {
        playerDice.OnDiceSettled -= HandlePlayerDice;
        playerResult = results;
        OnPlayerResult?.Invoke(results);
        demonDice.OnDiceSettled += HandleDemonDice;
        demonDice.ThrowDice();
    }

    private void HandleDemonDice(int[] results)
    {
        demonDice.OnDiceSettled -= HandleDemonDice;
        demonResult = results;
        OnDemonResult?.Invoke(results);
        Resolve();
    }

    private void Resolve()
    {
        ChinchiroHand pHand = EvaluateHand(playerResult, out int pPoint);
        ChinchiroHand dHand = EvaluateHand(demonResult, out int dPoint);

        if (pHand == ChinchiroHand.Meshi || pHand == ChinchiroHand.Hifumi)
        {
            GamblingManager.Instance.ResolveResult(false, 0f);
            return;
        }
        if (dHand == ChinchiroHand.Meshi || dHand == ChinchiroHand.Hifumi)
        {
            GamblingManager.Instance.ResolveResult(true, GetMultiplier(pHand));
            return;
        }

        int pRank = HandRank(pHand, pPoint);
        int dRank = HandRank(dHand, dPoint);

        bool won = pRank > dRank;
        float mult = won ? GetMultiplier(pHand) : 0f;
        GamblingManager.Instance.ResolveResult(won, mult);
    }

    private ChinchiroHand EvaluateHand(int[] d, out int point)
    {
        point = 0;
        if (d[0] == 1 && d[1] == 2 && d[2] == 3) return ChinchiroHand.Hifumi;
        if (d[0] == 4 && d[1] == 5 && d[2] == 6) return ChinchiroHand.Shigoro;

        // ゾロ目
        if (d[0] == d[1] && d[1] == d[2])
        {
            if (d[0] == 1) return ChinchiroHand.Pinzoro;
            point = d[0];
            return ChinchiroHand.Zoro;
        }

        // 目あり（ペア＋別の目）
        for (int i = 0; i < 3; i++)
        {
            int a = d[i], b = d[(i + 1) % 3], c = d[(i + 2) % 3];
            if (a == b) { point = c; return ChinchiroHand.Normal; }
        }

        return ChinchiroHand.Meshi;
    }

    private int HandRank(ChinchiroHand hand, int point)
    {
        switch (hand)
        {
            case ChinchiroHand.Pinzoro: return 100;
            case ChinchiroHand.Shigoro: return 90;
            case ChinchiroHand.Zoro: return 50 + point;
            case ChinchiroHand.Normal: return point;
            default: return -1;
        }
    }

    private float GetMultiplier(ChinchiroHand hand)
    {
        switch (hand)
        {
            case ChinchiroHand.Pinzoro: return data.pinzoroMultiplier;
            case ChinchiroHand.Shigoro: return data.shigoroMultiplier;
            case ChinchiroHand.Zoro: return data.zoroMultiplier;
            default: return data.normalMultiplier;
        }
    }
}
