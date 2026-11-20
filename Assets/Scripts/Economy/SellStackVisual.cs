using System.Collections.Generic;
using UnityEngine;

public class SellStackVisual : MonoBehaviour
{
    [Header("Stack Layout")]
    [SerializeField] private int maxColumns = 10;
    [SerializeField] private int maxRows = 10;

    [SerializeField] private float horizontalSpacing = 0.6f;
    [SerializeField] private float depthSpacing = 0.5f;
    [SerializeField] private float verticalSpacing = 0.25f;

    private readonly List<SellStackItem> items =
        new List<SellStackItem>();

    public int Count => items.Count;

    public SellStackItem AddResource(ResourceDefinition resource)
    {
        if (resource == null || resource.StackVisualPrefab == null)
            return null;

        GameObject visual = Instantiate(
            resource.StackVisualPrefab,
            transform
        );

        visual.transform.localRotation = Quaternion.Euler(
            90f,
            0f,
            0f
        );

        SellStackItem item =
            new SellStackItem(resource, visual);

        items.Add(item);

        RefreshLayout();

        return item;
    }

    public SellStackItem RemoveLastItem()
    {
        if (items.Count == 0)
            return null;

        int lastIndex = items.Count - 1;

        SellStackItem item = items[lastIndex];
        items.RemoveAt(lastIndex);

        RefreshLayout();

        return item;
    }

    private void RefreshLayout()
    {
        int totalItems = items.Count;

        if (totalItems == 0)
            return;

        int maxItemsPerLayer = maxColumns * maxRows;

        int itemsOnBaseLayer = Mathf.Min(
            totalItems,
            maxItemsPerLayer
        );

        int columns = Mathf.Min(
            maxColumns,
            Mathf.CeilToInt(Mathf.Sqrt(itemsOnBaseLayer))
        );

        int rows = Mathf.CeilToInt(
            (float)itemsOnBaseLayer / columns
        );

        float xOffset =
            (columns - 1) * horizontalSpacing / 2f;

        float zOffset =
            (rows - 1) * depthSpacing / 2f;

        for (int i = 0; i < items.Count; i++)
        {
            int layer = i / maxItemsPerLayer;
            int indexInLayer = i % maxItemsPerLayer;

            int row = indexInLayer / columns;
            int column = indexInLayer % columns;

            items[i].Visual.transform.localPosition =
                new Vector3(
                    column * horizontalSpacing - xOffset,
                    layer * verticalSpacing,
                    row * depthSpacing - zOffset
                );
        }
    }
}