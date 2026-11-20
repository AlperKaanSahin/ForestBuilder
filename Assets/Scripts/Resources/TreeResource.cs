using UnityEngine;

public class TreeResource : ResourceNode
{

    private TreeVisual treeVisual;

    private void Awake()
    {
        treeVisual = GetComponent<TreeVisual>();
    }

    public void Initialize(int health, float scale)
    {


        transform.localScale = Vector3.one * scale;
        transform.rotation = Quaternion.Euler(
            0f,
            Random.Range(0f, 360f),
            0f
        );

        treeVisual?.SelectRandomVariant();

        InitializeNode(health);


    }
}