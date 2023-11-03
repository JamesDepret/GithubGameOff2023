namespace Manager;
public partial class HarvestManager : Node
{
	[Export] public int HarvestSpeedUpgradeCost { get; set; } = 60;
	[Export] public int HarvestIncomeUpgradeCost { get; set; } = 50;
	[Export] public int MaxIncome { get; set; } = 32;
	public int BaseIncome { get; set; } = 1;
	public int HarvestTimeLevel { get; set; } = 0;
	private int baseHarvestTime = 6;
	public Godot.Timer HarvestTimer { get; set; }
	public override void _Ready()
	{
		HarvestTimer = GetNode<Godot.Timer>("Timer");
		HarvestTimer.WaitTime = baseHarvestTime;
		HarvestTimer.Timeout += HarvestTimerTimeout;
	}

	public double GetHarvestTimePercentage()
	{
		return 1 - HarvestTimer.TimeLeft / HarvestTimer.WaitTime;
	}

	public bool UpgradeHarvestTime()
	{
		var parts = GameEvents.Instance.Parts;
		if (HarvestTimeLevel >= 5 ) return true;
		if (parts < HarvestSpeedUpgradeCost) return false;
		HarvestTimeLevel++;
		baseHarvestTime--;
		HarvestTimer.WaitTime = baseHarvestTime;
		GameEvents.Instance.EmitPartsCollected(-HarvestSpeedUpgradeCost);
		HarvestSpeedUpgradeCost += 60;
        return HarvestTimeLevel >=5;
    }

    public bool UpgradeHarvestIncome()
	{
		var parts = GameEvents.Instance.Parts;
		if (BaseIncome >= MaxIncome ) return true;
		if (parts < HarvestIncomeUpgradeCost) return false;
		GameEvents.Instance.EmitPartsCollected(-HarvestIncomeUpgradeCost);
		BaseIncome++;
		return BaseIncome >=MaxIncome;
	}

	private void HarvestTimerTimeout()
	{
		GameEvents.Instance.EmitPartsCollected(BaseIncome);
	}
}
