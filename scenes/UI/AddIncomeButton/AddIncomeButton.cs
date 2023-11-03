namespace UI;
public partial class AddIncomeButton : Button
{
	[Export] private HarvestManager harvestManager;
	
	public override void _Ready()
	{
		Pressed += OnHarvestIncomeUpgraded;
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("ui_harvest_income_upgrade"))
		{
			OnHarvestIncomeUpgraded();
		}
	}

	private void OnHarvestIncomeUpgraded()
	{
		Visible = !harvestManager.UpgradeHarvestIncome();
	}
}
