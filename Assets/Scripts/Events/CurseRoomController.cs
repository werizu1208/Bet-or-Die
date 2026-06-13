using UnityEngine;
using System;

public class CurseRoomController : MonoBehaviour
{
    public static CurseRoomController Instance { get; private set; }

    [SerializeField] private SkillData[] curseSkills;

    public event Action<SkillData> OnCursePresented;
    public event Action<SkillData> OnCurseApplied;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void EnterCurseRoom()
    {
        SkillData curse = curseSkills[UnityEngine.Random.Range(0, curseSkills.Length)];
        OnCursePresented?.Invoke(curse);
        // 呪いは拒否不可・自動付与
        ApplyCurse(curse);
    }

    private void ApplyCurse(SkillData curse)
    {
        SkillManager.Instance.GrantSkill(curse);
        OnCurseApplied?.Invoke(curse);
    }
}
