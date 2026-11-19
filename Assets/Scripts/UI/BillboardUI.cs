using UnityEngine;

public class BillboardUI : MonoBehaviour
{
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        if (mainCamera == null)
            return;

        transform.rotation = Quaternion.LookRotation(
            transform.position - mainCamera.transform.position
        );
    }
}