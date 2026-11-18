using TMPro;
using UnityEngine;

public class CarryCapacityUpgradeUI : MonoBehaviour
{
    [SerializeField] private CarryCapacityUpgrade upgrade;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private TMP_Text upgradeText;

    private void Update()
    {
        int currentCapacity = playerInventory.MaxWood;
        int nextCapacity = currentCapacity + upgrade.CapacityIncrease;

        upgradeText.text =
            $"CARRY CAPACITY\n" +
            $"{currentCapacity} > {nextCapacity}\n" +
            $"{upgrade.RemainingCost} COINS";
    }
}