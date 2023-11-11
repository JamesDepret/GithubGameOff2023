namespace UI;
public partial class UpgradesScreen : CanvasLayer
{
	[Signal] public delegate void UpgradeSelectedEventHandler(BaseUpgrade upgrade);
	public List<BaseUpgrade> CurrentTurrets { get; set; }
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
	BaseUpgrade? previousUpgradePointer = null;
	public Control HelpPanel { get; set; }
	public override void _Ready()
	{
		HelpPanel = GetNode<Control>("HelpPanel");
		selectedCard = GetNode<ShipUpgradeCard>("ShipUpgradeCard");
		CardContainer = GetNode<HBoxContainer>("MarginContainer/CardContainer");
		DoneButton = GetNode<Button>("DoneButton");
		BuyButton = GetNode<Button>("BuyButton");
		AddSupplyButton = GetNode<Button>("TextureRect/BuyRoom");
		turretContainers = GetNode<HFlowContainer>("TurretContainers");
		turretContainers.GetChildren().ToList().ForEach(child => child.QueueFree());

		BuyButton.Pressed += OnUpgradeCardBought;
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

	public void SetSelectedUpgrade(BaseUpgrade upgrade, BaseUpgrade? previousUpgrade = null)
	{
		if (previousUpgrade != null) upgrade.PreviousUpgradePointer = previousUpgrade;
		selectedUpgrade = upgrade;
		selectedCard.Visible = true;
		SetupSelectedCard();
	}

	public void SetTurrets(BaseUpgrade[] turrets)
	{
		CurrentTurrets = new(turrets);
		
		turretContainers.GetChildren().ToList().ForEach(child => child.QueueFree());
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
			upgradeCard.SetupCard(upgrade.Price, upgrade.SupplyCost, upgrade, parts < upgrade.Price, currentSupply + upgrade.SupplyCost > maxSupply);
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
		selectedCard.SetupCard(selectedUpgrade.Price, 
							   selectedUpgrade.SupplyCost, 
							   selectedUpgrade, 
							   parts < selectedUpgrade.Price, 
							   currentSupply + selectedUpgrade.SupplyCost > maxSupply, 
							   selectedUpgrade.Icon);
        BuyButton.Visible = !(parts < selectedUpgrade.Price || currentSupply + selectedUpgrade.SupplyCost > maxSupply);
    }

	private void OnUpgradeCardBought()
	{
		
		CurrentTurrets.Add(selectedUpgrade);
		AddTurretIcon(selectedUpgrade);
		GameEvents.Instance.EmitPartsCollected(-selectedUpgrade.Price);
		EmitSignal(SignalName.UpgradeSelected, selectedUpgrade);
		SetupCards();

		if (selectedUpgrade.PreviousUpgradePointer != null)
		{
			CurrentTurrets.Remove(selectedUpgrade.PreviousUpgradePointer);
			selectedCard.Visible = false;
			BuyButton.Visible = false;
			SetTurrets(CurrentTurrets.ToArray());
		}

	}

	private void AddTurretIcon(BaseUpgrade upgrade)
	{
        CurrentTurret turretIcon = new()
        {
            TextureNormal = upgrade.Icon,
            TextureHover = upgrade.Icon,
            TexturePressed = upgrade.Icon,
            TextureFocused = upgrade.Icon,
            Tower = upgrade,
            UpgradesScreen = this
        };
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
