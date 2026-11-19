using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    [Header("Rock")]
    [SerializeField] private RockResource rockPrefab;
    [SerializeField] private int rockCount = 5;

    [Header("Scale")]
    [SerializeField] private float minScale = 0.8f;
    [SerializeField] private float maxScale = 1.2f;

    private ResourceCluster cluster;

    private void Awake()
    {
        cluster = GetComponentInParent<ResourceCluster>();
    }

    private void Start()
    {
        SpawnRocks();
    }

    private void SpawnRocks()
    {
        if (cluster == null)
        {
            Debug.LogWarning($"{name}: ResourceCluster is missing.");
            return;
        }

        if (rockPrefab == null)
        {
            Debug.LogWarning($"{name}: Rock prefab is missing.");
            return;
        }

        for (int i = 0; i < rockCount; i++)
        {
            SpawnRock();
        }
    }

    private void SpawnRock()
    {
        Vector3 position = cluster.GetRandomPoint();

        RockResource rock = Instantiate(
            rockPrefab,
            position,
            Quaternion.Euler(
                0f,
                Random.Range(0f, 360f),
                0f
            ),
            transform
        );

        float scale = Random.Range(minScale, maxScale);
        rock.transform.localScale *= scale;

        rock.Initialize();
    }
}