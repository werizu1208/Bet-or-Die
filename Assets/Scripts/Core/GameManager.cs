using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Config")]
    [SerializeField] private GameConfig config;
    [SerializeField] private StageConfig[] stageConfigs;

    public GameConfig Config => config;

    public GameState CurrentState { get; private set; }
    public int CurrentStageIndex { get; private set; } = 0;

    public StageConfig CurrentStageConfig =>
        CurrentStageIndex < stageConfigs.Length ? stageConfigs[CurrentStageIndex] : stageConfigs[stageConfigs.Length - 1];

    public event Action<GameState> OnStateChanged;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start() => ChangeState(GameState.StartScreen);

    public void ChangeState(GameState newState)
    {
        CurrentState = newState;
        OnStateChanged?.Invoke(newState);
    }

    public void StartGame()
    {
        CurrentStageIndex = 0;
        ChangeState(GameState.RoomEntering);
    }

    public void AdvanceToNextStage()
    {
        CurrentStageIndex++;
        int banishStage = config.demonBanishmentStage - 1;
        if (CurrentStageIndex >= banishStage)
            ChangeState(GameState.DemonBanishment);
        else
            ChangeState(GameState.RoomEntering);
    }

    public bool CanEscape() =>
        CurrentStageIndex >= config.escapeUnlockStage;

    public int GetStageClearThreshold()
    {
        int threshold = config.stageClearBaseAmount;
        for (int i = 0; i < CurrentStageIndex; i++)
            threshold = Mathf.RoundToInt(threshold * config.stageClearMultiplier);
        return threshold;
    }

    public float GetLifespanBetChance()
    {
        float chance = config.lifespanBetBaseChance + config.lifespanBetIncreasePerStage * CurrentStageIndex;
        return Mathf.Min(chance, config.lifespanBetMaxChance);
    }

    public float GetWinRatePenalty(float preMultiplier)
    {
        float penalty = 0f;
        for (int i = config.multiplierThresholds.Length - 1; i >= 0; i--)
        {
            if (preMultiplier >= config.multiplierThresholds[i])
            {
                penalty = config.winRatePenalties[i];
                break;
            }
        }
        return penalty;
    }
}
