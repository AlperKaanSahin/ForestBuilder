using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RepeatableUpgradeUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RepeatableUpgrade upgrade;

    [Header("UI")]
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text infoText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private Button purchaseButton;

    public RepeatableUpgrade Upgrade => upgrade;

    private void Start()
    {
        Refresh();
        upgrade.LevelChanged += OnLevelChanged;
    }

    private void OnDestroy()
    {
        if (upgrade != null)
        {
            upgrade.LevelChanged -= OnLevelChanged;
        }
    }

    private void OnLevelChanged(int level)
    {
        Refresh();
    }

    public void Purchase()
    {
        upgrade.TryPurchase();
    }

    private void Refresh()
    {
        nameText.text = upgrade.DisplayName;
        infoText.text = upgrade.ProgressText;
        costText.text = $"{upgrade.CurrentCost} Coins";

        purchaseButton.interactable = !upgrade.IsMaxLevel;
    }
}