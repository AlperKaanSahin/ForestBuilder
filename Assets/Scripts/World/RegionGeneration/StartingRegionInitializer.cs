using System.Collections.Generic;
using UnityEngine;

public class StartingRegionInitializer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Region region;
    [SerializeField] private RegionGenerator regionGenerator;
    [SerializeField] private ExpansionUI expansionUI;

    [Header("Starting Region")]
    [SerializeField] private BiomeDefinition biome;
    [SerializeField] private int seed = 12345;
    [SerializeField] private RegionManager regionManager;



    private void Start()
    {
        RegionData data = new RegionData(
            biome,
            0,
            seed,
            new List<RegionFeatureDefinition>()
        );

        region.Initialize(
            new HexCoordinates(0, 0),
            data
        );

        if (!regionManager.RegisterRegion(region))
        {
            Debug.LogError("Starting region registration failed.");
            return;
        }
        RegionBoundary boundary =
    region.GetComponentInChildren<RegionBoundary>(true);

        if (boundary != null)
        {
            boundary.Initialize(region, regionManager);
        }

        regionGenerator.Generate();

        ExpansionPoint[] points =
            region.GetComponentsInChildren<ExpansionPoint>(true);

        foreach (ExpansionPoint point in points)
        {
            point.Initialize(region, regionManager);

            ExpansionInteractionZone interactionZone =
                point.GetComponentInChildren<ExpansionInteractionZone>(true);

            if (interactionZone != null)
            {
                interactionZone.Initialize(point, expansionUI);
            }
        }

    }
}