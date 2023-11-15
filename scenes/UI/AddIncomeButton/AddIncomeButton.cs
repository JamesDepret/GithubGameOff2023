namespace UI;
public partial class AddIncomeButton : Button
{
	[Export] private HarvestManager harvestManager;
	
	public override void _Ready()
	{
		Pressed += OnHarvestIncomeUpgraded;
	}

	private void OnHarvestIncomeUpgraded()
	{
		Visible = !harvestManager.UpgradeHarvestIncome();
	}
}
