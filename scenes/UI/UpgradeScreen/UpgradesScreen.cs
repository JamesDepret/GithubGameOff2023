namespace UI;
public partial class UpgradesScreen : CanvasLayer
{
	[Signal] public delegate void UpgradeSelectedEventHandler(BaseUpgrade upgrade);
	[Export] PackedScene UpgradeCardScene;
	HBoxContainer CardContainer;
	public override void _Ready()
	{
		CardContainer = GetNode<HBoxContainer>("MarginContainer/CardContainer");
		GetTree().Paused = true;
	}

	public void SetAbilityUpgrades(BaseUpgrade[] upgrades)
	{
		var parts = GameEvents.Instance.Parts;
		foreach (var upgrade in upgrades)
		{
			var upgradeCard = UpgradeCardScene.Instantiate() as ShipUpgradeCard;
			CardContainer.AddChild(upgradeCard);
			upgradeCard.SetAbilityUpgrade(upgrade);
			upgradeCard.Selected += () => OnUpgradeCardSelected(upgrade);
			upgradeCard.SetPrice(upgrade.Price);
			if (parts < upgrade.Price) upgradeCard.SetDisabled(true);
		}
	}

	public void OnUpgradeCardSelected(BaseUpgrade upgrade)
	{
		GameEvents.Instance.EmitPartsCollected(-upgrade.Price);
		EmitSignal(SignalName.UpgradeSelected, upgrade);
		GetTree().Paused = false;
		QueueFree();
	}

}
