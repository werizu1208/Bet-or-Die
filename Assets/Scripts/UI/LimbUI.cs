using UnityEngine;
using UnityEngine.UI;

public class LimbUI : MonoBehaviour
{
    [Header("Limb Images (人体UI)")]
    [SerializeField] private Image rightArmImage;
    [SerializeField] private Image leftArmImage;
    [SerializeField] private Image rightLegImage;
    [SerializeField] private Image leftLegImage;
    [SerializeField] private Image headImage;

    [Header("Colors")]
    [SerializeField] private Color intactColor = Color.white;
    [SerializeField] private Color lostColor = new Color(0.3f, 0.3f, 0.3f, 0.5f);

    void OnEnable()
    {
        ResourceManager.Instance.OnResourceChanged += Refresh;
        Refresh();
    }

    void OnDisable()
    {
        if (ResourceManager.Instance != null)
            ResourceManager.Instance.OnResourceChanged -= Refresh;
    }

    private void Refresh()
    {
        var res = ResourceManager.Instance;
        SetLimb(rightArmImage, res.HasLimb(LimbType.RightArm));
        SetLimb(leftArmImage, res.HasLimb(LimbType.LeftArm));
        SetLimb(rightLegImage, res.HasLimb(LimbType.RightLeg));
        SetLimb(leftLegImage, res.HasLimb(LimbType.LeftLeg));
        SetLimb(headImage, res.HasLimb(LimbType.Head));
    }

    private void SetLimb(Image img, bool intact)
    {
        if (img != null) img.color = intact ? intactColor : lostColor;
    }

    // 四肢選択権スキル用：選択可能な四肢ボタンを有効化する
    public void EnableLimbSelection(System.Action<LimbType> onSelected)
    {
        SetupLimbButton(rightArmImage, LimbType.RightArm, onSelected);
        SetupLimbButton(leftArmImage, LimbType.LeftArm, onSelected);
        SetupLimbButton(rightLegImage, LimbType.RightLeg, onSelected);
        SetupLimbButton(leftLegImage, LimbType.LeftLeg, onSelected);
    }

    private void SetupLimbButton(Image img, LimbType limb, System.Action<LimbType> onSelected)
    {
        if (img == null) return;
        var btn = img.GetComponent<Button>();
        if (btn == null) btn = img.gameObject.AddComponent<Button>();
        btn.interactable = ResourceManager.Instance.HasLimb(limb);
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => onSelected?.Invoke(limb));
    }
}
