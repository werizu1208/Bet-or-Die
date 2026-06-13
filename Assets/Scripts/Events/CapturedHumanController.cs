using UnityEngine;
using System;

public class CapturedHumanController : MonoBehaviour
{
    public static CapturedHumanController Instance { get; private set; }

    [SerializeField] private SkillData[] rewardSkillPool;

    public event Action<int> OnChoiceReady;
    public event Action<SkillData> OnHelpResult;
    public event Action OnIgnoreResult;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void Enter()
    {
        int cost = EventRoomController.Instance.GetCapturedHumanCost();
        OnChoiceReady?.Invoke(cost);
    }

    public void ChooseHelp()
    {
        int cost = EventRoomController.Instance.GetCapturedHumanCost();
        bool paid = ResourceManager.Instance.TrySpendLifespan(cost);

        if (paid)
        {
            SkillData reward = rewardSkillPool[UnityEngine.Random.Range(0, rewardSkillPool.Length)];
            SkillManager.Instance.GrantSkill(reward);
            OnHelpResult?.Invoke(reward);
        }
        else
        {
            // 寿命が足りない場合は助けられない
            OnIgnoreResult?.Invoke();
        }
    }

    public void ChooseIgnore() => OnIgnoreResult?.Invoke();
}
