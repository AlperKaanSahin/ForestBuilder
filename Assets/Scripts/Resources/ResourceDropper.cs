using UnityEngine;

[RequireComponent(typeof(TreeResource))]
public class ResourceDropper : MonoBehaviour
{
    [Header("Drop")]
    [SerializeField] private Rigidbody woodPiecePrefab;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnHeight = 1f;
    [SerializeField] private float spawnRadius = 0.6f;
    [SerializeField] private float horizontalForce = 2f;
    [SerializeField] private float upwardForce = 2f;

    private TreeResource treeResource;

    private void Awake()
    {
        treeResource = GetComponent<TreeResource>();
    }

    private void OnEnable()
    {
        treeResource.Chopped += HandleTreeChopped;
    }

    private void OnDisable()
    {
        treeResource.Chopped -= HandleTreeChopped;
    }

    private void HandleTreeChopped()
    {
        DropWood(treeResource.WoodReward);
    }

    private void DropWood(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;

            Vector3 spawnPosition = transform.position + new Vector3(
                randomOffset.x,
                spawnHeight,
                randomOffset.y
            );

            Rigidbody woodPiece = Instantiate(
                woodPiecePrefab,
                spawnPosition,
                Random.rotation
            );

            Vector3 forceDirection = new Vector3(
                randomOffset.x,
                upwardForce,
                randomOffset.y
            ).normalized;

            woodPiece.AddForce(
                forceDirection * horizontalForce,
                ForceMode.Impulse
            );
        }
    }
}