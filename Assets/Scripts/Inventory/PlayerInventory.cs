using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private int maxWood = 20;

    private int currentWood;

    public int CurrentWood => currentWood;
    public int MaxWood => maxWood;
    public bool IsFull => currentWood >= maxWood;

    public event System.Action<int, int> WoodChanged;

    private void Start()
    {
        NotifyWoodChanged();
    }

    public bool TryAddWood(int amount)
    {
        if (amount <= 0)
            return false;

        if (currentWood + amount > maxWood)
            return false;

        currentWood += amount;

        NotifyWoodChanged();

        Debug.Log($"Wood: {currentWood} / {maxWood}");

        return true;
    }

    public int TakeAllWood()
    {
        int woodToTake = currentWood;

        currentWood = 0;

        NotifyWoodChanged();

        return woodToTake;
    }

    private void NotifyWoodChanged()
    {
        WoodChanged?.Invoke(currentWood, maxWood);
    }
}