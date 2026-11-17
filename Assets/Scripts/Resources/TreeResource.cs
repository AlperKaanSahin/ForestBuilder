using UnityEngine;

public class TreeResource : MonoBehaviour
{
    private enum TreeSize
    {
        Small,
        Medium,
        Large
    }

    [Header("Scale")]
    [SerializeField] private float minScale = 0.8f;
    [SerializeField] private float maxScale = 1.3f;

    [Header("Wood Rewards")]
    [SerializeField] private Vector2Int smallWoodRange = new Vector2Int(3, 4);
    [SerializeField] private Vector2Int mediumWoodRange = new Vector2Int(5, 6);
    [SerializeField] private Vector2Int largeWoodRange = new Vector2Int(7, 9);

    private TreeSize treeSize;
    private int woodReward;

    private void Start()
    {
        GenerateTree();
    }

    private void GenerateTree()
    {
        float scale = Random.Range(minScale, maxScale);

        transform.localScale = Vector3.one * scale;
        transform.Rotate(0f, Random.Range(0f, 360f), 0f);

        if (scale < 0.95f)
        {
            treeSize = TreeSize.Small;
            woodReward = Random.Range(
                smallWoodRange.x,
                smallWoodRange.y + 1
            );
        }
        else if (scale < 1.15f)
        {
            treeSize = TreeSize.Medium;
            woodReward = Random.Range(
                mediumWoodRange.x,
                mediumWoodRange.y + 1
            );
        }
        else
        {
            treeSize = TreeSize.Large;
            woodReward = Random.Range(
                largeWoodRange.x,
                largeWoodRange.y + 1
            );
        }

        Debug.Log($"{name}: {treeSize}, Scale: {scale:F2}, Wood: {woodReward}");
    }
}