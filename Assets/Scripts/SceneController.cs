using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }

    [Header("Scene Names")]
    [SerializeField] private string startSceneName = "StartScene";
    [SerializeField] private string gameSceneName = "GameScene";

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void GoToGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void GoToStart()
    {
        SceneManager.LoadScene(startSceneName);
    }

    public void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
