namespace BaseRewards;

public partial class HealthReward : BaseReward
{
    public int HealthPercentage { get; set; } = 10;

    protected override void Collect()
    {
        player.HealPercentage(HealthPercentage);
        base.Collect();
    }
}