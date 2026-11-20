using System.Collections.Generic;
using UnityEngine;

public class ResourceUnlockSystem : MonoBehaviour
{
    [Header("Starting Resources")]
    [SerializeField] private ResourceDefinition[] initiallyUnlockedResources;

    private readonly HashSet<ResourceDefinition> unlockedResources =
        new HashSet<ResourceDefinition>();

    private void Awake()
    {
        foreach (ResourceDefinition resource in initiallyUnlockedResources)
        {
            if (resource != null)
            {
                unlockedResources.Add(resource);
            }
        }
    }

    public bool IsUnlocked(ResourceDefinition resource)
    {
        if (resource == null)
            return false;

        return unlockedResources.Contains(resource);
    }

    public bool Unlock(ResourceDefinition resource)
    {
        if (resource == null)
            return false;

        if (!unlockedResources.Add(resource))
            return false;
        return true;
    }
}