using UnityEngine;
using System;

public enum BaccaratBet { Player, Banker, Tie }

public class BaccaratGame : MonoBehaviour
{
    public static BaccaratGame Instance { get; private set; }

    private BaccaratData data;
    private BaccaratBet playerBet;

    public event Action OnAwaitingBet;
    public event Action<int, int, BaccaratBet> OnResult;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void Begin(BaccaratData d)
    {
        data = d;
        OnAwaitingBet?.Invoke();
    }

    public void PlaceBet(BaccaratBet bet)
    {
        playerBet = bet;
        int playerScore = DrawHand();
        int bankerScore = DrawHand();
        Resolve(playerScore, bankerScore);
    }

    private int DrawHand()
    {
        int a = UnityEngine.Random.Range(1, 14);
        int b = UnityEngine.Random.Range(1, 14);
        return (Mathf.Min(a, 10) + Mathf.Min(b, 10)) % 10;
    }

    private void Resolve(int playerScore, int bankerScore)
    {
        bool won;
        float multiplier;

        if (playerScore > bankerScore)
        {
            won = playerBet == BaccaratBet.Player;
            multiplier = data.playerWinMultiplier;
        }
        else if (bankerScore > playerScore)
        {
            won = playerBet == BaccaratBet.Banker;
            multiplier = data.bankerWinMultiplier;
        }
        else
        {
            won = playerBet == BaccaratBet.Tie;
            multiplier = data.tieMultiplier;
        }

        OnResult?.Invoke(playerScore, bankerScore, playerBet);
        GamblingManager.Instance.ResolveResult(won, multiplier);
    }
}
