using System;
using UnityEngine;

public class RerollSystem : MonoBehaviour
{
    [Header("Starting Rerolls")]
    [SerializeField, Min(0)] private int startingRerolls = 2;

    private int availableRerolls;

    public int AvailableRerolls => availableRerolls;

    public event Action<int> RerollsChanged;

    private void Awake()
    {
        availableRerolls = startingRerolls;
    }

    public bool TrySpendReroll()
    {
        if (availableRerolls <= 0)
            return false;

        availableRerolls--;

        RerollsChanged?.Invoke(availableRerolls);

        return true;
    }

    public void GrantRerolls(int amount)
    {
        if (amount <= 0)
            return;

        availableRerolls += amount;

        RerollsChanged?.Invoke(availableRerolls);
    }
}