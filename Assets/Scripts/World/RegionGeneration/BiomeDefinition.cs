using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "BiomeDefinition",
    menuName = "Forest Builder/Region/Biome Definition"
)]
public class BiomeDefinition : ScriptableObject
{
    [Header("Identity")]
    [SerializeField] private string displayName;

    [Header("Cluster Generation")]
    [SerializeField] private List<ClusterDefinition> clusterDefinitions = new();

    public string DisplayName => displayName;
    public IReadOnlyList<ClusterDefinition> ClusterDefinitions => clusterDefinitions;
}