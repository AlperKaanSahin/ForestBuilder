using UnityEngine;

public class Region : MonoBehaviour
{
    [Header("Region Shape")]
    [SerializeField] private float radius = 30f;

    [Header("Debug")]
    [SerializeField] private float debugEdgeMargin = 5f;

    [Header("Grid")]
    [SerializeField] private HexCoordinates coordinates;

    [Header("Generated Data")]
    [SerializeField] private RegionData data;


    public RegionData Data => data;

    public HexCoordinates Coordinates => coordinates;

    public float Radius => radius;
    public Vector3 Center => transform.position;

    public bool ContainsPoint(Vector3 worldPosition)
    {
        return ContainsPoint(worldPosition, radius);
    }

    private bool ContainsPoint(Vector3 worldPosition, float checkRadius)
    {
        Vector3 localPosition = worldPosition - Center;

        float x = Mathf.Abs(localPosition.x);
        float z = Mathf.Abs(localPosition.z);

        return x <= checkRadius &&
               z <= Mathf.Sqrt(3f) * checkRadius / 2f &&
               Mathf.Sqrt(3f) * x + z <= Mathf.Sqrt(3f) * checkRadius;
    }

    private void DrawHex(float hexRadius, Color color)
    {
        if (hexRadius <= 0f)
            return;

        Gizmos.color = color;

        Vector3[] corners = new Vector3[6];

        for (int i = 0; i < 6; i++)
        {
            float angle = Mathf.Deg2Rad * (60f * i);

            corners[i] = Center + new Vector3(
                hexRadius * Mathf.Cos(angle),
                0.1f,
                hexRadius * Mathf.Sin(angle)
            );
        }

        for (int i = 0; i < 6; i++)
        {
            Gizmos.DrawLine(
                corners[i],
                corners[(i + 1) % 6]
            );
        }
    }

    public Vector3 GetRandomPoint()
    {
        for (int attempt = 0; attempt < 100; attempt++)
        {
            float x = Random.Range(-radius, radius);
            float z = Random.Range(-radius, radius);

            Vector3 candidate = Center + new Vector3(x, 0f, z);

            if (ContainsPoint(candidate))
                return candidate;
        }

        return Center;
    }

    public bool TryGetRandomPoint(float edgeMargin, out Vector3 point)
    {
        float usableRadius = radius - edgeMargin;

        if (usableRadius <= 0f)
        {
            point = Center;
            return false;
        }

        for (int attempt = 0; attempt < 100; attempt++)
        {
            float x = Random.Range(-usableRadius, usableRadius);
            float z = Random.Range(-usableRadius, usableRadius);

            Vector3 candidate = Center + new Vector3(x, 0f, z);

            if (ContainsPoint(candidate, usableRadius))
            {
                point = candidate;
                return true;
            }
        }

        point = Center;
        return false;
    }

    public bool CanFitCircle(Vector3 worldPosition, float circleRadius)
    {
        float usableRadius =
    radius - circleRadius / Mathf.Cos(30f * Mathf.Deg2Rad);

        if (usableRadius <= 0f)
            return false;

        return ContainsPoint(worldPosition, usableRadius);
    }
    private void OnDrawGizmosSelected()
    {
        DrawHex(radius, Color.yellow);
        DrawHex(radius - debugEdgeMargin, Color.cyan);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(Center, 0.4f);
    }
    public void Initialize(
    HexCoordinates newCoordinates,
    RegionData newData)
    {
        coordinates = newCoordinates;
        data = newData;

        UpdateWorldPosition();
    }
    public void UpdateWorldPosition()
    {
        transform.position = HexGridUtility.CoordinatesToWorldPosition(
            coordinates,
            radius
        );
    }
    private void OnValidate()
    {
        UpdateWorldPosition();
    }
}