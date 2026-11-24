using UnityEngine;

public class HarvestPowerUpgrade : RepeatableUpgrade
{
    [Header("References")]
    [SerializeField] private ResourceHarvester resourceHarvester;

    [Header("Harvest Power")]
    [SerializeField] private int damageIncrease = 1;

    public int DamageIncrease => damageIncrease;

    public override string DisplayName => "Harvest Power";

    public override string ProgressText =>
        $"{resourceHarvester.DamagePerHit} → " +
        $"{resourceHarvester.DamagePerHit + damageIncrease} Damage";

    protected override void ApplyUpgrade()
    {
        resourceHarvester.IncreaseDamage(damageIncrease);
    }
}