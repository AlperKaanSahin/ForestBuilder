using UnityEngine;

public class PlayerCurrency : MonoBehaviour
{
    private int coins;

    public int Coins => coins;

    public event System.Action<int> CoinsChanged;

    private void Start()
    {
        NotifyCoinsChanged();
    }

    public void AddCoins(int amount)
    {
        if (amount <= 0)
            return;

        coins += amount;

        NotifyCoinsChanged();

        Debug.Log($"Coins: {coins}");
    }

    private void NotifyCoinsChanged()
    {
        CoinsChanged?.Invoke(coins);
    }
}