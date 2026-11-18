using UnityEngine;

public class WoodPiece : MonoBehaviour
{
    public bool IsCollectible { get; private set; }

    private void Awake()
    {
        int woodLayer = LayerMask.NameToLayer("Wood");

        if (woodLayer != -1)
        {
            Physics.IgnoreLayerCollision(
                woodLayer,
                woodLayer,
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