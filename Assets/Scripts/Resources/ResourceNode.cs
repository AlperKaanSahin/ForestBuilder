using System;
using UnityEngine;

public abstract class ResourceNode : MonoBehaviour
{
    [SerializeField] private ResourceDefinition resource;

    private int currentHealth;
    private bool isInitialized;
    private bool isDepleted;

    public ResourceDefinition Resource => resource;

    public event Action Depleted;

    protected void InitializeNode(int health)
    {
        currentHealth = health;
        isInitialized = true;
        isDepleted = false;
    }

    public bool TakeDamage(int damage)
    {
        if (!isInitialized || isDepleted || damage <= 0)
            return false;

        currentHealth -= damage;



        if (currentHealth <= 0)
        {
            Deplete();
            return true;
        }

        return false;
    }

    private void Deplete()
    {
        if (isDepleted)
            return;

        isDepleted = true;

        Depleted?.Invoke();

        Destroy(gameObject);
    }
}