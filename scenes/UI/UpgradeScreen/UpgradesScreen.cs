namespace UI;
public partial class UpgradesScreen : CanvasLayer
{
	[Signal] public delegate void UpgradeSelectedEventHandler(BaseUpgrade upgrade);
	public BaseUpgrade[] CurrentTurrets { get; set; }
	[Export] PackedScene UpgradeCardScene;
	private ShipUpgradeCard selectedCard;
	private BaseUpgrade selectedUpgrade;
	private HBoxContainer CardContainer;
	private Button DoneButton;
	private Button AddSupplyButton;
	private Button BuyButton;
	private BaseUpgrade[] availableUpgrades;
	private HFlowContainer turretContainers;
	private bool canAddSupply = true;
	public override void _Ready()
	{
		selectedCard = GetNode<ShipUpgradeCard>("ShipUpgradeCard");
		CardContainer = GetNode<HBoxContainer>("MarginContainer/CardContainer");
		DoneButton = GetNode<Button>("DoneButton");
		BuyButton = GetNode<Button>("BuyButton");
		AddSupplyButton = GetNode<Button>("TextureRect/BuyRoom");
		turretContainers = GetNode<HFlowContainer>("TurretContainers");
		turretContainers.GetChildren().ToList().ForEach(child => child.QueueFree());

		BuyButton.Pressed += OnUpgradeCardSelected;
		DoneButton.Pressed += OnDoneSelected;
		AddSupplyButton.Pressed += OnAddSupply;
		GetTree().Paused = true;
	}

	public void SetAbilityUpgrades(BaseUpgrade[] upgrades)
	{
		availableUpgrades = upgrades;
		selectedUpgrade = null;
		selectedCard.Visible = false;
		BuyButton.Visible = false;
		SetupCards();
	}

	public void SetSelectedUpgrade(BaseUpgrade upgrade)
	{
		selectedUpgrade = upgrade;
		selectedCard.Visible = true;
		SetupSelectedCard();
	}

	public void SetTurrets(BaseUpgrade[] turrets)
	{
		CurrentTurrets = Array.Empty<BaseUpgrade>();
		CurrentTurrets = turrets;
		foreach (var item in turrets)
		{
			AddTurretIcon(item);
		}
	}

	private void SetupCards()
	{
		CardContainer.GetChildren().ToList().ForEach(child => child.QueueFree());
		var parts = GameEvents.Instance.Parts;
		var currentSupply = GameEvents.Instance.Supply;
		var maxSupply = GameEvents.Instance.MaxSupply;
		var maxSupplyUpgraded = GameEvents.Instance.MaxSupplyUpgraded;
		foreach (var upgrade in availableUpgrades)
        {
            var upgradeCard = UpgradeCardScene.Instantiate() as UpgradeCard;
            CardContainer.AddChild(upgradeCard);
            upgradeCard.UpgradesScreen = this;
            upgradeCard.SetAbilityUpgrade(upgrade);
            upgradeCard.SetPrice(upgrade.Price);
            upgradeCard.SetSupply(upgrade.SupplyCost);
            upgradeCard.SetDisabledForPrice(parts < upgrade.Price);
            upgradeCard.SetDisabledForSupply(currentSupply + upgrade.SupplyCost > maxSupply);
        }
		
		if (selectedUpgrade != null) SetupSelectedCard();
		AddSupplyButton.Visible = maxSupply < maxSupplyUpgraded;
		canAddSupply = parts < GameEvents.Instance.SupplyUpgradePrice;
    }

    private void SetupSelectedCard()
    {
		var parts = GameEvents.Instance.Parts;
		var currentSupply = GameEvents.Instance.Supply;
		var maxSupply = GameEvents.Instance.MaxSupply;
        selectedCard.SetAbilityUpgrade(selectedUpgrade);
        selectedCard.SetPrice(selectedUpgrade.Price);
        selectedCard.SetSupply(selectedUpgrade.SupplyCost);
        selectedCard.SetDisabledForPrice(parts < selectedUpgrade.Price);
        selectedCard.SetDisabledForSupply(currentSupply + selectedUpgrade.SupplyCost > maxSupply);
        BuyButton.Visible = !(parts < selectedUpgrade.Price || currentSupply + selectedUpgrade.SupplyCost > maxSupply);
    }

	private void OnUpgradeCardSelected()
	{
		AddTurretIcon(selectedUpgrade);
		GameEvents.Instance.EmitPartsCollected(-selectedUpgrade.Price);
		EmitSignal(SignalName.UpgradeSelected, selectedUpgrade);
		SetupCards();
	}

	private void AddTurretIcon(BaseUpgrade upgrade)
	{
		TextureRect turretIcon = new();
		turretIcon.Texture = upgrade.Icon;
		turretContainers.AddChild(turretIcon);
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
