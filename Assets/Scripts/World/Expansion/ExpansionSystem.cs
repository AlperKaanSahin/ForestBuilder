using System.Collections.Generic;
using UnityEngine;

public class ExpansionSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerCurrency playerCurrency;
    [SerializeField] private RegionFactory regionFactory;
    [SerializeField] private RegionCandidateGenerator candidateGenerator;
    [SerializeField] private RerollSystem rerollSystem;

    [Header("Expansion Cost")]
    [SerializeField, Min(1)] private int startingCost = 50;
    [SerializeField, Min(0)] private int costIncrease = 50;

    private int expansionCount;

    public int ExpansionCount => expansionCount;
    private readonly Dictionary<Vector2Int, RegionOffer> offers = new();

    public int CurrentCost =>
        startingCost + expansionCount * costIncrease;

    public bool CanAffordExpansion =>
        playerCurrency != null &&
        playerCurrency.Coins >= CurrentCost;

    public bool TryExpand(
        HexCoordinates coordinates,
        RegionData data)
    {
        if (playerCurrency == null || regionFactory == null)
        {
            Debug.LogError("ExpansionSystem references are missing.");
            return false;
        }

        if (data == null || data.Biome == null)
            return false;

        int cost = CurrentCost;

        if (!playerCurrency.TrySpendCoins(cost))
        {
            Debug.Log("Not enough coins to expand.");
            return false;
        }

        Region newRegion = regionFactory.CreateRegion(
            coordinates,
            data
        );

        if (newRegion == null)
        {
            playerCurrency.AddCoins(cost);

            Debug.LogWarning(
                "Expansion failed. Coins refunded."
            );

            return false;
        }

        expansionCount++;

        Debug.Log(
            $"Expansion successful | " +
            $"Cost: {cost} | " +
            $"Total expansions: {expansionCount} | " +
            $"Next cost: {CurrentCost}"
        );

        return true;
    }
    public RegionOffer GetOrCreateOffer(HexCoordinates coordinates)
    {
        Vector2Int key = new Vector2Int(
            coordinates.Q,
            coordinates.R
        );

        if (offers.TryGetValue(key, out RegionOffer existingOffer))
            return existingOffer;

        if (candidateGenerator == null)
        {
            Debug.LogError("CandidateGenerator reference is missing.");
            return null;
        }

        int seed = Random.Range(0, int.MaxValue);

        List<RegionData> candidates =
            candidateGenerator.GenerateCandidates(1, seed);

        if (candidates.Count == 0)
        {
            Debug.LogWarning("No candidates generated.");
            return null;
        }

        RegionOffer offer = new RegionOffer(
            coordinates,
            candidates
        );

        offers.Add(key, offer);

        return offer;
    }
    public bool TryReroll(HexCoordinates coordinates)
    {
        Vector2Int key = new Vector2Int(
            coordinates.Q,
            coordinates.R
        );

        if (!offers.TryGetValue(key, out RegionOffer offer))
            return false;

        if (candidateGenerator == null || rerollSystem == null)
            return false;

        if (rerollSystem.AvailableRerolls <= 0)
            return false;

        int seed = Random.Range(0, int.MaxValue);

        List<RegionData> newCandidates =
            candidateGenerator.GenerateCandidates(1, seed);

        if (newCandidates.Count == 0)
            return false;

        if (!rerollSystem.TrySpendReroll())
            return false;

        offer.UpdateCandidates(newCandidates);

        Debug.Log(
            $"Region offer rerolled: ({coordinates.Q}, {coordinates.R})"
        );

        return true;
    }
}