using System;
using System.Collections.Generic;
using UnityEngine;

public class RegionCandidateGenerator : MonoBehaviour
{
    [SerializeField] private RegionGenerationSettings settings;
    [SerializeField, Min(1)] private int candidateCount = 3;

    public List<RegionData> GenerateCandidates(int tier, int seed)
    {
        List<RegionData> candidates = new();

        if (settings == null)
        {
            Debug.LogError("Region generation settings missing.");
            return candidates;
        }

        System.Random random = new System.Random(seed);

        List<BiomeDefinition> availableBiomes = new();

        foreach (BiomeDefinition biome in settings.Biomes)
        {
            if (biome != null && !availableBiomes.Contains(biome))
                availableBiomes.Add(biome);
        }

        int count = Mathf.Min(candidateCount, availableBiomes.Count);

        for (int i = 0; i < count; i++)
        {
            int biomeIndex = random.Next(availableBiomes.Count);

            BiomeDefinition selectedBiome = availableBiomes[biomeIndex];
            availableBiomes.RemoveAt(biomeIndex);

            List<RegionFeatureDefinition> features =
                SelectFeatures(tier, random);

            RegionData candidate = new RegionData(
                selectedBiome,
                tier,
                random.Next(),
                features
            );

            candidates.Add(candidate);
        }

        return candidates;
    }

    private List<RegionFeatureDefinition> SelectFeatures(
        int tier,
        System.Random random)
    {
        List<RegionFeatureDefinition> selected = new();
        List<RegionFeatureDefinition> eligible = new();

        foreach (RegionFeatureDefinition feature in settings.Features)
        {
            if (feature == null)
                continue;

            if (feature.MinimumTier > tier)
                continue;

            if (feature.SelectionWeight <= 0f)
                continue;

            eligible.Add(feature);
        }

        if (eligible.Count == 0)
            return selected;

        // Şimdilik her adayda en fazla bir feature bulunabilir.
        float totalWeight = 0f;

        foreach (RegionFeatureDefinition feature in eligible)
            totalWeight += feature.SelectionWeight;

        double roll = random.NextDouble() * totalWeight;

        foreach (RegionFeatureDefinition feature in eligible)
        {
            roll -= feature.SelectionWeight;

            if (roll < 0)
            {
                selected.Add(feature);
                break;
            }
        }

        return selected;
    }

    [ContextMenu("Test Candidate Generation")]
    private void TestCandidates()
    {
        List<RegionData> candidates = GenerateCandidates(1, 12345);

        foreach (RegionData candidate in candidates)
        {
            string featureName = candidate.Features.Count > 0
                ? candidate.Features[0].DisplayName
                : "None";

            Debug.Log(
                $"Biome: {candidate.Biome.DisplayName} | " +
                $"Tier: {candidate.Tier} | " +
                $"Feature: {featureName} | " +
                $"Seed: {candidate.Seed}"
            );
        }
    }
}