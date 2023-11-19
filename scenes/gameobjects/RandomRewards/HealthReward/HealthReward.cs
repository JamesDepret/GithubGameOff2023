namespace BaseRewards;

public partial class HealthReward : BaseReward
{
    [Export] public float HealthPercentage { get; set; } = .1f;

    protected override void Collect()
    {
        player.HealPercentage(HealthPercentage);
        base.Collect();
    }
}