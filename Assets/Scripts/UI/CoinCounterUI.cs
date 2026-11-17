using TMPro;
using UnityEngine;

public class CoinCounterUI : MonoBehaviour
{
    [SerializeField] private PlayerCurrency playerCurrency;
    [SerializeField] private TMP_Text coinText;

    private void OnEnable()
    {
        playerCurrency.CoinsChanged += UpdateCoinText;
    }

    private void Start()
    {
        UpdateCoinText(playerCurrency.Coins);
    }

    private void OnDisable()
    {
        playerCurrency.CoinsChanged -= UpdateCoinText;
    }

    private void UpdateCoinText(int coins)
    {
        coinText.text = $"Coins: {coins}";
    }
}