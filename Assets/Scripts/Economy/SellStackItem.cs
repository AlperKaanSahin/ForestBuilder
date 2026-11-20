using UnityEngine;

public class SellStackItem
{
    public ResourceDefinition Resource { get; }
    public GameObject Visual { get; }

    public SellStackItem(
        ResourceDefinition resource,
        GameObject visual)
    {
        Resource = resource;
        Visual = visual;
    }
}