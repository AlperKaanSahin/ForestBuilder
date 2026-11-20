using UnityEngine;

public class ResourcePickup : MonoBehaviour
{
    [SerializeField] private float pickupRadius = 1.5f;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private CarryStackVisual carryStackVisual;

    private void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(
            transform.position,
            pickupRadius
        );

        foreach (Collider collider in colliders)
        {
            if (!collider.TryGetComponent<ResourcePiece>(
                    out ResourcePiece resourcePiece))
            {
                continue;
            }

            if (!resourcePiece.IsCollectible)
                continue;

            if (playerInventory.TryAddResource(
                    resourcePiece.Resource,
                    resourcePiece.Amount))
            {
                for (int i = 0; i < resourcePiece.Amount; i++)
                {
                    carryStackVisual.AddResource(
                        resourcePiece.Resource
                    );
                }

                Destroy(resourcePiece.gameObject);
            }
            else
            {
                break;
            }
        }
    }
}