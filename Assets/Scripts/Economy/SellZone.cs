using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SellZone : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private PlayerCurrency playerCurrency;
    [SerializeField] private SellStackVisual sellStackVisual;
    [SerializeField] private CarryStackVisual carryStackVisual;
    [SerializeField] private SellCoinEffect sellCoinEffect;

    [Header("Transfer")]
    [SerializeField] private float transferInterval = 0.15f;
    [SerializeField] private float transferDuration = 0.25f;

    [Header("Selling")]
    [SerializeField] private int coinPerWood = 1;
    [SerializeField] private float startingSellInterval = 0.20f;
    [SerializeField] private float minimumSellInterval = 0.06f;
    [SerializeField] private float sellAcceleration = 0.02f;

    private BoxCollider zoneCollider;

    private bool playerInside;
    private int woodInSellStack;

    private Coroutine transferCoroutine;
    private Coroutine sellCoroutine;

    private void Awake()
    {
        zoneCollider = GetComponent<BoxCollider>();
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
            StartTransfer();
        }
        else if (!isInside && playerInside)
        {
            playerInside = false;
            StopTransfer();
        }
    }

    private void StartTransfer()
    {
        if (transferCoroutine != null)
            return;

        transferCoroutine = StartCoroutine(TransferWood());
    }

    private void StopTransfer()
    {
        if (transferCoroutine == null)
            return;

        StopCoroutine(transferCoroutine);
        transferCoroutine = null;
    }

    private IEnumerator TransferWood()
    {
        while (playerInside && playerInventory.CurrentWood > 0)
        {
            GameObject woodVisual = carryStackVisual.TakeLastVisual();

            if (playerInventory.TryRemoveWood(1))
            {
                if (woodVisual != null)
                {
                    yield return StartCoroutine(
                        MoveWoodToSellStack(woodVisual)
                    );
                }

                woodInSellStack++;
                sellStackVisual.AddWood();

                StartSelling();
            }

            yield return new WaitForSeconds(transferInterval);
        }

        transferCoroutine = null;
    }
    private IEnumerator MoveWoodToSellStack(GameObject woodVisual)
    {
        Vector3 startPosition = woodVisual.transform.position;
        Vector3 targetPosition = sellStackVisual.transform.position;

        Quaternion startRotation = woodVisual.transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(90f, 0f, 0f);

        woodVisual.transform.SetParent(null);

        float elapsedTime = 0f;

        while (elapsedTime < transferDuration)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(
                elapsedTime / transferDuration
            );

            t = Mathf.SmoothStep(0f, 1f, t);

            woodVisual.transform.position = Vector3.Lerp(
                startPosition,
                targetPosition,
                t
            );

            woodVisual.transform.rotation = Quaternion.Slerp(
                startRotation,
                targetRotation,
                t
            );

            yield return null;
        }

        Destroy(woodVisual);
    }

    private void StartSelling()
    {
        if (sellCoroutine != null)
            return;

        sellCoroutine = StartCoroutine(SellWood());
    }

    private IEnumerator SellWood()
    {
        float currentSellInterval = startingSellInterval;

        while (woodInSellStack > 0)
        {
            yield return new WaitForSeconds(currentSellInterval);

            GameObject woodVisual = sellStackVisual.RemoveWood();

            Vector3 coinStartPosition = sellStackVisual.transform.position;

            if (woodVisual != null)
            {
                coinStartPosition = woodVisual.transform.position;
                Destroy(woodVisual);
            }

            woodInSellStack--;

            if (sellCoinEffect != null)
            {
                yield return StartCoroutine(
                    sellCoinEffect.PlayCoinEffect(coinStartPosition)
                );
            }

            playerCurrency.AddCoins(coinPerWood);

            currentSellInterval = Mathf.Max(
                minimumSellInterval,
                currentSellInterval - sellAcceleration
            );
        }

        sellCoroutine = null;
    }
}