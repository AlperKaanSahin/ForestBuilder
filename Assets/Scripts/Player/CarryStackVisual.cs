using System.Collections.Generic;
using UnityEngine;

public class CarryStackVisual : MonoBehaviour
{
    [SerializeField] private float verticalSpacing = 0.25f;

    private readonly List<CarryStackItem> items =
        new List<CarryStackItem>();

    public int Count => items.Count;

    public void AddResource(ResourceDefinition resource)
    {
        if (resource == null || resource.StackVisualPrefab == null)
            return;

        int index = items.Count;

        GameObject visual = Instantiate(
            resource.StackVisualPrefab,
            transform
        );

        visual.transform.localPosition = new Vector3(
            0f,
            index * verticalSpacing,
            0f
        );

        visual.transform.localRotation = Quaternion.Euler(
            0f,
            0f,
            90f
        );

        items.Add(
            new CarryStackItem(resource, visual)
        );
    }

    public CarryStackItem TakeLastItem()
    {
        if (items.Count == 0)
            return null;

        int lastIndex = items.Count - 1;

        CarryStackItem item = items[lastIndex];
        items.RemoveAt(lastIndex);

        return item;
    }
}