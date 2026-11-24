using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeProgressUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RepeatableUpgrade upgrade;
    [SerializeField] private Image segmentPrefab;
    [SerializeField] private Transform segmentContainer;

    [Header("Colors")]
    [SerializeField] private Color filledColor = Color.white;
    [SerializeField] private Color emptyColor = Color.gray;

    private readonly List<Image> segments = new List<Image>();

    private void Start()
    {
        CreateSegments();
        Refresh();

        upgrade.LevelChanged += OnLevelChanged;
    }

    private void OnDestroy()
    {
        if (upgrade != null)
        {
            upgrade.LevelChanged -= OnLevelChanged;
        }
    }

    private void CreateSegments()
    {
        for (int i = 0; i < upgrade.MaxLevel; i++)
        {
            Image segment = Instantiate(
                segmentPrefab,
                segmentContainer
            );

            segments.Add(segment);
        }
    }

    private void OnLevelChanged(int level)
    {
        Refresh();
    }

    private void Refresh()
    {
        for (int i = 0; i < segments.Count; i++)
        {
            segments[i].color =
                i < upgrade.CurrentLevel
                    ? filledColor
                    : emptyColor;
        }
    }
}