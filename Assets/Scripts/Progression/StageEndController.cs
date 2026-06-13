using UnityEngine;
using System;

public class StageEndController : MonoBehaviour
{
    public static StageEndController Instance { get; private set; }

    public event Action<bool, bool> OnStageEndEvaluated;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void Evaluate()
    {
        ResourceManager.Instance.CheckStageEndGameOver();
        if (GameManager.Instance.CurrentState == GameState.GameOver) return;

        int gold = ResourceManager.Instance.Gold;
        int threshold = GameManager.Instance.GetStageClearThreshold();
        bool cleared = gold >= threshold;
        bool canEscape = GameManager.Instance.CanEscape();

        OnStageEndEvaluated?.Invoke(cleared, canEscape);
    }

    public void PlayerChoosesEscape()
    {
        GameManager.Instance.ChangeState(GameState.Victory);
    }

    public void PlayerChoosesContinue()
    {
        GameManager.Instance.AdvanceToNextStage();
        StageManager.Instance.InitializeStage();
    }
}
