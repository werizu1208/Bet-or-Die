using UnityEngine;
using System;

public enum ChoHanBet { Cho, Han }

public class ChoHanGame : MonoBehaviour
{
    public static ChoHanGame Instance { get; private set; }

    [SerializeField] private DiceController diceController;

    private ChoHanData data;
    private ChoHanBet playerBet;

    public event Action OnAwaitingBet;
    public event Action<int, int, bool> OnResult;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void Begin(ChoHanData d)
    {
        data = d;
        OnAwaitingBet?.Invoke();
    }

    public void PlaceBet(ChoHanBet bet)
    {
        playerBet = bet;
        diceController.OnDiceSettled += HandleDiceResult;
        diceController.ThrowDice();
    }

    private void HandleDiceResult(int[] results)
    {
        diceController.OnDiceSettled -= HandleDiceResult;
        int d1 = results[0], d2 = results[1];
        int total = d1 + d2;
        bool isZorome = d1 == d2;
        bool isCho = total % 2 == 0;

        OnResult?.Invoke(d1, d2, isCho);

        // ゾロ目は没収
        if (isZorome && data.zoromeConfiscation)
        {
            GamblingManager.Instance.ResolveResult(false, 0f);
            return;
        }

        bool won = (isCho && playerBet == ChoHanBet.Cho) || (!isCho && playerBet == ChoHanBet.Han);
        GamblingManager.Instance.ResolveResult(won, data.winMultiplier);
    }
}
