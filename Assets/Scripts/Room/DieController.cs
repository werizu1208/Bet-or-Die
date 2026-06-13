using UnityEngine;
using System;

public class DieController : MonoBehaviour
{
    public static DieController Instance { get; private set; }

    public event Action<LimbType> OnLimbLost;
    public event Action OnDieComplete;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void ExecuteDie()
    {
        var res = ResourceManager.Instance;
        var cfg = GameManager.Instance.CurrentStageConfig;

        // 頭のみ残っている場合は即ゲームオーバー
        if (res.RemainingLimbs.Count == 1 && res.HasLimb(LimbType.Head))
        {
            res.LoseLimb(LimbType.Head);
            return;
        }

        if (res.Lifespan > 0)
        {
            bool spent = res.TrySpendLifespan(cfg.dieCostLifespan);
            if (!spent)
            {
                // 寿命が足りない分だけ残りを使い切り
                res.TrySpendLifespan(res.Lifespan);
                LoseRandomLimb();
            }
        }
        else
        {
            LoseRandomLimb();
        }

        OnDieComplete?.Invoke();
        ProceedAfterDie();
    }

    // 四肢選択権スキルがある場合はUIから呼ぶ
    public void ExecuteDieWithLimbChoice(LimbType chosen)
    {
        ResourceManager.Instance.LoseLimb(chosen);
        OnLimbLost?.Invoke(chosen);
        OnDieComplete?.Invoke();
        ProceedAfterDie();
    }

    private void LoseRandomLimb()
    {
        // 頭は最後にしか選ばれない（頭以外が残っている場合）
        var res = ResourceManager.Instance;
        var limbs = new System.Collections.Generic.List<LimbType>(res.RemainingLimbs);
        if (limbs.Count > 1) limbs.Remove(LimbType.Head);

        LimbType lost = limbs[UnityEngine.Random.Range(0, limbs.Count)];
        res.LoseLimb(lost);
        OnLimbLost?.Invoke(lost);
    }

    private void ProceedAfterDie()
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
}
