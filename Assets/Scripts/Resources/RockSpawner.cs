using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    [Header("Rock")]
    [SerializeField] private RockResource rockPrefab;
    [SerializeField] private int rockCount = 5;

    [Header("Scale")]
    [SerializeField] private float minScale = 0.8f;
    [SerializeField] private float maxScale = 1.2f;

    [Header("Stone Rewards")]
    [SerializeField] private Vector2Int smallStoneRange = new Vector2Int(2, 3);
    [SerializeField] private Vector2Int mediumStoneRange = new Vector2Int(3, 5);
    [SerializeField] private Vector2Int largeStoneRange = new Vector2Int(5, 7);

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

            return;
        }

        if (rockPrefab == null)
        {

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
            Quaternion.identity,
            transform
        );

        float scale = Random.Range(minScale, maxScale);

        int health;
        int stoneAmount;

        if (scale < 0.95f)
        {
            health = 4;
            stoneAmount = Random.Range(
                smallStoneRange.x,
                smallStoneRange.y + 1
            );
        }
        else if (scale < 1.1f)
        {
            health = 6;
            stoneAmount = Random.Range(
                mediumStoneRange.x,
                mediumStoneRange.y + 1
            );
        }
        else
        {
            health = 8;
            stoneAmount = Random.Range(
                largeStoneRange.x,
                largeStoneRange.y + 1
            );
        }

        rock.Initialize(health, scale);

        ResourceDropper resourceDropper =
            rock.GetComponent<ResourceDropper>();

        if (resourceDropper != null)
        {
            resourceDropper.Initialize(stoneAmount);
        }
    }
}