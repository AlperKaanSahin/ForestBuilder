using UnityEngine;

public class ResourceValueCalculator : MonoBehaviour
{
    public int GetSellValue(ResourceDefinition resource)
    {
        if (resource == null)
            return 0;

        return resource.BaseSellValue;
    }
}