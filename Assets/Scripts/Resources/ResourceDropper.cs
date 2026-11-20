using UnityEngine;

[RequireComponent(typeof(ResourceNode))]
public class ResourceDropper : MonoBehaviour
{
    [Header("Drop")]
    [SerializeField] private Rigidbody resourcePiecePrefab;
    [SerializeField] private int dropCount = 1;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnHeight = 0.7f;
    [SerializeField] private float spawnRadius = 0.4f;
    private ResourceNode resourceNode;

    private void Awake()
    {
        resourceNode = GetComponent<ResourceNode>();
    }

    private void OnEnable()
    {
        resourceNode.Depleted += HandleResourceDepleted;
    }

    private void OnDisable()
    {
        resourceNode.Depleted -= HandleResourceDepleted;
    }

    private void HandleResourceDepleted()
    {
        DropResource();
    }
    public void Initialize(int amount)
    {
        dropCount = Mathf.Max(1, amount);
    }

    private void DropResource()
    {
        for (int i = 0; i < dropCount; i++)
        {
            Vector2 randomOffset =
                Random.insideUnitCircle * spawnRadius;

            Vector3 spawnPosition =
                transform.position +
                new Vector3(
                    randomOffset.x,
                    spawnHeight,
                    randomOffset.y
                );

            Instantiate(
                resourcePiecePrefab,
                spawnPosition,
                Random.rotation
            );
        }
    }
}