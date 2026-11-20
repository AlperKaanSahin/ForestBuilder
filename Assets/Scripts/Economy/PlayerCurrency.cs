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

      
    }

    private void NotifyCoinsChanged()
    {
        CoinsChanged?.Invoke(coins);
    }
    public bool TrySpendCoins(int amount)
    {
        if (amount <= 0)
            return false;

        if (coins < amount)
            return false;

        coins -= amount;

        NotifyCoinsChanged();


        return true;
    }
}