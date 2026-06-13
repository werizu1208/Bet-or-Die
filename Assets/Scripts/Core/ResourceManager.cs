using UnityEngine;
using System;
using System.Collections.Generic;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    public int Gold { get; private set; }
    public int Lifespan { get; private set; }
    public List<LimbType> RemainingLimbs { get; private set; } = new List<LimbType>();

    private int maxLifespan;
    private int maxLimbs;

    public event Action OnResourceChanged;
    public event Action OnGameOver;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void Initialize(GameConfig config)
    {
        Gold = config.startingGold;
        Lifespan = config.startingLifespan;
        maxLifespan = config.startingLifespan;
        maxLimbs = config.startingLimbs;

        RemainingLimbs.Clear();
        RemainingLimbs.Add(LimbType.RightArm);
        RemainingLimbs.Add(LimbType.LeftArm);
        RemainingLimbs.Add(LimbType.RightLeg);
        RemainingLimbs.Add(LimbType.LeftLeg);
        RemainingLimbs.Add(LimbType.Head);

        OnResourceChanged?.Invoke();
    }

    // ── Gold ──────────────────────────────────────────────

    public void AddGold(int amount)
    {
        Gold += amount;
        OnResourceChanged?.Invoke();
    }

    public bool TrySpendGold(int amount)
    {
        if (Gold < amount) return false;
        Gold -= amount;
        OnResourceChanged?.Invoke();
        return true;
    }

    // ── Lifespan ──────────────────────────────────────────

    public void AddLifespan(int years)
    {
        var config = GameManager.Instance.Config;
        int overflow = (Lifespan + years) - maxLifespan;
        Lifespan = Mathf.Min(Lifespan + years, maxLifespan);
        if (overflow > 0)
            Gold += overflow * config.lifespanToGoldRate;
        OnResourceChanged?.Invoke();
    }

    public bool TrySpendLifespan(int years)
    {
        if (Lifespan < years) return false;
        Lifespan -= years;
        OnResourceChanged?.Invoke();
        return true;
    }

    // ── Limbs ─────────────────────────────────────────────

    public void AddLimb(LimbType limb)
    {
        if (!RemainingLimbs.Contains(limb) && RemainingLimbs.Count < maxLimbs)
        {
            RemainingLimbs.Add(limb);
            OnResourceChanged?.Invoke();
        }
    }

    public void LoseLimb(LimbType limb)
    {
        RemainingLimbs.Remove(limb);
        OnResourceChanged?.Invoke();
        CheckGameOver();
    }

    public LimbType LoseRandomLimb()
    {
        int index = UnityEngine.Random.Range(0, RemainingLimbs.Count);
        LimbType lost = RemainingLimbs[index];
        LoseLimb(lost);
        return lost;
    }

    public bool HasLimb(LimbType limb) => RemainingLimbs.Contains(limb);
    public bool HasBothArms() => RemainingLimbs.Contains(LimbType.RightArm) && RemainingLimbs.Contains(LimbType.LeftArm);
    public bool HasBothLegs() => RemainingLimbs.Contains(LimbType.RightLeg) && RemainingLimbs.Contains(LimbType.LeftLeg);
    public bool HasAnyLimb() => RemainingLimbs.Count > 0;

    // ── Conversion ───────────────────────────────────────

    public void ConvertLimbToLifespanAndGold(LimbType limb)
    {
        var config = GameManager.Instance.Config;
        RemainingLimbs.Remove(limb);
        int lifespanOverflow = (Lifespan + config.limbToLifespanRate) - maxLifespan;
        Lifespan = Mathf.Min(Lifespan + config.limbToLifespanRate, maxLifespan);
        if (lifespanOverflow > 0)
            Gold += lifespanOverflow * config.lifespanToGoldRate;
        OnResourceChanged?.Invoke();
    }

    public void ConvertLifespanToGold(int years)
    {
        var config = GameManager.Instance.Config;
        int actual = Mathf.Min(years, Lifespan);
        Lifespan -= actual;
        Gold += actual * config.lifespanToGoldRate;
        OnResourceChanged?.Invoke();
    }

    // ── Win/Loss ─────────────────────────────────────────

    public void ApplyWinGold(int betAmount, float preMultiplier, float gameMultiplier)
    {
        int earned = Mathf.RoundToInt(betAmount * preMultiplier * gameMultiplier);
        AddGold(earned);
    }

    public void ApplyWinLifespan(int betAmount, float preMultiplier, float gameMultiplier)
    {
        var config = GameManager.Instance.Config;
        int earned = Mathf.RoundToInt(betAmount * preMultiplier * gameMultiplier);
        int lifespanOverflow = (Lifespan + earned) - maxLifespan;
        Lifespan = Mathf.Min(Lifespan + earned, maxLifespan);
        if (lifespanOverflow > 0)
            Gold += lifespanOverflow * config.lifespanToGoldRate;
        OnResourceChanged?.Invoke();
    }

    public void ApplyLoss(BetCurrency currency, int amount)
    {
        if (currency == BetCurrency.Gold)
            Gold = Mathf.Max(0, Gold - amount);
        else
            Lifespan = Mathf.Max(0, Lifespan - amount);
        OnResourceChanged?.Invoke();
    }

    // ── Game Over Check ──────────────────────────────────

    public void CheckGameOver()
    {
        if (RemainingLimbs.Count == 0)
        {
            OnGameOver?.Invoke();
            GameManager.Instance.ChangeState(GameState.GameOver);
        }
    }

    public void CheckStageEndGameOver()
    {
        if (Lifespan <= 0)
        {
            OnGameOver?.Invoke();
            GameManager.Instance.ChangeState(GameState.GameOver);
        }
    }
}
