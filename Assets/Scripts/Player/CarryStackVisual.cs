using System.Collections.Generic;
using UnityEngine;

public class CarryStackVisual : MonoBehaviour
{
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private GameObject woodVisualPrefab;
    [SerializeField] private float verticalSpacing = 0.25f;

    private readonly List<GameObject> woodVisuals = new List<GameObject>();

    private void OnEnable()
    {
        playerInventory.WoodChanged += UpdateStack;
    }

    private void Start()
    {
        UpdateStack(
            playerInventory.CurrentWood,
            playerInventory.MaxWood
        );
    }

    private void OnDisable()
    {
        playerInventory.WoodChanged -= UpdateStack;
    }

    private void UpdateStack(int currentWood, int maxWood)
    {
        while (woodVisuals.Count < currentWood)
        {
            AddWoodVisual();
        }

        while (woodVisuals.Count > currentWood)
        {
            RemoveWoodVisual();
        }
    }

    private void AddWoodVisual()
    {
        int index = woodVisuals.Count;

        GameObject woodVisual = Instantiate(
            woodVisualPrefab,
            transform
        );

        woodVisual.transform.localPosition = new Vector3(
            0f,
            index * verticalSpacing,
            0f
        );

        woodVisual.transform.localRotation = Quaternion.Euler(
            0f,
            0f,
            90f
        );

        woodVisuals.Add(woodVisual);
    }

    private void RemoveWoodVisual()
    {
        int lastIndex = woodVisuals.Count - 1;

        GameObject woodVisual = woodVisuals[lastIndex];

        woodVisuals.RemoveAt(lastIndex);
        Destroy(woodVisual);
    }
    public GameObject TakeLastVisual()
    {
        if (woodVisuals.Count == 0)
            return null;

        int lastIndex = woodVisuals.Count - 1;

        GameObject woodVisual = woodVisuals[lastIndex];
        woodVisuals.RemoveAt(lastIndex);

        return woodVisual;
    }
}