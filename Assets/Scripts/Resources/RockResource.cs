using UnityEngine;

public class RockResource : MonoBehaviour
{
    private RandomVisualSelector visualSelector;

    private void Awake()
    {
        visualSelector = GetComponent<RandomVisualSelector>();
    }

    public void Initialize()
    {
        if (visualSelector != null)
            visualSelector.SelectRandomVariant();
    }
}