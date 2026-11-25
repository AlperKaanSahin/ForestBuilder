using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RegionData
{
    [SerializeField] private BiomeDefinition biome;
    [SerializeField] private int tier;
    [SerializeField] private int seed;
    [SerializeField] private List<RegionFeatureDefinition> features = new();

    public BiomeDefinition Biome => biome;
    public int Tier => tier;
    public int Seed => seed;
    public IReadOnlyList<RegionFeatureDefinition> Features => features;

    public RegionData(
        BiomeDefinition biome,
        int tier,
        int seed,
        List<RegionFeatureDefinition> features)
    {
        this.biome = biome;
        this.tier = tier;
        this.seed = seed;
        this.features = features ?? new List<RegionFeatureDefinition>();
    }
}