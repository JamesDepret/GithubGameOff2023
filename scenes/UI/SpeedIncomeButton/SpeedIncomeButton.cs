namespace UI;
public partial class SpeedIncomeButton : Button
{
	[Export] private HarvestManager harvestManager;
	
	public override void _Ready()
	{
		Pressed += OnHarvestSpeedIncomeUpgraded;
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("ui_harvest_speed_upgrade"))
		{
			OnHarvestSpeedIncomeUpgraded();
		}
	}

	private void OnHarvestSpeedIncomeUpgraded()
	{
		Visible = !harvestManager.UpgradeHarvestTime();
		Text = $"2 - {harvestManager.HarvestSpeedUpgradeCost}";
	}
}
