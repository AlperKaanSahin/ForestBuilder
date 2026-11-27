using System;
using UnityEngine;

public abstract class RepeatableUpgrade : MonoBehaviour
{
    [Header("Common References")]
    [SerializeField] private PlayerCurrency playerCurrency;

    [Header("Progression")]
    [SerializeField] private int startingCost = 10;
    [SerializeField] private int costIncrease = 10;
    [SerializeField] private int maxLevel = 10;

    private int currentLevel;
    private int currentCost;

    public int CurrentLevel => currentLevel;
    public int MaxLevel => maxLevel;
    public int CurrentCost => currentCost;
    public bool IsMaxLevel => currentLevel >= maxLevel;

    public abstract string DisplayName { get; }
    public abstract string ProgressText { get; }

    public event Action<int> LevelChanged;

    protected virtual void Awake()
    {
        currentLevel = 0;
        currentCost = startingCost;
    }

    public bool TryPurchase()
    {
        if (IsMaxLevel)
            return false;

        if (!playerCurrency.TrySpendCoins(currentCost))
            return false;

        ApplyUpgrade();

        currentLevel++;
        currentCost = GetNextCost();

        LevelChanged?.Invoke(currentLevel);

        return true;
    }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
    public bool DebugLevelUp()
    {
        if (IsMaxLevel)
            return false;

        ApplyUpgrade();

        currentLevel++;
        currentCost = GetNextCost();

        LevelChanged?.Invoke(currentLevel);

        return true;
    }
#endif

    protected abstract void ApplyUpgrade();

    protected virtual int GetNextCost()
    {
        return startingCost + (currentLevel * costIncrease);
    }
}