using System.Collections.Generic;

public class RegionOffer
{
    public HexCoordinates Coordinates { get; }
    public IReadOnlyList<RegionData> Candidates { get; private set; }

    public RegionOffer(
        HexCoordinates coordinates,
        List<RegionData> candidates)
    {
        Coordinates = coordinates;
        Candidates = candidates;
    }

    public void UpdateCandidates(List<RegionData> candidates)
    {
        Candidates = candidates;
    }
}