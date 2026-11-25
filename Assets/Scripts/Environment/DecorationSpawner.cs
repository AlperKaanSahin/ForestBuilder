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

    [Header("References")]
    [SerializeField] private Region region;

    [Header("Spawn Restrictions")]
    [SerializeField] private LayerMask noSpawnLayer;
    [SerializeField] private float noSpawnRadius = 0.75f;
    [SerializeField] private int maxSpawnAttempts = 30;

    [Header("Scale Variation")]
    [SerializeField] private float minScale = 0.8f;
    [SerializeField] private float maxScale = 1.2f;

    private void Start()
    {
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
        if (region == null)
        {
            spawnPosition = Vector3.zero;
            return false;
        }

        for (int attempt = 0; attempt < maxSpawnAttempts; attempt++)
        {
            if (!region.TryGetRandomPoint(
                    noSpawnRadius,
                    out Vector3 candidatePosition))
            {
                continue;
            }

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


}