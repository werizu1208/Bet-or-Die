using UnityEngine;
using TMPro;

public class ResourceUI : MonoBehaviour
{
    [Header("Text References")]
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI lifespanText;
    [SerializeField] private TextMeshProUGUI stageText;
    [SerializeField] private TextMeshProUGUI roomText;

    void OnEnable()
    {
        ResourceManager.Instance.OnResourceChanged += Refresh;
        StageManager.Instance.OnRoomChanged += RefreshRoom;
        Refresh();
    }

    void OnDisable()
    {
        if (ResourceManager.Instance != null)
            ResourceManager.Instance.OnResourceChanged -= Refresh;
        if (StageManager.Instance != null)
            StageManager.Instance.OnRoomChanged -= RefreshRoom;
    }

    private void Refresh()
    {
        var res = ResourceManager.Instance;
        if (goldText != null) goldText.text = $"{res.Gold}G";
        if (lifespanText != null) lifespanText.text = $"{res.Lifespan}year";
    }

    private void RefreshRoom(int current, int total)
    {
        if (stageText != null) stageText.text = $"Stage {GameManager.Instance.CurrentStageIndex + 1}";
        if (roomText != null) roomText.text = $"{current} / {total}";
    }
}
