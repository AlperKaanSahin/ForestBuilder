public class CarryStackItem
{
    public ResourceDefinition Resource { get; }
    public UnityEngine.GameObject Visual { get; }

    public CarryStackItem(
        ResourceDefinition resource,
        UnityEngine.GameObject visual)
    {
        Resource = resource;
        Visual = visual;
    }
}