using UnityEngine;
using System;

public class EventRoomController : MonoBehaviour
{
    public static EventRoomController Instance { get; private set; }

    [SerializeField] private EventRoomConfig config;

    public EventRoomType CurrentEventType { get; private set; }
    public event Action<EventRoomType> OnEventRoomEntered;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void EnterEventRoom()
    {
        CurrentEventType = RollEventType();
        OnEventRoomEntered?.Invoke(CurrentEventType);
        GameManager.Instance.ChangeState(GameState.EventRoom);
    }

    private EventRoomType RollEventType()
    {
        var res = ResourceManager.Instance;
        bool limbMissing = res.RemainingLimbs.Count < 5;

        var pool = new System.Collections.Generic.List<EventRoomType>
        {
            EventRoomType.ShopGold,
            EventRoomType.ShopLifespan,
            EventRoomType.SkillGrant,
            EventRoomType.CurseRoom,
            EventRoomType.CapturedHuman
        };

        if (limbMissing) pool.Add(EventRoomType.LimbRepair);

        return pool[UnityEngine.Random.Range(0, pool.Count)];
    }

    public float GetLimbRepairMultiplier()
    {
        int stage = GameManager.Instance.CurrentStageIndex + 1;
        if (stage <= 2) return config.stage1To2Multiplier;
        if (stage <= 5) return config.stage3To5Multiplier;
        return config.stage6PlusMultiplier;
    }

    public int GetCapturedHumanCost() => config.capturedHumanLifespanCost;
    public int GetSkillGrantChoiceCount() => config.skillGrantChoiceCount;

    public void FinishEventRoom()
    {
        var stage = StageManager.Instance;
        if (stage.IsLastRoom())
            GameManager.Instance.ChangeState(GameState.StageEnd);
        else
        {
            stage.AdvanceRoom();
            if (stage.IsEventRoom)
                EnterEventRoom();
            else
                RoomController.Instance.EnterRoom();
        }
    }
}
