using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CarryCapacityUpgrade : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private PlayerCurrency playerCurrency;
    [SerializeField] private UpgradeCoinEffect coinEffect;

    [Header("Upgrade")]
    [SerializeField] private int capacityIncrease = 5;
    [SerializeField] private int startingCost = 10;
    [SerializeField] private int costIncrease = 10;

    [Header("Payment")]
    [SerializeField] private float paymentInterval = 0.1f;

    private BoxCollider zoneCollider;
    private bool playerInside;
    private int currentCost;
    private int remainingCost;

    private Coroutine paymentCoroutine;

    public int CurrentCost => currentCost;
    public int RemainingCost => remainingCost;
    public int CapacityIncrease => capacityIncrease;

    private void Awake()
    {
        zoneCollider = GetComponent<BoxCollider>();

        currentCost = startingCost;
        remainingCost = currentCost;
    }

    private void Update()
    {
        if (player == null)
            return;

        Bounds bounds = zoneCollider.bounds;

        bool isInside =
            player.position.x >= bounds.min.x &&
            player.position.x <= bounds.max.x &&
            player.position.z >= bounds.min.z &&
            player.position.z <= bounds.max.z;

        if (isInside && !playerInside)
        {
            playerInside = true;
            StartPayment();
        }
        else if (!isInside && playerInside)
        {
            playerInside = false;
            StopPayment();
        }
    }

    private void StartPayment()
    {
        if (paymentCoroutine != null)
            return;

        paymentCoroutine = StartCoroutine(PayForUpgrade());
    }

    private void StopPayment()
    {
        if (paymentCoroutine == null)
            return;

        StopCoroutine(paymentCoroutine);
        paymentCoroutine = null;
    }

    private IEnumerator PayForUpgrade()
    {
        while (playerInside)
        {
            // Mevcut upgrade için ödeme yap.
            while (playerInside && remainingCost > 0)
            {
                if (!playerCurrency.TrySpendCoins(1))
                {
                    paymentCoroutine = null;
                    yield break;
                }

                remainingCost--;

                if (coinEffect != null)
                {
                    StartCoroutine(
                        coinEffect.PlayCoinEffect()
                    );
                }

                yield return new WaitForSeconds(paymentInterval);
            }

            // Oyuncu ödeme sırasında çıktıysa dur.
            if (!playerInside)
                break;

            // Ödeme tamamlandı.
            CompleteUpgrade();

            // Küçük bir ara verip yeni upgrade'in
            // ödemesine otomatik devam et.
            yield return new WaitForSeconds(paymentInterval);
        }

        paymentCoroutine = null;
    }

    private void CompleteUpgrade()
    {
        playerInventory.IncreaseCapacity(capacityIncrease);

        currentCost += costIncrease;
        remainingCost = currentCost;

        Debug.Log(
            $"Carry capacity upgraded to {playerInventory.MaxWood}. " +
            $"Next cost: {currentCost}"
        );
    }
}