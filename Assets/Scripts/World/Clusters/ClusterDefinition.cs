using System;
using UnityEngine;

[Serializable]
public class ClusterDefinition
{
    [SerializeField] private ResourceCluster prefab;
    [SerializeField] private int minCount = 1;
    [SerializeField] private int maxCount = 1;

    public ResourceCluster Prefab => prefab;
    public int MinCount => minCount;
    public int MaxCount => maxCount;
}