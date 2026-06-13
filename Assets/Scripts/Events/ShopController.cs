using UnityEngine;
using System;

public class ShopController : MonoBehaviour
{
    public static ShopController Instance { get; private set; }

    [SerializeField] private ItemData[] goldShopItems;
    [SerializeField] private ItemData[] lifespanShopItems;

    public event Action<ItemData[]> OnShopOpened;
    public event Action<ItemData, bool> OnPurchaseResult;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void OpenGoldShop() => OnShopOpened?.Invoke(goldShopItems);
    public void OpenLifespanShop() => OnShopOpened?.Invoke(lifespanShopItems);

    public void TryPurchase(ItemData item)
    {
        var res = ResourceManager.Instance;
        bool success = false;

        if (item.costType == ResourceType.Gold)
            success = res.TrySpendGold(item.costAmount);
        else if (item.costType == ResourceType.Lifespan)
            success = res.TrySpendLifespan(item.costAmount);

        if (success && item.linkedSkill != null)
            SkillManager.Instance.GrantSkill(item.linkedSkill);

        OnPurchaseResult?.Invoke(item, success);
    }
}
