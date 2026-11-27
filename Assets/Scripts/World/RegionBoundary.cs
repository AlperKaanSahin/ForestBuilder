
using UnityEngine;

public class RegionBoundary : MonoBehaviour
{
    [Header("Hex Settings")]
    [SerializeField] private float hexRadius = 30f;

    [Header("Fence Settings")]
    [SerializeField] private GameObject fencePrefab;
    [SerializeField] private float segmentLength = 2.2f;
    [SerializeField, Range(1f, 1.1f)]
    private float fenceOverlap = 1.03f;
    [SerializeField] private float fenceHeight = 1.4f;
    [SerializeField] private float fenceThickness = 0.2f;

    private readonly GameObject[] boundaries = new GameObject[6];
    private Region region;
    private RegionManager regionManager;
    private bool boundariesGenerated;

    private void Start()
    {
        EnsureBoundariesGenerated();
    }
    private void EnsureBoundariesGenerated()
    {
        if (boundariesGenerated)
            return;

        if (fencePrefab == null || segmentLength <= 0f)
        {
            Debug.LogError("Invalid fence configuration.", this);
            return;
        }

        GenerateBoundaries();
        boundariesGenerated = true;
    }

    public void Initialize(Region region, RegionManager regionManager)
    {
        if (region == null || regionManager == null)
        {
            Debug.LogError("RegionBoundary: Missing references.", this);
            return;
        }

        if (this.regionManager != null)
            this.regionManager.RegionRegistered -= OnRegionRegistered;

        this.region = region;
        this.regionManager = regionManager;

        EnsureBoundariesGenerated();

        this.regionManager.RegionRegistered += OnRegionRegistered;

        RefreshBoundaries();
    }
    private void OnRegionRegistered(Region registeredRegion)
    {
        RefreshBoundaries();
    }

    private void RefreshBoundaries()
    {
        if (region == null || regionManager == null)
            return;

        for (int i = 0; i < 6; i++)
        {
            HexDirection direction = (HexDirection)i;

            HexCoordinates neighborCoordinates =
                region.Coordinates.GetNeighbor(direction);

            bool neighborExists =
                regionManager.HasRegion(neighborCoordinates);

            SetBoundaryActive(i, !neighborExists);
        }
    }

    private void OnDestroy()
    {
        if (regionManager != null)
            regionManager.RegionRegistered -= OnRegionRegistered;
    }

    private void GenerateBoundaries()
    {
        if (fencePrefab == null || segmentLength <= 0f)
        {
            Debug.LogError("Invalid fence configuration.", this);
            return;
        }

        // Matches North, NE, SE, South, SW, NW.
        float[] angles = { 60f, 0f, 300f, 240f, 180f, 120f };

        for (int i = 0; i < 6; i++)
        {
            Vector3 start = GetVertex(angles[i]);
            Vector3 end = GetVertex(angles[i] + 60f);

            CreateBoundary(i, start, end);
        }
    }

    private Vector3 GetVertex(float angle)
    {
        float radians = angle * Mathf.Deg2Rad;

        return new Vector3(
            Mathf.Cos(radians) * hexRadius,
            0f,
            Mathf.Sin(radians) * hexRadius
        );
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        for (int i = 0; i < 6; i++)
        {
            float angle = i * 60f;

            Vector3 vertex = transform.TransformPoint(GetVertex(angle));

            Gizmos.DrawSphere(vertex, 0.5f);
        }
    }
    private void CreateBoundary(int index, Vector3 start, Vector3 end)
    {
        GameObject boundary = new GameObject($"Boundary_{index}");
        boundary.transform.SetParent(transform, false);

        Vector3 direction = end - start;
        float edgeLength = direction.magnitude;

        int segmentCount = Mathf.CeilToInt(edgeLength / segmentLength);
        float actualLength = edgeLength / segmentCount;
        float scaleFactor =
    (actualLength / segmentLength) * fenceOverlap;

        float rotationY = -Mathf.Atan2(
            direction.z, direction.x
        ) * Mathf.Rad2Deg;

        for (int i = 0; i < segmentCount; i++)
        {
            float t = (i + 0.5f) / segmentCount;

            GameObject segment = Instantiate(
                fencePrefab,
                boundary.transform
            );

            segment.name = $"Fence_{i}";

            segment.transform.localPosition = Vector3.Lerp(
                start, end, t
            );

            segment.transform.localRotation =
                Quaternion.Euler(0f, rotationY, 0f);

            segment.transform.localScale =
                new Vector3(scaleFactor, 1f, 1f);
        }

        // One physical barrier per hex edge.
        BoxCollider collider = boundary.AddComponent<BoxCollider>();

        collider.center = new Vector3(
            (start.x + end.x) * 0.5f,
            fenceHeight * 0.5f,
            (start.z + end.z) * 0.5f
        );

        // Collider coordinates are local to the boundary.
        // Rotate the boundary to align its local X axis.
        boundary.transform.localRotation =
            Quaternion.Euler(0f, rotationY, 0f);

        // Keep the parent unrotated for fence placement.
        // Instead, place collider on a separate child.
        Destroy(collider);

        GameObject barrier = new GameObject("PhysicalBarrier");
        barrier.transform.SetParent(boundary.transform, false);

        barrier.transform.localPosition =
            (start + end) * 0.5f +
            Vector3.up * (fenceHeight * 0.5f);

        // Reset parent rotation so coordinates remain consistent.
        boundary.transform.localRotation = Quaternion.identity;

        barrier.transform.localRotation =
            Quaternion.Euler(0f, rotationY, 0f);

        BoxCollider barrierCollider = barrier.AddComponent<BoxCollider>();
        barrierCollider.size = new Vector3(
            edgeLength,
            fenceHeight,
            fenceThickness
        );

        boundaries[index] = boundary;
    }

    public void SetBoundaryActive(int index, bool active)
    {
        if (index < 0 || index >= boundaries.Length)
            return;

        if (boundaries[index] != null)
            boundaries[index].SetActive(active);
    }
}
