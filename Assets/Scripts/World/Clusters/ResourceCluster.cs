using UnityEngine;

public class ResourceCluster : MonoBehaviour
{
    [Header("Cluster Area")]
    [SerializeField] private float radius = 5f;

    public float Radius => radius;
    public Vector3 Center => transform.position;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Center, radius);
    }

    public Vector3 GetRandomPoint()
    {
        Vector2 randomPoint = Random.insideUnitCircle * radius;

        return Center + new Vector3(
            randomPoint.x,
            0f,
            randomPoint.y
        );
    }
}