using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class UpgradeZone : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private GameObject upgradePanel;

    private BoxCollider zoneCollider;
    private bool playerInside;

    public bool PlayerInside => playerInside;

    private void Awake()
    {
        zoneCollider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if (player == null)
            return;

        Bounds bounds = zoneCollider.bounds;

        bool isInside =
            player.position.x >= bounds.min.x &&
            player.position.x <= bounds.max.x &&
            player.position.z >= bounds.min.z &&
            player.position.z <= bounds.max.z;

        if (isInside && !playerInside)
        {
            playerInside = true;
            HandlePlayerEntered();
        }
        else if (!isInside && playerInside)
        {
            playerInside = false;
            HandlePlayerExited();
        }
    }

    private void HandlePlayerEntered()
    {
        Debug.Log(
            $"UPGRADE ZONE OBJECT: {gameObject.name} | " +
            $"PANEL: {upgradePanel}"
        );

        if (upgradePanel == null)
            return;

        upgradePanel.SetActive(true);
    }

    private void HandlePlayerExited()
    {
        upgradePanel.SetActive(false);

        Debug.Log("=== UPGRADE ZONE: PLAYER EXITED ===");
    }
}