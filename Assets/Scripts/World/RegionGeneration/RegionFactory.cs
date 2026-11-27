using System.Collections.Generic;
using UnityEngine;

public class RegionFactory : MonoBehaviour
{
    [Header("Region")]
    [SerializeField] private Region regionPrefab;
    [SerializeField] private Transform regionParent;
    [SerializeField] private RegionManager regionManager;

    [Header("Player References")]
    [SerializeField] private Transform player;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private PlayerCurrency playerCurrency;
    [SerializeField] private CarryStackVisual carryStackVisual;
    [SerializeField] private ExpansionUI expansionUI;

    [Header("Shared Systems")]
    [SerializeField] private ResourceValueCalculator valueCalculator;
    [SerializeField] private GameObject upgradePanel;

    [Header("Temporary Test")]
    [SerializeField] private BiomeDefinition testBiome;

    public Region CreateRegion(
        HexCoordinates coordinates,
        RegionData data)
    {
        if (regionPrefab == null ||
            regionManager == null ||
            data == null ||
            data.Biome == null)
        {
            Debug.LogError("RegionFactory: Missing required data.");
            return null;
        }

        if (regionManager.HasRegion(coordinates))
        {
            Debug.LogWarning("Region already exists.");
            return null;
        }

        Region newRegion = Instantiate(
            regionPrefab,
            regionParent
        );

        newRegion.gameObject.SetActive(false);

        newRegion.Initialize(coordinates, data);

        InitializeSystems(newRegion);

        if (!regionManager.RegisterRegion(newRegion))
        {
            Destroy(newRegion.gameObject);
            return null;
        }
        RegionBoundary boundary =
    newRegion.GetComponentInChildren<RegionBoundary>(true);

        if (boundary != null)
        {
            boundary.Initialize(newRegion, regionManager);
        }

        ExpansionPoint[] points =
            newRegion.GetComponentsInChildren<ExpansionPoint>(true);

        foreach (ExpansionPoint point in points)
        {
            point.Initialize(newRegion, regionManager);

            ExpansionInteractionZone interactionZone =
                point.GetComponentInChildren<ExpansionInteractionZone>(true);

            if (interactionZone != null)
            {
                interactionZone.Initialize(point, expansionUI);
            }
        }

        newRegion.gameObject.name =
            $"Region_{coordinates.Q}_{coordinates.R}";

        newRegion.gameObject.SetActive(true);

        return newRegion;
    }

    private void InitializeSystems(Region region)
    {
        SellZone sellZone =
            region.GetComponentInChildren<SellZone>(true);

        if (sellZone != null)
        {
            sellZone.Initialize(
                player,
                playerInventory,
                playerCurrency,
                carryStackVisual,
                valueCalculator
            );
        }

        UpgradeZone upgradeZone =
            region.GetComponentInChildren<UpgradeZone>(true);

        if (upgradeZone != null)
            upgradeZone.Initialize(player, upgradePanel);

        SellCoinEffect coinEffect =
            region.GetComponentInChildren<SellCoinEffect>(true);

        if (coinEffect != null)
            coinEffect.Initialize(player);
    }

    [ContextMenu("Test Create North Region")]
    private void TestCreateNorthRegion()
    {
        if (!Application.isPlaying)
        {
            Debug.LogWarning("Run this test in Play mode.");
            return;
        }

        if (testBiome == null)
        {
            Debug.LogError("Test biome is missing.");
            return;
        }

        RegionData data = new RegionData(
            testBiome,
            1,
            12345,
            new List<RegionFeatureDefinition>()
        );

        Region result = CreateRegion(
            new HexCoordinates(0, -1),
            data
        );

        if (result != null)
            Debug.Log("North region created successfully.");
    }
}