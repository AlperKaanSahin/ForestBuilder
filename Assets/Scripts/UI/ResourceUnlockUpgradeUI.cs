using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceUnlockUpgradeUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ResourceUnlockUpgrade upgrade;

    [Header("UI")]
    [SerializeField] private TMP_Text infoText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private Button purchaseButton;

    private void Update()
    {
        if (upgrade.IsUnlocked)
        {
            infoText.text = "Unlocked";
            costText.text = "";
            purchaseButton.interactable = false;
        }
        else
        {
            infoText.text = "Locked";
            costText.text = $"{upgrade.Cost} Coins";
            purchaseButton.interactable = true;
        }
    }

    public void Purchase()
    {
        upgrade.TryPurchase();
    }
}