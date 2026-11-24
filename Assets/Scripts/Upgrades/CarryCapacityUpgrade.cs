using UnityEngine;

public class CarryCapacityUpgrade : RepeatableUpgrade
{
    [Header("References")]
    [SerializeField] private PlayerInventory playerInventory;

    [Header("Carry Capacity")]
    [SerializeField] private int capacityIncrease = 5;

    public int CapacityIncrease => capacityIncrease;

    public override string DisplayName => "Carry Capacity";

    public override string ProgressText =>
        $"{playerInventory.MaxCapacity} → " +
        $"{playerInventory.MaxCapacity + capacityIncrease} Capacity";

    protected override void ApplyUpgrade()
    {
        playerInventory.IncreaseCapacity(capacityIncrease);
    }
}