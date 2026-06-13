using UnityEngine;

public enum ItemTier { Normal, Premium }

[CreateAssetMenu(fileName = "ItemData", menuName = "BetOrDie/ItemData")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string itemName;
    [TextArea] public string description;
    public Sprite icon;
    public ItemTier tier = ItemTier.Normal;

    [Header("Cost")]
    public ResourceType costType = ResourceType.Gold;
    public int costAmount = 100;

    [Header("Skill Reference")]
    public SkillData linkedSkill;
}
