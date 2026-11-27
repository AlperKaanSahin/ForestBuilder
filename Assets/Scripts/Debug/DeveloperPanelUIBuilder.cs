
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DeveloperPanelUIBuilder : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DeveloperPanel developerPanel;
    [SerializeField] private RectTransform panelRoot;

#if UNITY_EDITOR || DEVELOPMENT_BUILD
    private TMP_Text coinsValueText;
    private TMP_Text speedLevelText;
    private TMP_Text harvestLevelText;
    private TMP_Text capacityLevelText;

    private bool isSubscribed;
#endif

    private void Start()
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        BuildUI();
#endif
    }

    private void OnDestroy()
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        UnsubscribeFromEvents();
#endif
    }

#if UNITY_EDITOR || DEVELOPMENT_BUILD

    private void BuildUI()
    {
        if (developerPanel == null || panelRoot == null)
        {
            Debug.LogError("Developer Panel references missing.", this);
            return;
        }

        if (panelRoot.childCount > 0)
        {
            Debug.LogWarning("DevPanelRoot must be empty.", this);
            return;
        }

        ConfigurePanel();

        CreateLabel(
            panelRoot, "DEVELOPER PANEL",
            32, 52, FontStyles.Bold
        );

        // Currency
        CreateSectionTitle("CURRENCY");

        coinsValueText = CreateLabel(
            panelRoot, "Coins: 0", 23, 38
        );

        RectTransform currencyRow = CreateRow("CurrencyRow");

        CreateButton(currencyRow, "+100", 160,
            developerPanel.Add100Coins);

        CreateButton(currencyRow, "+1,000", 160,
            developerPanel.Add1000Coins);

        CreateButton(currencyRow, "+10,000", 160,
            developerPanel.Add10000Coins);

        // Upgrades
        CreateSectionTitle("PLAYER UPGRADES");

        speedLevelText = CreateUpgradeRow(
            "Movement Speed",
            developerPanel.SpeedPlus,
            developerPanel.SpeedMax
        );

        harvestLevelText = CreateUpgradeRow(
            "Harvest Power",
            developerPanel.HarvestPlus,
            developerPanel.HarvestMax
        );

        capacityLevelText = CreateUpgradeRow(
            "Carry Capacity",
            developerPanel.CapacityPlus,
            developerPanel.CapacityMax
        );

        // Progression
        CreateSectionTitle("PROGRESSION");

        RectTransform progressionRow = CreateRow("ProgressionRow");

        CreateButton(progressionRow, "UNLOCK STONE", 235,
            developerPanel.UnlockStone);

        CreateButton(progressionRow, "+5 REROLLS", 235,
            developerPanel.AddRerolls);

        // Testing
        CreateSectionTitle("TESTING");

        RectTransform testingRow = CreateRow("TestingRow");

        CreateButton(testingRow, "MAX ALL UPGRADES", 300,
            developerPanel.MaxAllUpgrades);

        SubscribeToEvents();
        RefreshValues();
    }

    private void ConfigurePanel()
    {
        panelRoot.anchorMin = new Vector2(0.5f, 0.5f);
        panelRoot.anchorMax = new Vector2(0.5f, 0.5f);
        panelRoot.pivot = new Vector2(0.5f, 0.5f);
        panelRoot.anchoredPosition = Vector2.zero;
        panelRoot.sizeDelta = new Vector2(700, 850);
        panelRoot.localScale = Vector3.one;

        Image background = panelRoot.GetComponent<Image>();
        if (background == null)
            background = panelRoot.gameObject.AddComponent<Image>();

        background.color = new Color(0.12f, 0.15f, 0.19f, 0.97f);

        VerticalLayoutGroup layout =
            panelRoot.GetComponent<VerticalLayoutGroup>();

        if (layout == null)
            layout = panelRoot.gameObject
                .AddComponent<VerticalLayoutGroup>();

        layout.padding = new RectOffset(24, 24, 20, 20);
        layout.spacing = 8f;
        layout.childAlignment = TextAnchor.UpperCenter;
        layout.childControlWidth = true;
        layout.childControlHeight = true;
        layout.childForceExpandWidth = true;
        layout.childForceExpandHeight = false;
    }

    private RectTransform CreateObject(string name, Transform parent)
    {
        GameObject obj = new GameObject(
            name, typeof(RectTransform)
        );

        obj.transform.SetParent(parent, false);
        return obj.GetComponent<RectTransform>();
    }

    private void CreateSectionTitle(string content)
    {
        CreateLabel(
            panelRoot, content, 24, 38, FontStyles.Bold
        );
    }

    private TMP_Text CreateLabel(
        Transform parent,
        string content,
        float fontSize,
        float height,
        FontStyles style = FontStyles.Normal)
    {
        RectTransform rect = CreateObject(content, parent);

        TextMeshProUGUI label =
            rect.gameObject.AddComponent<TextMeshProUGUI>();

        label.text = content;
        label.fontSize = fontSize;
        label.fontStyle = style;
        label.color = Color.white;
        label.alignment = TextAlignmentOptions.MidlineLeft;
        label.enableWordWrapping = false;
        label.enableAutoSizing = true;
        label.fontSizeMin = 14;
        label.fontSizeMax = fontSize;
        label.raycastTarget = false;

        LayoutElement element =
            rect.gameObject.AddComponent<LayoutElement>();

        element.preferredHeight = height;
        element.flexibleWidth = 1f;

        return label;
    }

    private RectTransform CreateRow(string name)
    {
        RectTransform row = CreateObject(name, panelRoot);

        HorizontalLayoutGroup layout =
            row.gameObject.AddComponent<HorizontalLayoutGroup>();

        layout.spacing = 10f;
        layout.childAlignment = TextAnchor.MiddleLeft;
        layout.childControlWidth = true;
        layout.childControlHeight = true;
        layout.childForceExpandWidth = false;
        layout.childForceExpandHeight = true;

        LayoutElement element =
            row.gameObject.AddComponent<LayoutElement>();

        element.preferredHeight = 62f;

        return row;
    }

    private TMP_Text CreateUpgradeRow(
        string displayName,
        UnityAction plusAction,
        UnityAction maxAction)
    {
        RectTransform row = CreateRow(
            displayName.Replace(" ", "") + "Row"
        );

        TMP_Text nameText = CreateLabel(
            row, displayName, 23, 56
        );

        LayoutElement nameLayout =
            nameText.GetComponent<LayoutElement>();

        nameLayout.minWidth = 180f;
        nameLayout.flexibleWidth = 1f;

        TMP_Text levelText = CreateLabel(
            row, "0/0", 21, 56
        );

        LayoutElement levelLayout =
            levelText.GetComponent<LayoutElement>();

        levelLayout.preferredWidth = 75f;
        levelLayout.flexibleWidth = 0f;

        CreateButton(row, "+1", 110, plusAction);
        CreateButton(row, "MAX", 110, maxAction);

        return levelText;
    }

    private Button CreateButton(
        Transform parent,
        string content,
        float width,
        UnityAction action)
    {
        RectTransform rect = CreateObject(
            content.Replace(" ", "") + "Button",
            parent
        );

        Image image = rect.gameObject.AddComponent<Image>();
        image.color = new Color(0.24f, 0.32f, 0.42f);

        Button button = rect.gameObject.AddComponent<Button>();
        button.targetGraphic = image;

        ColorBlock colors = button.colors;
        colors.normalColor = Color.white;
        colors.highlightedColor = new Color(0.85f, 0.9f, 1f);
        colors.pressedColor = new Color(0.65f, 0.75f, 0.9f);
        colors.selectedColor = Color.white;
        button.colors = colors;

        LayoutElement element =
            rect.gameObject.AddComponent<LayoutElement>();

        element.preferredWidth = width;
        element.preferredHeight = 58f;
        element.flexibleWidth = 0f;

        RectTransform textRect = CreateObject("Text", rect);
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = new Vector2(5f, 0f);
        textRect.offsetMax = new Vector2(-5f, 0f);

        TextMeshProUGUI buttonText =
            textRect.gameObject.AddComponent<TextMeshProUGUI>();

        buttonText.text = content;
        buttonText.fontSize = 22;
        buttonText.enableAutoSizing = true;
        buttonText.fontSizeMin = 14;
        buttonText.fontSizeMax = 22;
        buttonText.enableWordWrapping = false;
        buttonText.color = Color.white;
        buttonText.alignment = TextAlignmentOptions.Center;
        buttonText.raycastTarget = false;

        if (action != null)
            button.onClick.AddListener(action);

        return button;
    }

    // Live values

    private void RefreshValues()
    {
        if (developerPanel == null)
            return;

        if (developerPanel.Currency != null &&
            coinsValueText != null)
        {
            coinsValueText.text =
                $"Coins: {developerPanel.Currency.Coins:N0}";
        }

        RefreshLevel(
            speedLevelText,
            developerPanel.MovementSpeedUpgrade
        );

        RefreshLevel(
            harvestLevelText,
            developerPanel.HarvestPowerUpgrade
        );

        RefreshLevel(
            capacityLevelText,
            developerPanel.CarryCapacityUpgrade
        );
    }

    private void RefreshLevel(TMP_Text label, RepeatableUpgrade upgrade)
    {
        if (label == null || upgrade == null)
            return;

        label.text = $"{upgrade.CurrentLevel}/{upgrade.MaxLevel}";
    }

    // Events

    private void SubscribeToEvents()
    {
        if (isSubscribed || developerPanel == null)
            return;

        if (developerPanel.Currency != null)
            developerPanel.Currency.CoinsChanged += OnCoinsChanged;

        SubscribeToUpgrade(developerPanel.MovementSpeedUpgrade);
        SubscribeToUpgrade(developerPanel.HarvestPowerUpgrade);
        SubscribeToUpgrade(developerPanel.CarryCapacityUpgrade);

        isSubscribed = true;
    }

    private void SubscribeToUpgrade(RepeatableUpgrade upgrade)
    {
        if (upgrade != null)
            upgrade.LevelChanged += OnUpgradeLevelChanged;
    }

    private void OnCoinsChanged(int coins)
    {
        RefreshValues();
    }

    private void OnUpgradeLevelChanged(int level)
    {
        RefreshValues();
    }

    private void UnsubscribeFromEvents()
    {
        if (!isSubscribed || developerPanel == null)
            return;

        if (developerPanel.Currency != null)
            developerPanel.Currency.CoinsChanged -= OnCoinsChanged;

        UnsubscribeFromUpgrade(developerPanel.MovementSpeedUpgrade);
        UnsubscribeFromUpgrade(developerPanel.HarvestPowerUpgrade);
        UnsubscribeFromUpgrade(developerPanel.CarryCapacityUpgrade);

        isSubscribed = false;
    }

    private void UnsubscribeFromUpgrade(RepeatableUpgrade upgrade)
    {
        if (upgrade != null)
            upgrade.LevelChanged -= OnUpgradeLevelChanged;
    }

#endif
}
