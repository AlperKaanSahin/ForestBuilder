using UnityEngine;
using UnityEngine.UI;

public class ExpansionUI : MonoBehaviour
{
    [SerializeField] private GameObject expandButton;
    [SerializeField] private Button button;
    [SerializeField] private RegionSelectionUI regionSelectionUI;

    private ExpansionPoint currentPoint;

    private void Awake()
    {
        button.onClick.AddListener(OnExpandClicked);
        expandButton.SetActive(false);
    }

    public void Show(ExpansionPoint point)
    {
        if (point == null || !point.IsAvailable)
            return;

        currentPoint = point;
        expandButton.SetActive(true);
    }

    public void Hide(ExpansionPoint point)
    {
        regionSelectionUI.CloseIfPoint(point);

        if (currentPoint != point)
            return;

        currentPoint = null;
        expandButton.SetActive(false);
    }

private void OnExpandClicked()
    {
        if (currentPoint == null || !currentPoint.IsAvailable)
            return;

        expandButton.SetActive(false);
        regionSelectionUI.Open(currentPoint);
    }
}