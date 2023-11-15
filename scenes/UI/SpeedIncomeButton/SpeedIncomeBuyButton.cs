namespace UI;
public partial class SpeedIncomeBuyButton : Button
{
	[Export] private HarvestManager harvestManager;
	
	public override void _Ready()
	{
		harvestManager = GetNode<HarvestManager>("/root/Main/Managers/HarvestManager");
		Pressed += OnHarvestSpeedIncomeUpgraded;
	}

	public override void _ExitTree()
    {
		Pressed -= OnHarvestSpeedIncomeUpgraded;
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
	}
}
