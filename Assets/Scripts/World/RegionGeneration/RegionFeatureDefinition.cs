using UnityEngine;

[CreateAssetMenu(
    fileName = "RegionFeatureDefinition",
    menuName = "Forest Builder/Region/Feature Definition"
)]
public class RegionFeatureDefinition : ScriptableObject
{
    [Header("Identity")]
    [SerializeField] private string displayName;

    [Header("Generation")]
    [Min(0)]
    [SerializeField] private int minimumTier = 1;

    [Min(0f)]
    [SerializeField] private float selectionWeight = 1f;

    public string DisplayName => displayName;
    public int MinimumTier => minimumTier;
    public float SelectionWeight => selectionWeight;
}