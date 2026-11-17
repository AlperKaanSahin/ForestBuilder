using UnityEngine;

public class WoodPickup : MonoBehaviour
{
    [SerializeField] private float pickupRadius = 1.5f;
    [SerializeField] private PlayerInventory playerInventory;

    private void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(
            transform.position,
            pickupRadius
        );

        foreach (Collider collider in colliders)
        {
            if (!collider.TryGetComponent<WoodPiece>(out WoodPiece woodPiece))
                continue;

            if (playerInventory.TryAddWood(1))
            {
                Destroy(woodPiece.gameObject);
            }
            else
            {
                break;
            }
        }
    }
}