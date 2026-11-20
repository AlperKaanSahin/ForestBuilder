using UnityEngine;

public class ResourceUnlockUpgrade : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerCurrency playerCurrency;
    [SerializeField] private ResourceUnlockSystem resourceUnlockSystem;

    [Header("Upgrade")]
    [SerializeField] private ResourceDefinition resourceToUnlock;
    [SerializeField] private int cost = 30;

    public int Cost => cost;
    public ResourceDefinition ResourceToUnlock => resourceToUnlock;

    public bool IsUnlocked =>
        resourceUnlockSystem.IsUnlocked(resourceToUnlock);

    public bool TryPurchase()
    {
        if (IsUnlocked)
            return false;

        if (!playerCurrency.TrySpendCoins(cost))
            return false;

        resourceUnlockSystem.Unlock(resourceToUnlock);

        return true;
    }
}