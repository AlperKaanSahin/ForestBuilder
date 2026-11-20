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
    [SerializeField] private ResourceValueCalculator resourceValueCalculator;

    [Header("Transfer")]
    [SerializeField] private float transferInterval = 0.15f;
    [SerializeField] private float transferDuration = 0.25f;

    [Header("Selling")]
    [SerializeField] private float startingSellInterval = 0.20f;
    [SerializeField] private float minimumSellInterval = 0.06f;
    [SerializeField] private float sellAcceleration = 0.02f;

    private BoxCollider zoneCollider;

    private bool playerInside;

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

        transferCoroutine = StartCoroutine(TransferResources());
    }

    private void StopTransfer()
    {
        if (transferCoroutine == null)
            return;

        StopCoroutine(transferCoroutine);
        transferCoroutine = null;
    }

    private IEnumerator TransferResources()
    {
        while (playerInside && carryStackVisual.Count > 0)
        {
            CarryStackItem carryItem =
                carryStackVisual.TakeLastItem();

            if (carryItem == null)
                break;

            if (playerInventory.TryRemoveResource(
                    carryItem.Resource,
                    1))
            {
                if (carryItem.Visual != null)
                {
                    yield return StartCoroutine(
                        MoveResourceToSellStack(
                            carryItem.Visual
                        )
                    );
                }

                sellStackVisual.AddResource(
                    carryItem.Resource
                );

                StartSelling();
            }
            else
            {
                Debug.LogWarning(
                    $"Could not remove " +
                    $"{carryItem.Resource.DisplayName} " +
                    $"from inventory."
                );

                break;
            }

            yield return new WaitForSeconds(
                transferInterval
            );
        }

        transferCoroutine = null;
    }

    private IEnumerator MoveResourceToSellStack(
        GameObject resourceVisual)
    {
        Vector3 startPosition =
            resourceVisual.transform.position;

        Vector3 targetPosition =
            sellStackVisual.transform.position;

        Quaternion startRotation =
            resourceVisual.transform.rotation;

        Quaternion targetRotation =
            Quaternion.Euler(90f, 0f, 0f);

        resourceVisual.transform.SetParent(null);

        float elapsedTime = 0f;

        while (elapsedTime < transferDuration)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(
                elapsedTime / transferDuration
            );

            t = Mathf.SmoothStep(0f, 1f, t);

            resourceVisual.transform.position =
                Vector3.Lerp(
                    startPosition,
                    targetPosition,
                    t
                );

            resourceVisual.transform.rotation =
                Quaternion.Slerp(
                    startRotation,
                    targetRotation,
                    t
                );

            yield return null;
        }

        Destroy(resourceVisual);
    }

    private void StartSelling()
    {
        if (sellCoroutine != null)
            return;

        sellCoroutine = StartCoroutine(
            SellResources()
        );
    }

    private IEnumerator SellResources()
    {
        float currentSellInterval =
            startingSellInterval;

        while (sellStackVisual.Count > 0)
        {
            yield return new WaitForSeconds(
                currentSellInterval
            );

            SellStackItem sellItem =
                sellStackVisual.RemoveLastItem();

            if (sellItem == null)
                break;

            Vector3 coinStartPosition =
                sellStackVisual.transform.position;

            if (sellItem.Visual != null)
            {
                coinStartPosition =
                    sellItem.Visual.transform.position;

                Destroy(sellItem.Visual);
            }

            if (sellCoinEffect != null)
            {
                yield return StartCoroutine(
                    sellCoinEffect.PlayCoinEffect(
                        coinStartPosition
                    )
                );
            }

            int sellValue =
                resourceValueCalculator.GetSellValue(
                    sellItem.Resource
                );

            playerCurrency.AddCoins(sellValue);

            currentSellInterval = Mathf.Max(
                minimumSellInterval,
                currentSellInterval - sellAcceleration
            );
        }

        sellCoroutine = null;
    }
}