using UnityEngine;
using System;

public class LimbRepairController : MonoBehaviour
{
    public static LimbRepairController Instance { get; private set; }

    public event Action<LimbType, int> OnRepairReady;
    public event Action<LimbType, bool> OnRepairResult;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void OpenRepairMenu()
    {
        var res = ResourceManager.Instance;
        // 欠損している四肢を取得
        foreach (LimbType limb in System.Enum.GetValues(typeof(LimbType)))
        {
            if (!res.HasLimb(limb))
            {
                int cost = CalculateCost();
                OnRepairReady?.Invoke(limb, cost);
            }
        }
    }

    public int CalculateCost()
    {
        var config = GameManager.Instance.Config;
        float multiplier = EventRoomController.Instance.GetLimbRepairMultiplier();
        return Mathf.RoundToInt(config.limbToGoldRate * multiplier);
    }

    public void TryRepair(LimbType limb)
    {
        int cost = CalculateCost();
        bool success = ResourceManager.Instance.TrySpendGold(cost);
        if (success)
            ResourceManager.Instance.AddLimb(limb);
        OnRepairResult?.Invoke(limb, success);
    }
}
