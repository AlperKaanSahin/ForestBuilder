using UnityEngine;

public class ExpansionInteractionZone : MonoBehaviour
{
    [SerializeField] private ExpansionPoint expansionPoint;
    [SerializeField] private ExpansionUI expansionUI;

    public void Initialize(
        ExpansionPoint point,
        ExpansionUI ui)
    {
        expansionPoint = point;
        expansionUI = ui;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        Debug.Log($"TRIGGER ENTER: {gameObject.name}");

        if (expansionPoint == null || expansionUI == null)
        {
            Debug.LogWarning(
                $"Missing references: {gameObject.name} | " +
                $"Point: {expansionPoint} | UI: {expansionUI}"
            );
            return;
        }

        Debug.Log(
            $"Direction: {expansionPoint.Direction} | " +
            $"Available: {expansionPoint.IsAvailable}"
        );

        if (!expansionPoint.IsAvailable)
            return;

        expansionUI.Show(expansionPoint);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (expansionUI != null)
            expansionUI.Hide(expansionPoint);
    }
}