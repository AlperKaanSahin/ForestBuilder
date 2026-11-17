using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    [Header("Tree")]
    [SerializeField] private TreeResource treePrefab;
    [SerializeField] private int treeCount = 15;

    [Header("Spawn Area")]
    [SerializeField] private Vector2 spawnAreaSize = new Vector2(40f, 40f);

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
        Vector3 spawnPosition = GetRandomSpawnPosition();

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

    private Vector3 GetRandomSpawnPosition()
    {
        float x = Random.Range(
            -spawnAreaSize.x / 2f,
            spawnAreaSize.x / 2f
        );

        float z = Random.Range(
            -spawnAreaSize.y / 2f,
            spawnAreaSize.y / 2f
        );

        return transform.position + new Vector3(x, 0f, z);
    }
}