using UnityEngine;

public class RockResource : ResourceNode
{
    private RandomVisualSelector visualSelector;

    private void Awake()
    {
        visualSelector = GetComponent<RandomVisualSelector>();
    }

    public void Initialize(int health, float scale)
    {
        transform.localScale = Vector3.one * scale;
        transform.rotation = Quaternion.Euler(
            0f,
            Random.Range(0f, 360f),
            0f
        );

        visualSelector?.SelectRandomVariant();

        InitializeNode(health);

    }
}