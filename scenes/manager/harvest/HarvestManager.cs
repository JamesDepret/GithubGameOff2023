namespace Manager;
public partial class HarvestManager : Node
{
	[Export] private ArenaManager arenaManager;
	[Export] public int HarvestSpeedUpgradeCost { get; set; } = 60;
	[Export] public int HarvestIncomeUpgradeCost { get; set; } = 50;
	[Export] public int MaxIncome { get; set; } = 32;
	[Export] public int WaveClearMultiplierInSeconds { get; set; } = 30;
	public int BaseIncome { get; set; } = 1;
	public int HarvestTimeLevel { get; set; } = 0;
	private int harvestTime = 6;
	public Godot.Timer HarvestTimer { get; set; }
	public override void _Ready()
	{
		HarvestTimer = GetNode<Godot.Timer>("Timer");
		HarvestTimer.WaitTime = harvestTime;
		HarvestTimer.Timeout += HarvestTimerTimeout;
		arenaManager.WaveCleared += OnWaveCleared;
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
		harvestTime--;
		HarvestTimer.WaitTime = harvestTime;
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

	private void OnWaveCleared()
	{
		var ticks = WaveClearMultiplierInSeconds / harvestTime;
		GameEvents.Instance.EmitPartsCollected(BaseIncome * ticks);
	}
}
