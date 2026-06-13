using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BetUI : MonoBehaviour
{
    [Header("Bet Amount")]
    [SerializeField] private Slider betSlider;
    [SerializeField] private TMP_InputField betInputField;
    [SerializeField] private TextMeshProUGUI minLabel;
    [SerializeField] private TextMeshProUGUI maxLabel;
    [SerializeField] private TextMeshProUGUI currencyLabel;
    [SerializeField] private TextMeshProUGUI preMultiplierLabel;

    [Header("Buttons")]
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;

    [Header("Gold Depletion Dialog")]
    [SerializeField] private GameObject depletionDialog;
    [SerializeField] private Button depletionYesButton;
    [SerializeField] private Button depletionNoButton;

    void OnEnable()
    {
        BetController.Instance.OnBetRangeReady += SetupUI;
        BetController.Instance.OnGoldDepletionWarning += ShowDepletionDialog;
        confirmButton?.onClick.AddListener(BetController.Instance.ConfirmBet);
        depletionYesButton?.onClick.AddListener(OnDepletionYes);
        depletionNoButton?.onClick.AddListener(OnDepletionNo);
        betSlider?.onValueChanged.AddListener(OnSliderChanged);

        float pm = RoomController.Instance.CurrentPreMultiplier;
        if (preMultiplierLabel != null) preMultiplierLabel.text = $"×{pm:F2}";
    }

    void OnDisable()
    {
        if (BetController.Instance != null)
        {
            BetController.Instance.OnBetRangeReady -= SetupUI;
            BetController.Instance.OnGoldDepletionWarning -= ShowDepletionDialog;
        }
        confirmButton?.onClick.RemoveAllListeners();
        depletionYesButton?.onClick.RemoveAllListeners();
        depletionNoButton?.onClick.RemoveAllListeners();
        betSlider?.onValueChanged.RemoveAllListeners();
    }

    private void SetupUI(int min, int max)
    {
        if (betSlider != null)
        {
            betSlider.minValue = min;
            betSlider.maxValue = max;
            betSlider.value = min;
        }
        if (betInputField != null) betInputField.text = min.ToString();
        if (minLabel != null) minLabel.text = min.ToString();
        if (maxLabel != null) maxLabel.text = max.ToString();

        var currency = RoomController.Instance.CurrentBetCurrency;
        if (currencyLabel != null)
            currencyLabel.text = currency == BetCurrency.Gold ? "G" : "year";
    }

    private void OnSliderChanged(float val)
    {
        int amount = Mathf.RoundToInt(val);
        BetController.Instance.SetBetAmount(amount);
        if (betInputField != null) betInputField.text = amount.ToString();
    }

    private void ShowDepletionDialog()
    {
        depletionDialog?.SetActive(true);
    }

    private void OnDepletionYes()
    {
        depletionDialog?.SetActive(false);
        BetController.Instance.ConfirmConversionAndBet();
    }

    private void OnDepletionNo()
    {
        depletionDialog?.SetActive(false);
    }
}
