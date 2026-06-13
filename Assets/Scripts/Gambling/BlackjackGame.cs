using UnityEngine;
using System.Collections.Generic;
using System;

public class BlackjackGame : MonoBehaviour
{
    public static BlackjackGame Instance { get; private set; }

    private BlackjackData data;
    private List<int> deck = new List<int>();
    private List<int> playerHand = new List<int>();
    private List<int> dealerHand = new List<int>();

    public event Action<List<int>, List<int>> OnHandsUpdated;
    public event Action<bool, float> OnGameEnd;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void Begin(BlackjackData d)
    {
        data = d;
        BuildDeck();
        playerHand.Clear();
        dealerHand.Clear();
        playerHand.Add(DrawCard());
        dealerHand.Add(DrawCard());
        playerHand.Add(DrawCard());
        dealerHand.Add(DrawCard()); // 2枚目は伏せ
        OnHandsUpdated?.Invoke(playerHand, dealerHand);
    }

    private void BuildDeck()
    {
        deck.Clear();
        for (int i = 0; i < 4; i++)
            for (int v = 1; v <= 13; v++)
                deck.Add(Mathf.Min(v, 10));
        for (int i = deck.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (deck[i], deck[j]) = (deck[j], deck[i]);
        }
    }

    private int DrawCard() { int c = deck[0]; deck.RemoveAt(0); return c; }

    private int HandValue(List<int> hand)
    {
        int total = 0, aces = 0;
        foreach (int c in hand) { total += c == 1 ? 11 : c; if (c == 1) aces++; }
        while (total > 21 && aces > 0) { total -= 10; aces--; }
        return total;
    }

    public void PlayerHit()
    {
        playerHand.Add(DrawCard());
        OnHandsUpdated?.Invoke(playerHand, dealerHand);
        if (HandValue(playerHand) > 21) Resolve();
    }

    public void PlayerStand()
    {
        while (HandValue(dealerHand) <= data.dealerHitThreshold)
            dealerHand.Add(DrawCard());
        Resolve();
    }

    private void Resolve()
    {
        int p = HandValue(playerHand);
        int d = HandValue(dealerHand);
        bool isBlackjack = playerHand.Count == 2 && p == 21;
        bool playerBust = p > 21;
        bool dealerBust = d > 21;

        bool won;
        float multiplier;

        if (playerBust) { won = false; multiplier = 0f; }
        else if (dealerBust || p > d) { won = true; multiplier = isBlackjack ? data.blackjackMultiplier : data.normalWinMultiplier; }
        else if (p == d) { won = false; multiplier = 0f; } // push → 負け扱い
        else { won = false; multiplier = 0f; }

        OnGameEnd?.Invoke(won, multiplier);
        GamblingManager.Instance.ResolveResult(won, multiplier);
    }

    // 透視スキル：伏せカードの値を返す
    public int PeekDealerHiddenCard() => dealerHand.Count > 1 ? dealerHand[1] : 0;
}
