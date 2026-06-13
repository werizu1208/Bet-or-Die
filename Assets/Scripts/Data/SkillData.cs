using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "BetOrDie/SkillData")]
public class SkillData : ScriptableObject
{
    [Header("Info")]
    public string skillName;
    [TextArea] public string description;
    public Sprite icon;

    [Header("Effect")]
    public SkillEffectType effectType;
    public GamblingGameType targetGame;
    [Range(0f, 1f)] public float effectValue = 0.05f;
    public bool isOneTimeUse = false;

    [Header("Cost")]
    public ResourceType costType = ResourceType.Gold;
    public int costAmount = 100;

    [Header("Curse Demerit")]
    public bool hasDemerit = false;
    public SkillEffectType demeritEffectType;
    public float demeritValue = 0.2f;
}
