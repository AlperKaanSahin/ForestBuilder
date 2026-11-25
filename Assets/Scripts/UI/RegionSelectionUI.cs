using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegionSelectionUI : MonoBehaviour
{
    [Serializable]

    private class CandidateCard
    {
        public TMP_Text biomeText;
        public TMP_Text featureText;
        public TMP_Text tierText;
        public Button selectButton;
    }

    [Header("References")]
    [SerializeField] private RegionCandidateGenerator candidateGenerator;
    [SerializeField] private GameObject selectionPanel;
    [SerializeField] private Button closeButton;
    [SerializeField] private RegionFactory regionFactory;

    [Header("Candidate Cards")]
    [SerializeField] private CandidateCard[] cards = new CandidateCard[3];

    private List<RegionData> currentCandidates;
    private ExpansionPoint currentPoint;

    private void Awake()
    {
        selectionPanel.SetActive(false);

        closeButton.onClick.AddListener(Close);

        for (int i = 0; i < cards.Length; i++)
        {
            int index = i;
            cards[i].selectButton.onClick.AddListener(
                () => SelectCandidate(index)
            );
        }
    }

    public void Open(ExpansionPoint point)
    {
        if (point == null || !point.IsAvailable)
            return;

        currentPoint = point;

        int seed = UnityEngine.Random.Range(0, int.MaxValue);

        currentCandidates = candidateGenerator.GenerateCandidates(
            1,
            seed
        );

        if (currentCandidates.Count == 0)
        {
            Debug.LogWarning("No region candidates generated.");
            return;
        }

        for (int i = 0; i < cards.Length; i++)
        {
            bool hasCandidate = i < currentCandidates.Count;

            cards[i].selectButton.interactable = hasCandidate;

            if (!hasCandidate)
            {
                cards[i].biomeText.text = "Unavailable";
                cards[i].featureText.text = "";
                cards[i].tierText.text = "";
                continue;
            }

            RegionData candidate = currentCandidates[i];

            cards[i].biomeText.text = candidate.Biome.DisplayName;

            cards[i].featureText.text =
                candidate.Features.Count > 0
                    ? candidate.Features[0].DisplayName
                    : "No special feature";

            cards[i].tierText.text = $"Tier {candidate.Tier}";
        }

        selectionPanel.SetActive(true);
    }

    private void SelectCandidate(int index)
    {
        if (currentCandidates == null ||
            index < 0 ||
            index >= currentCandidates.Count)
            return;

        if (currentPoint == null || !currentPoint.IsAvailable)
            return;

        RegionData selected = currentCandidates[index];

        HexCoordinates target =
            currentPoint.GetTargetCoordinates();

        Region newRegion = regionFactory.CreateRegion(
            target,
            selected
        );

        if (newRegion == null)
        {
            Debug.LogWarning("Region creation failed.");
            return;
        }

        Debug.Log(
            $"Region created: {selected.Biome.DisplayName} | " +
            $"Coordinates: ({target.Q}, {target.R})"
        );

        Close();
    }

    public void CloseIfPoint(ExpansionPoint point)
    {
        if (currentPoint != point)
            return;

        Close();
    }

    public void Close()
    {
        selectionPanel.SetActive(false);
        currentPoint = null;
        currentCandidates = null;
    }
}