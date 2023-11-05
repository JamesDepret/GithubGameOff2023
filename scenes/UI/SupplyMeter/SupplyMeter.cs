namespace UI;
public partial class SupplyMeter : HBoxContainer
{
	Label SupplyLabel;
	public override void _Ready()
	{
		SupplyLabel = GetNode<Label>("SupplyLabel");
		GameEvents.Instance.ShipUpgradeAdded += OnShipUpgradeAdded;
	}

	private void OnShipUpgradeAdded(BaseUpgrade upgrade, Godot.Collections.Array<BaseUpgrade> currentUpgrades)
	{
		SupplyLabel.Text = GameEvents.Instance.Supply + "/" + GameEvents.Instance.MaxSupply;
	}


}
