using UnityEngine;
using System;

public class GamblingManager : MonoBehaviour
{
    public static GamblingManager Instance { get; private set; }

    [Header("Game Data")]
    [SerializeField] private BlackjackData blackjackData;
    [SerializeField] private BaccaratData baccaratData;
    [SerializeField] private ChinchiroData chinchiroData;
    [SerializeField] private ChoHanData choHanData;
    [SerializeField] private AnimalRaceData animalRaceData;
    [SerializeField] private RouletteData rouletteData;
    [SerializeField] private SlotData slotData;

    private BetCurrency activeCurrency;
    private int activeBetAmount;
    private float activePreMultiplier;

    public event Action<bool, float> OnGambleResult;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void StartGamble(GamblingGameType type, BetCurrency currency, int betAmount, float preMultiplier)
    {
        activeCurrency = currency;
        activeBetAmount = betAmount;
        activePreMultiplier = preMultiplier;

        switch (type)
        {
            case GamblingGameType.Blackjack:  BlackjackGame.Instance.Begin(blackjackData); break;
            case GamblingGameType.Baccarat:   BaccaratGame.Instance.Begin(baccaratData); break;
            case GamblingGameType.Chinchiro:  ChinchiroGame.Instance.Begin(chinchiroData); break;
            case GamblingGameType.ChoHan:     ChoHanGame.Instance.Begin(choHanData); break;
            case GamblingGameType.AnimalRace: AnimalRaceGame.Instance.Begin(animalRaceData); break;
            case GamblingGameType.Roulette:   RouletteGame.Instance.Begin(rouletteData); break;
            case GamblingGameType.Slots:      SlotGame.Instance.Begin(slotData); break;
        }
    }

    // 各ゲームが結果を返す際に呼ぶ
    public void ResolveResult(bool playerWon, float gameMultiplier)
    {
        // スキルによる保険チェック
        if (!playerWon && SkillManager.Instance.HasSkill(SkillEffectType.BetInsurance))
        {
            SkillManager.Instance.ConsumeOneTimeSkill(SkillEffectType.BetInsurance);
            playerWon = true;
            gameMultiplier = 1f;
        }

        var res = ResourceManager.Instance;
        if (playerWon)
        {
            if (activeCurrency == BetCurrency.Gold)
                res.ApplyWinGold(activeBetAmount, activePreMultiplier, gameMultiplier);
            else
                res.ApplyWinLifespan(activeBetAmount, activePreMultiplier, gameMultiplier);
        }
        else
        {
            res.ApplyLoss(activeCurrency, activeBetAmount);
        }

        OnGambleResult?.Invoke(playerWon, gameMultiplier);
        AfterGamble();
    }

    private void AfterGamble()
    {
        var stage = StageManager.Instance;
        if (stage.IsLastRoom())
            GameManager.Instance.ChangeState(GameState.StageEnd);
        else
        {
            stage.AdvanceRoom();
            if (stage.IsEventRoom)
                GameManager.Instance.ChangeState(GameState.EventRoom);
            else
                RoomController.Instance.EnterRoom();
        }
    }

    public float ApplyWinRatePenalty(float baseWinRate)
    {
        float penalty = GameManager.Instance.GetWinRatePenalty(activePreMultiplier);
        return Mathf.Max(0f, baseWinRate - penalty);
    }

    public GamblingGameData GetData(GamblingGameType type)
    {
        switch (type)
        {
            case GamblingGameType.Blackjack:  return blackjackData;
            case GamblingGameType.Baccarat:   return baccaratData;
            case GamblingGameType.Chinchiro:  return chinchiroData;
            case GamblingGameType.ChoHan:     return choHanData;
            case GamblingGameType.AnimalRace: return animalRaceData;
            case GamblingGameType.Roulette:   return rouletteData;
            case GamblingGameType.Slots:      return slotData;
            default: return null;
        }
    }
}
