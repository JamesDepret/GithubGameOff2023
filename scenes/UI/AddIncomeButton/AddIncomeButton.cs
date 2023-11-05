namespace UI;
public partial class AddIncomeButton : Button
{
	[Export] private HarvestManager harvestManager;
	Label label;
	
	public override void _Ready()
	{
		label = GetNode<Label>("Label");
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
		label.Text = harvestManager.BaseIncome +"/"+ harvestManager.MaxIncome;
	}
}