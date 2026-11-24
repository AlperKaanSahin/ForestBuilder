using System.Collections.Generic;
using UnityEngine;

public class ResourceHarvester : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 4f;
    [SerializeField] private float harvestInterval = 0.5f;
    [SerializeField] private int damagePerHit = 1;

    public int DamagePerHit => damagePerHit;

    [SerializeField] private ResourceUnlockSystem resourceUnlockSystem;

    private readonly List<ResourceNode> resourcesInRange =
        new List<ResourceNode>();

    private ResourceNode currentResource;
    private float harvestTimer;

    private void Update()
    {
        DetectResources();

        if (currentResource == null)
        {
            SelectNextResource();
            return;
        }

        harvestTimer += Time.deltaTime;

        if (harvestTimer >= harvestInterval)
        {
            bool depleted =
                currentResource.TakeDamage(damagePerHit);

            if (depleted)
            {
                currentResource = null;
            }

            harvestTimer = 0f;
        }
    }

    public void IncreaseDamage(int amount)
    {
        if (amount <= 0)
            return;

        damagePerHit += amount;
    }
    private void DetectResources()
    {
        resourcesInRange.Clear();

        Collider[] colliders = Physics.OverlapSphere(
            transform.position,
            detectionRadius
        );

        foreach (Collider collider in colliders)
        {


            ResourceNode resource =
                collider.GetComponentInParent<ResourceNode>();

            if (resource != null &&
                !resourcesInRange.Contains(resource))
            {
                resourcesInRange.Add(resource);
            }
        }

        if (currentResource != null &&
            !resourcesInRange.Contains(currentResource))
        {
            currentResource = null;
            harvestTimer = 0f;
        }
    }

    private void SelectNextResource()
    {
        foreach (ResourceNode resource in resourcesInRange)
        {
            if (!resourceUnlockSystem.IsUnlocked(resource.Resource))
                continue;

            currentResource = resource;
            harvestTimer = 0f;



            return;
        }
    }
}