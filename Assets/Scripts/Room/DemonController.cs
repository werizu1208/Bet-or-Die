using UnityEngine;

public class DemonController : MonoBehaviour
{
    public static DemonController Instance { get; private set; }

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public BetCurrency RollBetCurrency()
    {
        float lifespanChance = GameManager.Instance.GetLifespanBetChance();
        var demon = StageManager.Instance.CurrentDemon;

        // マモン特殊ルール：寿命ベット時は最低額が跳ね上がる（RollBetCurrencyは通常通り）
        return UnityEngine.Random.value < lifespanChance ? BetCurrency.Lifespan : BetCurrency.Gold;
    }

    public int GetMinimumBet(BetCurrency currency)
    {
        var cfg = GameManager.Instance.CurrentStageConfig;
        var demon = StageManager.Instance.CurrentDemon;
        int min = currency == BetCurrency.Gold ? cfg.minimumBetGold : cfg.minimumBetLifespan;

        // マモン：寿命ベット時の最低額をスパイク
        if (demon != null && demon.hasLifespanBetSpike && currency == BetCurrency.Lifespan)
            min = Mathf.RoundToInt(min * demon.lifespanBetSpikeMinMultiplier);

        return min;
    }

    public int GetMaximumBet(BetCurrency currency)
    {
        var res = ResourceManager.Instance;
        return currency == BetCurrency.Gold ? res.Gold : res.Lifespan;
    }

    public GamblingGameType PickGamblingGame()
    {
        var demon = StageManager.Instance.CurrentDemon;
        if (demon == null || demon.gamblingGames.Length == 0)
            return GamblingGameType.Slots;

        // 両腕なし → カード系除外、両足なし → レース系除外
        var available = new System.Collections.Generic.List<GamblingGameType>(demon.gamblingGames);
        var res = ResourceManager.Instance;
        if (!res.HasBothArms())
        {
            available.Remove(GamblingGameType.Blackjack);
            available.Remove(GamblingGameType.Baccarat);
        }
        if (!res.HasBothLegs())
            available.Remove(GamblingGameType.AnimalRace);

        if (available.Count == 0) return GamblingGameType.Slots;
        return available[UnityEngine.Random.Range(0, available.Count)];
    }
}
