using UnityEngine;

public class ExpansionPoint : MonoBehaviour
{
    [SerializeField] private HexDirection direction;
    [SerializeField] private GameObject platformVisual;

    private Region ownerRegion;
    private RegionManager regionManager;

    public HexDirection Direction => direction;

    public void Initialize(Region region, RegionManager manager)
    {
        if (regionManager != null)
            regionManager.RegionRegistered -= OnRegionRegistered;

        ownerRegion = region;
        regionManager = manager;

        regionManager.RegionRegistered += OnRegionRegistered;

        RefreshAvailability();
    }

    public HexCoordinates GetTargetCoordinates()
    {
        return ownerRegion.Coordinates.GetNeighbor(direction);
    }

    private void OnRegionRegistered(Region newRegion)
    {
        RefreshAvailability();
    }

    private void RefreshAvailability()
    {
        if (ownerRegion == null || regionManager == null)
            return;

        HexCoordinates target = GetTargetCoordinates();

        bool isAvailable = IsAvailable;

        if (platformVisual != null)
            platformVisual.SetActive(isAvailable);
    }
    public bool IsAvailable
    {
        get
        {
            if (ownerRegion == null || regionManager == null)
                return false;

            return !regionManager.HasRegion(GetTargetCoordinates());
        }
    }

    private void OnDestroy()
    {
        if (regionManager != null)
            regionManager.RegionRegistered -= OnRegionRegistered;
    }
}