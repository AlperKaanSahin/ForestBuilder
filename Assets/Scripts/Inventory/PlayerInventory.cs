using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private int maxCapacity = 20;

    private readonly Dictionary<ResourceDefinition, int> resources =
        new Dictionary<ResourceDefinition, int>();

    public int MaxCapacity => maxCapacity;
    public int TotalAmount => GetTotalAmount();
    public bool IsFull => TotalAmount >= maxCapacity;

    public event Action InventoryChanged;

    private void Start()
    {
        NotifyInventoryChanged();
    }

    public bool TryAddResource(ResourceDefinition resource, int amount)
    {
        if (resource == null || amount <= 0)
            return false;

        if (TotalAmount + amount > maxCapacity)
            return false;

        if (resources.ContainsKey(resource))
        {
            resources[resource] += amount;
        }
        else
        {
            resources.Add(resource, amount);
        }

        NotifyInventoryChanged();

        Debug.Log(
            $"{resource.DisplayName}: {resources[resource]} | " +
            $"Inventory: {TotalAmount}/{maxCapacity}"
        );

        return true;
    }

    public bool TryRemoveResource(ResourceDefinition resource, int amount)
    {
        if (resource == null || amount <= 0)
            return false;

        if (!resources.TryGetValue(resource, out int currentAmount))
            return false;

        if (currentAmount < amount)
            return false;

        currentAmount -= amount;

        if (currentAmount == 0)
            resources.Remove(resource);
        else
            resources[resource] = currentAmount;

        NotifyInventoryChanged();

        return true;
    }

    public int GetAmount(ResourceDefinition resource)
    {
        if (resource == null)
            return 0;

        return resources.TryGetValue(resource, out int amount)
            ? amount
            : 0;
    }

    public void IncreaseCapacity(int amount)
    {
        if (amount <= 0)
            return;

        maxCapacity += amount;

        NotifyInventoryChanged();

       
    }

    private int GetTotalAmount()
    {
        int total = 0;

        foreach (int amount in resources.Values)
            total += amount;

        return total;
    }

    private void NotifyInventoryChanged()
    {
        InventoryChanged?.Invoke();
    }
}