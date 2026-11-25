using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "RegionGenerationSettings",
    menuName = "Forest Builder/Region/Generation Settings"
)]
public class RegionGenerationSettings : ScriptableObject
{
    [Header("Available Biomes")]
    [SerializeField]
    private List<BiomeDefinition> biomes = new();

    [Header("Available Features")]
    [SerializeField]
    private List<RegionFeatureDefinition> features = new();

    public IReadOnlyList<BiomeDefinition> Biomes => biomes;
    public IReadOnlyList<RegionFeatureDefinition> Features => features;
}