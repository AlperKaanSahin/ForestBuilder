
using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class DeveloperPanel : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject panelRoot;

    [Header("Currency")]
    [SerializeField] private PlayerCurrency playerCurrency;

    [Header("Upgrades")]
    [SerializeField] private RepeatableUpgrade movementSpeedUpgrade;
    [SerializeField] private RepeatableUpgrade harvestPowerUpgrade;
    [SerializeField] private RepeatableUpgrade carryCapacityUpgrade;

    [Header("Progression")]
    [SerializeField] private RerollSystem rerollSystem;

    [SerializeField] private ResourceUnlockSystem resourceUnlockSystem;
    [SerializeField] private ResourceDefinition stoneResource;

    public PlayerCurrency Currency => playerCurrency;

    public RepeatableUpgrade MovementSpeedUpgrade => movementSpeedUpgrade;
    public RepeatableUpgrade HarvestPowerUpgrade => harvestPowerUpgrade;
    public RepeatableUpgrade CarryCapacityUpgrade => carryCapacityUpgrade;

    private void Awake()
    {
        if (panelRoot != null)
            panelRoot.SetActive(false);

#if !UNITY_EDITOR && !DEVELOPMENT_BUILD
        gameObject.SetActive(false);
#endif
    }

    private void Update()
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD

        bool togglePressed = false;

#if ENABLE_INPUT_SYSTEM
        togglePressed =
            Keyboard.current != null &&
            Keyboard.current.f1Key.wasPressedThisFrame;
#elif ENABLE_LEGACY_INPUT_MANAGER
        togglePressed = Input.GetKeyDown(KeyCode.F1);
#endif

        if (togglePressed && panelRoot != null)
        {
            panelRoot.SetActive(!panelRoot.activeSelf);
        }

#endif
    }

#if UNITY_EDITOR || DEVELOPMENT_BUILD

    // Currency

    public void Add100Coins()
    {
        playerCurrency?.AddCoins(100);
    }

    public void Add1000Coins()
    {
        playerCurrency?.AddCoins(1000);
    }

    public void Add10000Coins()
    {
        playerCurrency?.AddCoins(10000);
    }

    // Upgrade +1

    public void SpeedPlus()
    {
        movementSpeedUpgrade?.DebugLevelUp();
    }

    public void HarvestPlus()
    {
        harvestPowerUpgrade?.DebugLevelUp();
    }

    public void CapacityPlus()
    {
        carryCapacityUpgrade?.DebugLevelUp();
    }

    // Upgrade MAX

    private void MaxUpgrade(RepeatableUpgrade upgrade)
    {
        if (upgrade == null)
            return;

        while (!upgrade.IsMaxLevel)
        {
            if (!upgrade.DebugLevelUp())
                break;
        }
    }

    public void SpeedMax()
    {
        MaxUpgrade(movementSpeedUpgrade);
    }

    public void HarvestMax()
    {
        MaxUpgrade(harvestPowerUpgrade);
    }

    public void CapacityMax()
    {
        MaxUpgrade(carryCapacityUpgrade);
    }

    public void MaxAllUpgrades()
    {
        SpeedMax();
        HarvestMax();
        CapacityMax();
    }

    // Reroll

    public void AddRerolls()
    {
        rerollSystem?.GrantRerolls(5);
    }

    //Unlock System
    public void UnlockStone()
    {
        if (resourceUnlockSystem == null || stoneResource == null)
        {
            Debug.LogWarning("Stone unlock references are missing.");
            return;
        }

        if (resourceUnlockSystem.IsUnlocked(stoneResource))
        {
            Debug.Log("Stone is already unlocked.");
            return;
        }

        if (resourceUnlockSystem.Unlock(stoneResource))
        {
            Debug.Log("Stone unlocked through Developer Panel.");
        }
    }

#endif
}
