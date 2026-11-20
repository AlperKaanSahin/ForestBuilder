using TMPro;
using UnityEngine;

public class CarryCapacityUpgradeUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CarryCapacityUpgrade upgrade;
    [SerializeField] private PlayerInventory playerInventory;

    [Header("UI")]
    [SerializeField] private TMP_Text infoText;
    [SerializeField] private TMP_Text costText;

    private void Update()
    {
        infoText.text =
            $"{playerInventory.MaxCapacity} → " +
            $"{playerInventory.MaxCapacity + upgrade.CapacityIncrease}";

        costText.text =
            $"{upgrade.CurrentCost} Coins";
    }

    public void Purchase()
    {
        upgrade.TryPurchase();
    }
}