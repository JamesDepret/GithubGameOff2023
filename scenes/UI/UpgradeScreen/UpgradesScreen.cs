namespace UI;
public partial class UpgradesScreen : CanvasLayer
{
	[Signal] public delegate void UpgradeSelectedEventHandler(BaseUpgrade upgrade);
	[Export] PackedScene UpgradeCardScene;
	private HBoxContainer CardContainer;
	private Button DoneButton;
	private BaseUpgrade[] currentUpgrades;
	public override void _Ready()
	{
		CardContainer = GetNode<HBoxContainer>("MarginContainer/CardContainer");
		DoneButton = GetNode<Button>("DoneButton");
		DoneButton.Pressed += OnDoneSelected;
		GetTree().Paused = true;
	}

	public void SetAbilityUpgrades(BaseUpgrade[] upgrades)
	{
		currentUpgrades = upgrades;
		SetupCards();
	}

	private void SetupCards()
	{
		CardContainer.GetChildren().ToList().ForEach(child => child.QueueFree());
		var parts = GameEvents.Instance.Parts;
		foreach (var upgrade in currentUpgrades)
		{
			var upgradeCard = UpgradeCardScene.Instantiate() as ShipUpgradeCard;
			CardContainer.AddChild(upgradeCard);
			upgradeCard.SetAbilityUpgrade(upgrade);
			upgradeCard.Selected += () => OnUpgradeCardSelected(upgrade);
			upgradeCard.SetPrice(upgrade.Price);
			if (parts < upgrade.Price) upgradeCard.SetDisabled(true);
		}
	}

	private void OnUpgradeCardSelected(BaseUpgrade upgrade)
	{
		GameEvents.Instance.EmitPartsCollected(-upgrade.Price);
		EmitSignal(SignalName.UpgradeSelected, upgrade);
		SetupCards();
	}

	private void OnDoneSelected()
	{
		GetTree().Paused = false;
		QueueFree();
	}

}
