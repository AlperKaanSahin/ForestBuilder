using UnityEngine;

public static class HexGridUtility
{
    public static Vector3 CoordinatesToWorldPosition(
        HexCoordinates coordinates,
        float radius)
    {
        float x = radius * 1.5f * coordinates.Q;

        float z = -Mathf.Sqrt(3f) * radius *
                  (coordinates.R + coordinates.Q * 0.5f);

        return new Vector3(x, 0f, z);
    }
}