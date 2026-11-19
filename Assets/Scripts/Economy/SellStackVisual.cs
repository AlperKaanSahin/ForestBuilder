using System.Collections.Generic;
using UnityEngine;

public class SellStackVisual : MonoBehaviour
{
    [SerializeField] private GameObject woodVisualPrefab;
    [Header("Stack Layout")]
    [SerializeField] private int maxColumns = 10;
    [SerializeField] private int maxRows = 10;

    [SerializeField] private float horizontalSpacing = 0.6f;
    [SerializeField] private float depthSpacing = 0.5f;
    [SerializeField] private float verticalSpacing = 0.25f;

    private readonly List<GameObject> woodVisuals = new List<GameObject>();

    public int Count => woodVisuals.Count;

    public GameObject AddWood()
    {
        GameObject woodVisual = Instantiate(
            woodVisualPrefab,
            transform
        );

        woodVisual.transform.localRotation = Quaternion.Euler(
            90f,
            0f,
            0f
        );

        woodVisuals.Add(woodVisual);

        RefreshLayout();

        return woodVisual;
    }

    private void RefreshLayout()
    {
        int totalWood = woodVisuals.Count;

        if (totalWood == 0)
            return;

        int maxWoodPerLayer = maxColumns * maxRows;

        int woodOnBaseLayer = Mathf.Min(
            totalWood,
            maxWoodPerLayer
        );

        int columns = Mathf.Min(
            maxColumns,
            Mathf.CeilToInt(Mathf.Sqrt(woodOnBaseLayer))
        );

        int rows = Mathf.CeilToInt(
            (float)woodOnBaseLayer / columns
        );

        float xOffset = (columns - 1) * horizontalSpacing / 2f;
        float zOffset = (rows - 1) * depthSpacing / 2f;

        for (int i = 0; i < woodVisuals.Count; i++)
        {
            int layer = i / maxWoodPerLayer;
            int indexInLayer = i % maxWoodPerLayer;

            int row = indexInLayer / columns;
            int column = indexInLayer % columns;

            woodVisuals[i].transform.localPosition = new Vector3(
                column * horizontalSpacing - xOffset,
                layer * verticalSpacing,
                row * depthSpacing - zOffset
            );
        }
    }
    public GameObject RemoveWood()
    {
        if (woodVisuals.Count == 0)
            return null;

        int lastIndex = woodVisuals.Count - 1;

        GameObject woodVisual = woodVisuals[lastIndex];

        woodVisuals.RemoveAt(lastIndex);

        return woodVisual;
    }
}