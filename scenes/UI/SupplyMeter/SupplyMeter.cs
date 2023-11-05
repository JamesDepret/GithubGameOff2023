namespace UI;
public partial class SupplyMeter : HBoxContainer
{
	Label SupplyLabel;
	public override void _Ready()
	{
		SupplyLabel = GetNode<Label>("SupplyLabel");
		GameEvents.Instance.SupplyChanged += OnSupplyChanged;
	}

	private void OnSupplyChanged()
	{
		SupplyLabel.Text = GameEvents.Instance.Supply + "/" + GameEvents.Instance.MaxSupply;
	}


}
