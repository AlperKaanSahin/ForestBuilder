using UnityEngine;

public class ResourcePiece : MonoBehaviour
{
    [SerializeField] private ResourceDefinition resource;
    [SerializeField] private int amount = 1;

    public ResourceDefinition Resource => resource;
    public int Amount => amount;
    public bool IsCollectible { get; private set; }

    private void Awake()
    {
        int resourceLayer = LayerMask.NameToLayer("Wood");

        if (resourceLayer != -1)
        {
            Physics.IgnoreLayerCollision(
                resourceLayer,
                resourceLayer,
                true
            );
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsCollectible = true;
        }
    }
}