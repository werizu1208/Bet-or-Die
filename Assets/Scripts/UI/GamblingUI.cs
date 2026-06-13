using UnityEngine;

public class GamblingUI : MonoBehaviour
{
    [Header("Game Panels")]
    [SerializeField] private GameObject blackjackPanel;
    [SerializeField] private GameObject baccaratPanel;
    [SerializeField] private GameObject chinchiroPanel;
    [SerializeField] private GameObject choHanPanel;
    [SerializeField] private GameObject animalRacePanel;
    [SerializeField] private GameObject roulettePanel;
    [SerializeField] private GameObject slotsPanel;

    void OnEnable()
    {
        GamblingManager.Instance.OnGambleResult += OnResult;
    }

    void OnDisable()
    {
        if (GamblingManager.Instance != null)
            GamblingManager.Instance.OnGambleResult -= OnResult;
    }

    public void ShowGame(GamblingGameType type)
    {
        HideAll();
        switch (type)
        {
            case GamblingGameType.Blackjack:  blackjackPanel?.SetActive(true); break;
            case GamblingGameType.Baccarat:   baccaratPanel?.SetActive(true); break;
            case GamblingGameType.Chinchiro:  chinchiroPanel?.SetActive(true); break;
            case GamblingGameType.ChoHan:     choHanPanel?.SetActive(true); break;
            case GamblingGameType.AnimalRace: animalRacePanel?.SetActive(true); break;
            case GamblingGameType.Roulette:   roulettePanel?.SetActive(true); break;
            case GamblingGameType.Slots:      slotsPanel?.SetActive(true); break;
        }
    }

    private void HideAll()
    {
        blackjackPanel?.SetActive(false);
        baccaratPanel?.SetActive(false);
        chinchiroPanel?.SetActive(false);
        choHanPanel?.SetActive(false);
        animalRacePanel?.SetActive(false);
        roulettePanel?.SetActive(false);
        slotsPanel?.SetActive(false);
    }

    private void OnResult(bool won, float multiplier)
    {
        // 結果演出はここで処理（後でアニメーション等を追加）
    }
}
