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
    [SerializeField] private GameObject selectionPanel;
    [SerializeField] private Button closeButton;
    [SerializeField] private ExpansionSystem expansionSystem;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private RerollSystem rerollSystem;
    [SerializeField] private Button rerollButton;
    [SerializeField] private TMP_Text rerollButtonText;


    [Header("Candidate Cards")]
    [SerializeField] private CandidateCard[] cards = new CandidateCard[3];

    private IReadOnlyList<RegionData> currentCandidates;
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
        rerollButton.onClick.AddListener(OnRerollClicked);
        rerollSystem.RerollsChanged += OnRerollsChanged;
    }

    public void Open(ExpansionPoint point)
    {
        if (point == null || !point.IsAvailable)
            return;

        HexCoordinates target = point.GetTargetCoordinates();

        RegionOffer offer = expansionSystem.GetOrCreateOffer(target);

        if (offer == null || offer.Candidates.Count == 0)
        {
            Debug.LogWarning("No region offer available.");
            return;
        }

        currentPoint = point;
        currentCandidates = offer.Candidates;
        RefreshCandidateCards();
        RefreshRerollButton();



        costText.text =
            $"Expansion Cost: {expansionSystem.CurrentCost} Coins";

        selectionPanel.SetActive(true);
    }

    private void RefreshCandidateCards()
    {
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
    }
    private void SelectCandidate(int index)
    {
        if (currentCandidates == null ||
            index < 0 ||
            index >= currentCandidates.Count)
        {
            return;
        }

        if (currentPoint == null || !currentPoint.IsAvailable)
            return;

        RegionData selected = currentCandidates[index];

        HexCoordinates target =
            currentPoint.GetTargetCoordinates();

        bool success = expansionSystem.TryExpand(
            target,
            selected
        );

        if (!success)
        {
            Debug.LogWarning("Expansion purchase failed.");
            return;
        }

        Debug.Log(
            $"Region created: {selected.Biome.DisplayName} | " +
            $"Coordinates: ({target.Q}, {target.R})"
        );

        Close();
    }
    private void OnRerollClicked()
    {
        if (currentPoint == null || !currentPoint.IsAvailable)
            return;

        HexCoordinates target = currentPoint.GetTargetCoordinates();

        if (!expansionSystem.TryReroll(target))
            return;

        RegionOffer offer = expansionSystem.GetOrCreateOffer(target);

        currentCandidates = offer.Candidates;

        RefreshCandidateCards();
    }

    private void OnRerollsChanged(int amount)
    {
        RefreshRerollButton();
    }

    private void RefreshRerollButton()
    {
        int remaining = rerollSystem.AvailableRerolls;

        rerollButtonText.text = $"REROLL ({remaining})";
        rerollButton.interactable = remaining > 0;
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

    private void OnDestroy()
    {
        if (closeButton != null)
            closeButton.onClick.RemoveListener(Close);

        if (rerollButton != null)
            rerollButton.onClick.RemoveListener(OnRerollClicked);

        if (rerollSystem != null)
            rerollSystem.RerollsChanged -= OnRerollsChanged;
    }
}