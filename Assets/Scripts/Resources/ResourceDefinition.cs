using UnityEngine;

[CreateAssetMenu(
    fileName = "ResourceDefinition",
    menuName = "Forest Builder/Resource Definition"
)]
public class ResourceDefinition : ScriptableObject
{
    [Header("Identity")]
    [SerializeField] private string displayName;

    [Header("Economy")]
    [SerializeField] private int baseSellValue = 1;

    [Header("Visual")]
    [SerializeField] private GameObject stackVisualPrefab;

    public string DisplayName => displayName;
    public int BaseSellValue => baseSellValue;
    public GameObject StackVisualPrefab => stackVisualPrefab;
}