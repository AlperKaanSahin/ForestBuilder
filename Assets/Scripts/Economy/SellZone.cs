using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SellZone : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private PlayerCurrency playerCurrency;

    private BoxCollider zoneCollider;
    private bool playerInside;

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
            SellWood();
        }
        else if (!isInside && playerInside)
        {
            playerInside = false;
        }
    }

    private void SellWood()
    {
        int woodSold = playerInventory.TakeAllWood();

        if (woodSold <= 0)
            return;

        playerCurrency.AddCoins(woodSold);

        Debug.Log($"Sold {woodSold} wood for {woodSold} coins");
    }
}