using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Panels")]
    [SerializeField] private GameObject startScreenPanel;
    [SerializeField] private GameObject explanationPanel;
    [SerializeField] private GameObject betOrDiePanel;
    [SerializeField] private GameObject bettingPanel;
    [SerializeField] private GameObject gamblingPanel;
    [SerializeField] private GameObject eventRoomPanel;
    [SerializeField] private GameObject stageEndPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject demonBanishmentPanel;
    [SerializeField] private GameObject goldDepletionDialogPanel;

    [Header("HUD")]
    [SerializeField] private ResourceUI resourceUI;
    [SerializeField] private LimbUI limbUI;

    public ResourceUI ResourceUI => resourceUI;
    public LimbUI LimbUI => limbUI;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        GameManager.Instance.OnStateChanged += HandleStateChanged;
    }

    private void HandleStateChanged(GameState state)
    {
        HideAll();
        switch (state)
        {
            case GameState.StartScreen:      Show(startScreenPanel); break;
            case GameState.Explanation:      Show(explanationPanel); break;
            case GameState.BetOrDieChoice:   Show(betOrDiePanel); break;
            case GameState.Betting:          Show(bettingPanel); break;
            case GameState.Gambling:         Show(gamblingPanel); break;
            case GameState.EventRoom:        Show(eventRoomPanel); break;
            case GameState.StageEnd:         Show(stageEndPanel); break;
            case GameState.GameOver:         Show(gameOverPanel); break;
            case GameState.Victory:          Show(victoryPanel); break;
            case GameState.DemonBanishment:  Show(demonBanishmentPanel); break;
        }
    }

    private void HideAll()
    {
        startScreenPanel?.SetActive(false);
        explanationPanel?.SetActive(false);
        betOrDiePanel?.SetActive(false);
        bettingPanel?.SetActive(false);
        gamblingPanel?.SetActive(false);
        eventRoomPanel?.SetActive(false);
        stageEndPanel?.SetActive(false);
        gameOverPanel?.SetActive(false);
        victoryPanel?.SetActive(false);
        demonBanishmentPanel?.SetActive(false);
        goldDepletionDialogPanel?.SetActive(false);
    }

    private void Show(GameObject panel) => panel?.SetActive(true);

    public void ShowGoldDepletionDialog() => goldDepletionDialogPanel?.SetActive(true);
    public void HideGoldDepletionDialog() => goldDepletionDialogPanel?.SetActive(false);
}
