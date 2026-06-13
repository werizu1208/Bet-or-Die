using UnityEngine;
using System.Collections.Generic;
using System;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    [SerializeField] private DemonData[] demonPool;

    public int TotalRooms { get; private set; }
    public int CurrentRoom { get; private set; }
    public DemonData CurrentDemon { get; private set; }
    public bool IsEventRoom { get; private set; }

    private float eventRoomCurrentChance;

    public event Action<int, int> OnRoomChanged;
    public event Action<DemonData> OnDemonAssigned;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void InitializeStage()
    {
        var cfg = GameManager.Instance.CurrentStageConfig;
        int[] options = cfg.roomCountOptions;
        TotalRooms = options[UnityEngine.Random.Range(0, options.Length)];
        CurrentRoom = 0;
        eventRoomCurrentChance = cfg.eventRoomBaseChance;
        AdvanceRoom();
    }

    public void AdvanceRoom()
    {
        CurrentRoom++;
        IsEventRoom = RollEventRoom();
        if (!IsEventRoom)
            AssignDemon();
        OnRoomChanged?.Invoke(CurrentRoom, TotalRooms);
    }

    private bool RollEventRoom()
    {
        var cfg = GameManager.Instance.CurrentStageConfig;
        bool triggered = UnityEngine.Random.value < eventRoomCurrentChance;
        if (triggered)
            eventRoomCurrentChance = cfg.eventRoomBaseChance;
        else
            eventRoomCurrentChance = Mathf.Min(eventRoomCurrentChance + cfg.eventRoomIncreaseStep, cfg.eventRoomMaxChance);
        return triggered;
    }

    private void AssignDemon()
    {
        CurrentDemon = demonPool[UnityEngine.Random.Range(0, demonPool.Length)];
        OnDemonAssigned?.Invoke(CurrentDemon);
    }

    public bool IsLastRoom() => CurrentRoom >= TotalRooms;

    // 3択部屋スキル用：候補3部屋の情報を生成
    public RoomPreview[] GenerateRoomPreviews(int count = 3)
    {
        var previews = new RoomPreview[count];
        var cfg = GameManager.Instance.CurrentStageConfig;
        for (int i = 0; i < count; i++)
        {
            bool isEvent = UnityEngine.Random.value < eventRoomCurrentChance;
            DemonData demon = isEvent ? null : demonPool[UnityEngine.Random.Range(0, demonPool.Length)];
            float multiplier = isEvent ? 0f : CalculatePreMultiplier(demon, cfg);
            previews[i] = new RoomPreview { isEventRoom = isEvent, demon = demon, preMultiplier = multiplier };
        }
        return previews;
    }

    public float CalculatePreMultiplier(DemonData demon, StageConfig cfg)
    {
        float min = cfg.preMultiplierMin;
        float max = cfg.preMultiplierMax;
        float bias = demon != null ? demon.multiplierRangeBias : 0.5f;
        float mid = Mathf.Lerp(min, max, 0.5f);
        float rangeMin = bias >= 0.5f ? mid : min;
        float rangeMax = bias >= 0.5f ? max : mid;
        return UnityEngine.Random.Range(rangeMin, rangeMax);
    }
}

[System.Serializable]
public class RoomPreview
{
    public bool isEventRoom;
    public DemonData demon;
    public float preMultiplier;
}
