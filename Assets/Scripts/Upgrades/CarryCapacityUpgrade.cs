using UnityEngine;

public class CarryCapacityUpgrade : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private PlayerCurrency playerCurrency;

    [Header("Upgrade")]
    [SerializeField] private int capacityIncrease = 5;
    [SerializeField] private int startingCost = 10;
    [SerializeField] private int costIncrease = 10;

    private int currentCost;

    public int CurrentCost => currentCost;
    public int CapacityIncrease => capacityIncrease;

    private void Awake()
    {
        currentCost = startingCost;
    }

    public bool TryPurchase()
    {
        if (!playerCurrency.TrySpendCoins(currentCost))
            return false;

        playerInventory.IncreaseCapacity(capacityIncrease);

        currentCost += costIncrease;

        return true;
    }
}