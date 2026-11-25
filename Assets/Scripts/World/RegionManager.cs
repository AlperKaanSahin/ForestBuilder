using System;
using System.Collections.Generic;
using UnityEngine;

public class RegionManager : MonoBehaviour
{
    private readonly Dictionary<Vector2Int, Region> regions = new();
    public event Action<Region> RegionRegistered;

    public bool RegisterRegion(Region region)
    {
        if (region == null)
            return false;

        Vector2Int key = ToKey(region.Coordinates);

        if (regions.ContainsKey(key))
        {
            Debug.LogWarning($"Region already exists at {key}");
            return false;
        }

        regions.Add(key, region);
        RegionRegistered?.Invoke(region);

        Debug.Log($"Region registered: {key}");
        return true;
    }

    public bool HasRegion(HexCoordinates coordinates)
    {
        return regions.ContainsKey(ToKey(coordinates));
    }

    public bool TryGetRegion(
        HexCoordinates coordinates,
        out Region region)
    {
        return regions.TryGetValue(ToKey(coordinates), out region);
    }

    private Vector2Int ToKey(HexCoordinates coordinates)
    {
        return new Vector2Int(coordinates.Q, coordinates.R);
    }
}