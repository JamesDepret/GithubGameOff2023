namespace UI;
public partial class UpgradesScreen : CanvasLayer
{
	[Signal] public delegate void UpgradeSelectedEventHandler(BaseUpgrade upgrade);
	[Export] PackedScene UpgradeCardScene;
	private HBoxContainer CardContainer;
	private Button DoneButton;
	private Button AddSupplyButton;
	private BaseUpgrade[] currentUpgrades;
	private bool canAddSupply = true;
	public override void _Ready()
	{
		CardContainer = GetNode<HBoxContainer>("MarginContainer/CardContainer");
		DoneButton = GetNode<Button>("DoneButton");
		AddSupplyButton = GetNode<Button>("TextureRect/BuyRoom");
		DoneButton.Pressed += OnDoneSelected;
		AddSupplyButton.Pressed += OnAddSupply;
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
		var currentSupply = GameEvents.Instance.Supply;
		var maxSupply = GameEvents.Instance.MaxSupply;
		var maxSupplyUpgraded = GameEvents.Instance.MaxSupplyUpgraded;
		foreach (var upgrade in currentUpgrades)
		{
			var upgradeCard = UpgradeCardScene.Instantiate() as ShipUpgradeCard;
			CardContainer.AddChild(upgradeCard);
			upgradeCard.SetAbilityUpgrade(upgrade);
			upgradeCard.Selected += () => OnUpgradeCardSelected(upgrade);
			upgradeCard.SetPrice(upgrade.Price);
			upgradeCard.SetSupply(upgrade.SupplyCost);

			upgradeCard.SetDisabledForPrice(parts < upgrade.Price);
			upgradeCard.SetDisabledForSupply(currentSupply + upgrade.SupplyCost > maxSupply);
			AddSupplyButton.Visible = maxSupply < maxSupplyUpgraded;
			canAddSupply = parts < GameEvents.Instance.SupplyUpgradePrice;
			
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

	private void OnAddSupply()
	{
		GameEvents.Instance.BuySupply();
		SetupCards();
	}

}
