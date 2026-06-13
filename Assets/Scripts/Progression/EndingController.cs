using UnityEngine;
using System;

public class EndingController : MonoBehaviour
{
    public static EndingController Instance { get; private set; }

    public event Action<int> OnVictory;
    public event Action OnGameOver;
    public event Action OnDemonBanishment;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        GameManager.Instance.OnStateChanged += HandleStateChanged;
    }

    private void HandleStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.Victory:
                SkillManager.Instance.ClearAllSkills();
                OnVictory?.Invoke(ResourceManager.Instance.Gold);
                break;
            case GameState.GameOver:
                SkillManager.Instance.ClearAllSkills();
                OnGameOver?.Invoke();
                break;
            case GameState.DemonBanishment:
                SkillManager.Instance.ClearAllSkills();
                OnDemonBanishment?.Invoke();
                break;
        }
    }
}
