using TMPro;
using UnityEngine;

public class WoodCounterUI : MonoBehaviour
{
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private TMP_Text woodText;

    private void OnEnable()
    {
        playerInventory.WoodChanged += UpdateWoodText;
    }

    private void Start()
    {
        UpdateWoodText(
            playerInventory.CurrentWood,
            playerInventory.MaxWood
        );
    }

    private void OnDisable()
    {
        playerInventory.WoodChanged -= UpdateWoodText;
    }

    private void UpdateWoodText(int currentWood, int maxWood)
    {
        woodText.text = $"Wood: {currentWood} / {maxWood}";
    }
}