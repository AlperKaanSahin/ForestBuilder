using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    [Header("Tree")]
    [SerializeField] private TreeResource treePrefab;
    [SerializeField] private int treeCount = 15;

    [Header("Spawn Area")]
    [SerializeField] private float spawnRadius = 20f;

    [Header("Spawn Restrictions")]
    [SerializeField] private LayerMask noSpawnLayer;
    [SerializeField] private float noSpawnRadius = 1.5f;
    [SerializeField] private int maxSpawnAttempts = 30;

    [Header("Scale")]
    [SerializeField] private float minScale = 0.8f;
    [SerializeField] private float maxScale = 1.3f;

    [Header("Wood Rewards")]
    [SerializeField] private Vector2Int smallWoodRange = new Vector2Int(3, 4);
    [SerializeField] private Vector2Int mediumWoodRange = new Vector2Int(5, 6);
    [SerializeField] private Vector2Int largeWoodRange = new Vector2Int(7, 9);

    private void Start()
    {
        SpawnTrees();
    }

    private void SpawnTrees()
    {
        for (int i = 0; i < treeCount; i++)
        {
            SpawnTree();
        }
    }

    private void SpawnTree()
    {
        if (!TryGetSpawnPosition(out Vector3 spawnPosition))
        {
            Debug.LogWarning("Could not find a valid position for tree.");
            return;
        }

        TreeResource tree = Instantiate(
            treePrefab,
            spawnPosition,
            Quaternion.identity,
            transform
        );

        float scale = Random.Range(minScale, maxScale);

        int health;
        int woodAmount;

        if (scale < 0.95f)
        {
            health = 3;
            woodAmount = Random.Range(
                smallWoodRange.x,
                smallWoodRange.y + 1
            );
        }
        else if (scale < 1.15f)
        {
            health = 4;
            woodAmount = Random.Range(
                mediumWoodRange.x,
                mediumWoodRange.y + 1
            );
        }
        else
        {
            health = 5;
            woodAmount = Random.Range(
                largeWoodRange.x,
                largeWoodRange.y + 1
            );
        }

        tree.Initialize(health, woodAmount, scale);
    }

    private bool TryGetSpawnPosition(out Vector3 spawnPosition)
    {
        for (int attempt = 0; attempt < maxSpawnAttempts; attempt++)
        {
            Vector3 candidatePosition = GetRandomSpawnPosition();

            bool blocked = Physics.CheckSphere(
                candidatePosition,
                noSpawnRadius,
                noSpawnLayer
            );

            if (!blocked)
            {
                spawnPosition = candidatePosition;
                return true;
            }
        }

        spawnPosition = Vector3.zero;
        return false;
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float x;
        float z;

        do
        {
            x = Random.Range(-spawnRadius, spawnRadius);
            z = Random.Range(-spawnRadius, spawnRadius);
        }
        while (!IsInsideHex(x, z));

        return transform.position + new Vector3(x, 0f, z);
    }

    private bool IsInsideHex(float x, float z)
    {
        float absoluteX = Mathf.Abs(x);
        float absoluteZ = Mathf.Abs(z);

        return absoluteX <= spawnRadius &&
               absoluteZ <= Mathf.Sqrt(3f) * spawnRadius / 2f &&
               Mathf.Sqrt(3f) * absoluteX + absoluteZ
                   <= Mathf.Sqrt(3f) * spawnRadius;
    }
}