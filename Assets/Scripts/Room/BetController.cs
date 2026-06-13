using UnityEngine;
using System;

public class BetController : MonoBehaviour
{
    public static BetController Instance { get; private set; }

    public int CurrentBetAmount { get; private set; }
    public BetCurrency CurrentCurrency { get; private set; }

    public event Action<int, int> OnBetRangeReady;
    public event Action OnGoldDepletionWarning;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void BeginBetting(BetCurrency currency)
    {
        CurrentCurrency = currency;
        int min = DemonController.Instance.GetMinimumBet(currency);
        int max = DemonController.Instance.GetMaximumBet(currency);

        if (max <= 0)
        {
            // リソースが足りない → 変換警告を出す
            OnGoldDepletionWarning?.Invoke();
            return;
        }

        min = Mathf.Min(min, max);
        CurrentBetAmount = min;
        OnBetRangeReady?.Invoke(min, max);
    }

    public void SetBetAmount(int amount)
    {
        int min = DemonController.Instance.GetMinimumBet(CurrentCurrency);
        int max = DemonController.Instance.GetMaximumBet(CurrentCurrency);
        CurrentBetAmount = Mathf.Clamp(amount, min, max);
    }

    public void ConfirmBet()
    {
        var res = ResourceManager.Instance;
        // 残高チェック
        if (CurrentCurrency == BetCurrency.Gold && res.Gold < CurrentBetAmount)
        {
            OnGoldDepletionWarning?.Invoke();
            return;
        }
        GameManager.Instance.ChangeState(GameState.Gambling);
        GamblingManager.Instance.StartGamble(
            DemonController.Instance.PickGamblingGame(),
            CurrentCurrency,
            CurrentBetAmount,
            RoomController.Instance.CurrentPreMultiplier
        );
    }

    // 変換ダイアログでYesを選択した後に呼ぶ
    public void ConfirmConversionAndBet()
    {
        var res = ResourceManager.Instance;
        var config = GameManager.Instance.Config;

        if (CurrentCurrency == BetCurrency.Gold)
        {
            // 金が足りない → 寿命を金に変換
            int needed = CurrentBetAmount - res.Gold;
            int yearsNeeded = Mathf.CeilToInt((float)needed / config.lifespanToGoldRate);
            res.ConvertLifespanToGold(yearsNeeded);
        }
        ConfirmBet();
    }
}
