using UnityEngine;
using System;

public class RoomController : MonoBehaviour
{
    public static RoomController Instance { get; private set; }

    public float CurrentPreMultiplier { get; private set; }
    public BetCurrency CurrentBetCurrency { get; private set; }

    public event Action<float, BetCurrency> OnBetOrDieReady;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void EnterRoom()
    {
        var demon = StageManager.Instance.CurrentDemon;
        var cfg = GameManager.Instance.CurrentStageConfig;

        CurrentPreMultiplier = StageManager.Instance.CalculatePreMultiplier(demon, cfg);
        CurrentBetCurrency = DemonController.Instance.RollBetCurrency();

        GameManager.Instance.ChangeState(GameState.BetOrDieChoice);
        OnBetOrDieReady?.Invoke(CurrentPreMultiplier, CurrentBetCurrency);
    }

    public void PlayerChoseBet() => GameManager.Instance.ChangeState(GameState.Betting);
    public void PlayerChoseDie() => GameManager.Instance.ChangeState(GameState.DieFlow);
}
