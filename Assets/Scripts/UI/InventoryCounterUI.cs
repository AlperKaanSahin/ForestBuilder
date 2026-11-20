using TMPro;
using UnityEngine;

public class InventoryCounterUI : MonoBehaviour
{
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private TMP_Text inventoryText;

    private void OnEnable()
    {
        playerInventory.InventoryChanged += UpdateInventoryText;
    }

    private void Start()
    {
        UpdateInventoryText();
    }

    private void OnDisable()
    {
        playerInventory.InventoryChanged -= UpdateInventoryText;
    }

    private void UpdateInventoryText()
    {
        inventoryText.text =
            $"{playerInventory.TotalAmount} / {playerInventory.MaxCapacity}";
    }
}