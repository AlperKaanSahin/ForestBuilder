using UnityEngine;
using System.Collections.Generic;

public class RegionGenerator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Region region;

    [Header("Cluster Generation")]
    [SerializeField] private Transform clusterParent;


    [Header("Placement Rules")]
    [SerializeField] private ReservedArea[] reservedAreas;

    [SerializeField] private int maxPlacementAttempts = 50;

    [Header("Cluster Generation")]

    private readonly List<ResourceCluster> placedClusters =
    new List<ResourceCluster>();

    public void Generate()
    {
        GenerateClusters();
    }

    private void GenerateClusters()
    {
        if (region == null || region.Data == null || region.Data.Biome == null)
        {
            Debug.LogWarning("RegionGenerator region data is missing.");
            return;
        }

        foreach (ClusterDefinition definition in region.Data.Biome.ClusterDefinitions)
        {
            if (definition == null || definition.Prefab == null)
                continue;

            int clusterCount = Random.Range(
                definition.MinCount,
                definition.MaxCount + 1
            );

            for (int i = 0; i < clusterCount; i++)
            {
                ResourceCluster cluster = Instantiate(
                    definition.Prefab,
                    clusterParent
                );

                if (!TryPlaceCluster(cluster))
                {
                    Destroy(cluster.gameObject);

                    Debug.LogWarning(
                        $"Could not place cluster: {definition.Prefab.name}"
                    );
                }
            }
        }
    }

    private bool TryPlaceCluster(ResourceCluster cluster)
    {
        for (int attempt = 0; attempt < maxPlacementAttempts; attempt++)
        {
            if (!region.TryGetRandomPoint(
                    cluster.Radius,
                    out Vector3 position))
            {
                continue;
            }

            if (!region.CanFitCircle(position, cluster.Radius))
                continue;

            if (OverlapsReservedArea(position, cluster.Radius))
                continue;

            if (OverlapsPlacedCluster(position, cluster.Radius))
                continue;

            cluster.transform.position = position;
            placedClusters.Add(cluster);

            return true;
        }

        return false;
    }

    private bool OverlapsPlacedCluster(
        Vector3 position,
        float radius)
    {
        foreach (ResourceCluster other in placedClusters)
        {
            if (other == null)
                continue;

            Vector2 currentPosition =
                new Vector2(position.x, position.z);

            Vector2 otherPosition =
                new Vector2(other.Center.x, other.Center.z);

            float minimumDistance =
                radius + other.Radius;

            if (Vector2.Distance(
                    currentPosition,
                    otherPosition) < minimumDistance)
            {
                return true;
            }
        }

        return false;
    }

    private bool OverlapsReservedArea(
    Vector3 position,
    float clusterRadius
)
    {
        if (reservedAreas == null)
            return false;

        foreach (ReservedArea reservedArea in reservedAreas)
        {
            if (reservedArea == null)
                continue;

            if (reservedArea.Overlaps(position, clusterRadius))
                return true;
        }

        return false;
    }
}