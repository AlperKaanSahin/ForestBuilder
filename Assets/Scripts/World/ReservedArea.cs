using UnityEngine;

public class ReservedArea : MonoBehaviour
{
    [SerializeField] private float radius = 3f;

    public float Radius => radius;
    public Vector3 Center => transform.position;

    public bool Overlaps(Vector3 position, float otherRadius)
    {
        Vector2 thisCenter = new Vector2(Center.x, Center.z);
        Vector2 otherCenter = new Vector2(position.x, position.z);

        float minimumDistance = radius + otherRadius;

        return Vector2.Distance(thisCenter, otherCenter) < minimumDistance;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(Center, radius);
    }
}