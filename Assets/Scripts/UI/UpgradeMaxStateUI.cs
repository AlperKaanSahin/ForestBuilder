using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMaxStateUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RepeatableUpgrade upgrade;
    [SerializeField] private TMP_Text infoText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private Button purchaseButton;

    [Header("Max Display")]
    [SerializeField] private bool replaceInfoText = true;
    [SerializeField] private string maxInfoText = "MAX";

    [SerializeField] private bool replaceCostText = true;
    [SerializeField] private string maxCostText = "MAX";

    [Header("Button")]
    [SerializeField] private bool disableButtonAtMax = true;

    private void Start()
    {
        Refresh();
        upgrade.LevelChanged += OnLevelChanged;
    }

    private void OnDestroy()
    {
        if (upgrade != null)
            upgrade.LevelChanged -= OnLevelChanged;
    }

    private void OnLevelChanged(int level)
    {
        Refresh();
    }

    private void Refresh()
    {
        if (!upgrade.IsMaxLevel)
            return;

        if (replaceInfoText && infoText != null)
            infoText.text = maxInfoText;

        if (replaceCostText && costText != null)
            costText.text = maxCostText;

        if (disableButtonAtMax && purchaseButton != null)
            purchaseButton.interactable = false;
    }
}