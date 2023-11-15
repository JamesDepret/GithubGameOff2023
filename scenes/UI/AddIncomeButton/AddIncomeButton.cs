namespace UI;
public partial class AddIncomeButton : Button
{
	private HarvestManager harvestManager;
	
	public override void _Ready()
	{
		harvestManager = GetNode<HarvestManager>("/root/Main/Managers/HarvestManager");
		Pressed += OnHarvestIncomeUpgraded;
	}

	public override void _ExitTree()
	{
		Pressed -= OnHarvestIncomeUpgraded;
	}

	private void OnHarvestIncomeUpgraded()
	{
		Visible = !harvestManager.UpgradeHarvestIncome();
	}
}
