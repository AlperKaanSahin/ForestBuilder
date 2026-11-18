using System.Collections.Generic;
using UnityEngine;

public class SellStackVisual : MonoBehaviour
{
    [SerializeField] private GameObject woodVisualPrefab;
    [SerializeField] private float horizontalSpacing = 0.6f;
    [SerializeField] private float verticalSpacing = 0.25f;
    [SerializeField] private int woodPerRow = 2;

    private readonly List<GameObject> woodVisuals = new List<GameObject>();

    public int Count => woodVisuals.Count;

    public GameObject AddWood()
    {
        int index = woodVisuals.Count;

        int column = index % woodPerRow;
        int row = index / woodPerRow;

        float xOffset = (woodPerRow - 1) * horizontalSpacing / 2f;

        GameObject woodVisual = Instantiate(
            woodVisualPrefab,
            transform
        );

        woodVisual.transform.localPosition = new Vector3(
            column * horizontalSpacing - xOffset,
            row * verticalSpacing,
            0f
        );

        woodVisual.transform.localRotation = Quaternion.Euler(
            90f,
            0f,
            0f
        );

        woodVisuals.Add(woodVisual);

        return woodVisual;
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