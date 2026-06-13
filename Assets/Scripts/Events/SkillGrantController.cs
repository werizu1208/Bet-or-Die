using UnityEngine;
using System;

public class SkillGrantController : MonoBehaviour
{
    public static SkillGrantController Instance { get; private set; }

    [SerializeField] private SkillData[] skillPool;

    public event Action<SkillData[]> OnSkillChoicesReady;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void PresentSkillChoices()
    {
        int count = EventRoomController.Instance.GetSkillGrantChoiceCount();
        var choices = new SkillData[Mathf.Min(count, skillPool.Length)];
        var pool = new System.Collections.Generic.List<SkillData>(skillPool);

        for (int i = 0; i < choices.Length; i++)
        {
            int idx = UnityEngine.Random.Range(0, pool.Count);
            choices[i] = pool[idx];
            pool.RemoveAt(idx);
        }

        OnSkillChoicesReady?.Invoke(choices);
    }

    public void SelectSkill(SkillData chosen)
    {
        var res = ResourceManager.Instance;
        bool paid = false;

        if (chosen.costAmount == 0)
            paid = true;
        else if (chosen.costType == ResourceType.Gold)
            paid = res.TrySpendGold(chosen.costAmount);
        else if (chosen.costType == ResourceType.Lifespan)
            paid = res.TrySpendLifespan(chosen.costAmount);

        if (paid)
            SkillManager.Instance.GrantSkill(chosen);
    }
}
