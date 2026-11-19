using UnityEngine;

public class DecorationSpawner : MonoBehaviour
{
    [Header("Decoration Prefabs")]
    [SerializeField] private GameObject[] grassPrefabs;
    [SerializeField] private GameObject[] bushPrefabs;
    [SerializeField] private GameObject[] rockPrefabs;
    [SerializeField] private GameObject[] flowerPrefabs;

    [Header("Decoration Counts")]
    [SerializeField] private int grassCount = 30;
    [SerializeField] private int bushCount = 10;
    [SerializeField] private int rockCount = 6;
    [SerializeField] private int flowerCount = 8;

    [Header("Spawn Area")]
    [SerializeField] private float spawnRadius = 20f;

    [Header("Spawn Restrictions")]
    [SerializeField] private LayerMask noSpawnLayer;
    [SerializeField] private float noSpawnRadius = 0.75f;
    [SerializeField] private int maxSpawnAttempts = 30;

    [Header("Scale Variation")]
    [SerializeField] private float minScale = 0.8f;
    [SerializeField] private float maxScale = 1.2f;

    private void Start()
    {
        Debug.Log("DecorationSpawner START çalıştı");

        SpawnDecorations(grassPrefabs, grassCount);
        SpawnDecorations(bushPrefabs, bushCount);
        SpawnDecorations(rockPrefabs, rockCount);
        SpawnDecorations(flowerPrefabs, flowerCount);
    }
    private void SpawnDecorations(GameObject[] prefabs, int count)
    {
        if (prefabs == null || prefabs.Length == 0)
            return;

        for (int i = 0; i < count; i++)
        {
            if (!TryGetSpawnPosition(out Vector3 spawnPosition))
                continue;

            GameObject prefab = prefabs[
                Random.Range(0, prefabs.Length)
            ];

            GameObject decoration = Instantiate(
                prefab,
                spawnPosition,
                Quaternion.identity,
                transform
            );

            decoration.transform.rotation = Quaternion.Euler(
                0f,
                Random.Range(0f, 360f),
                0f
            );

            float scale = Random.Range(minScale, maxScale);
            decoration.transform.localScale *= scale;
        }
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