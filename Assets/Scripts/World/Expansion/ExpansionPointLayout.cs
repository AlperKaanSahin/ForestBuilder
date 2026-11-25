using UnityEngine;

[ExecuteAlways]
public class ExpansionPointLayout : MonoBehaviour
{
    [SerializeField] private Region region;
    [SerializeField] private ExpansionPoint[] expansionPoints;
    [SerializeField] private HexDirection direction;

    public HexDirection Direction => direction;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }

    private void OnValidate()
    {
        UpdatePositions();
    }

    private void UpdatePositions()
    {
        if (region == null || expansionPoints == null)
            return;

        foreach (ExpansionPoint point in expansionPoints)
        {
            if (point == null)
                continue;

            point.transform.position =
                GetEdgeCenter(point.Direction);
        }
    }

    private Vector3 GetEdgeCenter(HexDirection direction)
    {
        float radius = region.Radius;
        float apothem = radius * Mathf.Sqrt(3f) / 2f;

        float angle = direction switch
        {
            HexDirection.North => 90f,
            HexDirection.NorthEast => 30f,
            HexDirection.SouthEast => -30f,
            HexDirection.South => -90f,
            HexDirection.SouthWest => -150f,
            HexDirection.NorthWest => 150f,
            _ => 0f
        };

        float radians = angle * Mathf.Deg2Rad;

        Vector3 offset = new Vector3(
            Mathf.Cos(radians) * apothem,
            0f,
            Mathf.Sin(radians) * apothem
        );

        return region.Center + offset;
    }
}