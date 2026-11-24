using UnityEngine;

public class MovementSpeedUpgrade : RepeatableUpgrade
{
    [Header("References")]
    [SerializeField] private PlayerMovement playerMovement;

    [Header("Movement Speed")]
    [SerializeField] private float speedIncrease = 0.5f;

    public float SpeedIncrease => speedIncrease;

    public override string DisplayName => "Movement Speed";

    public override string ProgressText =>
        $"{playerMovement.MoveSpeed:0.0} → " +
        $"{playerMovement.MoveSpeed + speedIncrease:0.0} Speed";

    protected override void ApplyUpgrade()
    {
        playerMovement.IncreaseMoveSpeed(speedIncrease);
    }
}