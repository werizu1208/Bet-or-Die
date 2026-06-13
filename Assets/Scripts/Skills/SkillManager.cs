using UnityEngine;
using System;
using System.Collections.Generic;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }

    private List<SkillData> activeSkills = new List<SkillData>();

    public IReadOnlyList<SkillData> ActiveSkills => activeSkills;
    public event Action<SkillData> OnSkillAdded;
    public event Action<SkillData> OnSkillRemoved;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void GrantSkill(SkillData skill)
    {
        activeSkills.Add(skill);
        OnSkillAdded?.Invoke(skill);
    }

    public bool HasSkill(SkillEffectType type)
    {
        foreach (var s in activeSkills)
            if (s.effectType == type) return true;
        return false;
    }

    public SkillData GetSkill(SkillEffectType type)
    {
        foreach (var s in activeSkills)
            if (s.effectType == type) return s;
        return null;
    }

    public void ConsumeOneTimeSkill(SkillEffectType type)
    {
        var skill = GetSkill(type);
        if (skill != null && skill.isOneTimeUse)
        {
            activeSkills.Remove(skill);
            OnSkillRemoved?.Invoke(skill);
        }
    }

    public float GetWinRateBonus(GamblingGameType gameType)
    {
        float bonus = 0f;
        foreach (var s in activeSkills)
            if (s.effectType == SkillEffectType.GambleWinRateBoost && s.targetGame == gameType)
                bonus += s.effectValue;
        return bonus;
    }

    public int GetDieCostReduction()
    {
        var s = GetSkill(SkillEffectType.DieCostReduction);
        return s != null ? Mathf.RoundToInt(s.effectValue) : 0;
    }

    public float GetLifespanBetMinReduction()
    {
        var s = GetSkill(SkillEffectType.LifespanBetMinReduction);
        return s != null ? s.effectValue : 0f;
    }

    public float GetConversionRateBonus()
    {
        var s = GetSkill(SkillEffectType.ConversionRateBoost);
        return s != null ? s.effectValue : 0f;
    }

    public bool HasLimbChoiceOnDie() => HasSkill(SkillEffectType.LimbChoiceOnDie);
    public bool HasRoomSelection() => HasSkill(SkillEffectType.RoomSelection3Choice);

    // 呪い：臆病の呪い（負けでも寿命を失わない）
    public bool HasCurseTimid() => HasSkill(SkillEffectType.CurseTimid);

    // 呪い：強欲の呪いの金獲得ボーナス
    public float GetCurseGreedGoldBonus()
    {
        var s = GetSkill(SkillEffectType.CurseGreed);
        return s != null ? s.effectValue : 0f;
    }

    // 呪い：悪魔の加護（四肢ゲームオーバー1回回避）
    public bool HasCurseProtection() => HasSkill(SkillEffectType.CurseProtection);
    public void ConsumeCurseProtection() => ConsumeOneTimeSkill(SkillEffectType.CurseProtection);

    public void ClearAllSkills()
    {
        activeSkills.Clear();
    }
}
