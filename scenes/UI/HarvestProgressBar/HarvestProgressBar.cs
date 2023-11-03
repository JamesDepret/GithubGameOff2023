namespace UI;
public partial class HarvestProgressBar : ProgressBar
{
	[Export] private HarvestManager harvestManager;
	
	public override void _Process(double delta)
	{
		Value = harvestManager.GetHarvestTimePercentage();
	}


}
