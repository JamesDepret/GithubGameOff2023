namespace UI;
public partial class UpgradesScreen : CanvasLayer
{
	[Signal] public delegate void UpgradeSelectedEventHandler(BaseUpgrade upgrade);
	public List<BaseUpgrade> CurrentTurrets { get; set; }
	[Export] PackedScene UpgradeCardScene;
	[Export] PackedScene IncomeSpeedScene;
	[Export] PackedScene IncomeAmountScene;
	[Export] PackedScene SupplyScene;
	private HarvestManager harvestManager;
	private ShipUpgradeCard selectedCard;
	private BaseUpgrade selectedUpgrade;
	private HFlowContainer CardContainer;
	private Button StartWaveButton;
	private Button NormalUpgradeBuyButton;
	private Button IncomeSpeedBuyButton;
	private Button IncomeAmountBuyButton;
	private Button RoomUpgradeBuyButton;
	private BaseUpgrade[] availableUpgrades;
	private HFlowContainer turretContainers;
	private bool canAddSupply = true;
	BaseUpgrade? previousUpgradePointer = null;
	public Control HelpPanel { get; set; }
	public override void _Ready()
	{
		HelpPanel = GetNode<Control>("HelpPanel");
		selectedCard = GetNode<ShipUpgradeCard>("ShipUpgradeCard");
		CardContainer = GetNode<HFlowContainer>("UpgradeContainer");
		StartWaveButton = GetNode<Button>("StartWaveButton");
		NormalUpgradeBuyButton = GetNode<Button>("BuyButton");

		IncomeSpeedBuyButton = GetNode<Button>("SpeedIncomeBuyButton");
		IncomeAmountBuyButton = GetNode<Button>("AddIncomeButton");
		RoomUpgradeBuyButton = GetNode<Button>("RoomUpgradeBuyButton");

		turretContainers = GetNode<HFlowContainer>("TurretContainers");
		turretContainers.GetChildren().ToList().ForEach(child => child.QueueFree());

		harvestManager = GetNode<HarvestManager>("/root/Main/Managers/HarvestManager");

		NormalUpgradeBuyButton.Pressed += OnUpgradeCardBought;
		IncomeSpeedBuyButton.Pressed += CleanUpBuyCard;
		IncomeAmountBuyButton.Pressed += CleanUpBuyCard;
		RoomUpgradeBuyButton.Pressed += CleanUpBuyCard;

		StartWaveButton.Pressed += OnNextWaveStartSelected;
		GetTree().Paused = true;
		BuyButtonSetup(BuyButtonEnum.NormalUpgrade, true);
	}

	public void SetAbilityUpgrades(BaseUpgrade[] upgrades)
	{
		availableUpgrades = upgrades;
		selectedUpgrade = null;
		selectedCard.Visible = false;
		BuyButtonSetup(BuyButtonEnum.NormalUpgrade, true);
		SetupCards();
	}

	public void SetupSelectedIncomeUpgrade(int price, Texture2D icon, string name, string description, BuyButtonEnum button)
    {
		var parts = GameEvents.Instance.Parts;
		selectedCard.Visible = true;
		selectedCard.SetupCard(price, 0, null, parts < price, false, icon);
		selectedCard.SetTexts(name, description);
		BuyButtonSetup(button, parts < price);
    }

	public enum BuyButtonEnum
	{
		IncomeSpeed,
		IncomeAmount,
		RoomUpgrade,
		NormalUpgrade
	}

	// We have multiple buy buttons, this method will show the correct one
	public void BuyButtonSetup(BuyButtonEnum button, bool disabled = false)
	{
		NormalUpgradeBuyButton.Visible = false;
		IncomeSpeedBuyButton.Visible = false;
		IncomeAmountBuyButton.Visible = false;
		RoomUpgradeBuyButton.Visible = false;
        bool canBuy;
        switch (button)
		{
			case BuyButtonEnum.NormalUpgrade:
				NormalUpgradeBuyButton.Visible = !disabled;
				break;
			case BuyButtonEnum.IncomeSpeed:
				canBuy = harvestManager.HarvestTimeLevel < 5;
				IncomeSpeedBuyButton.Visible = !disabled && canBuy;
				break;
			case BuyButtonEnum.IncomeAmount:
				canBuy = harvestManager.BaseIncome < harvestManager.MaxIncome;
				IncomeAmountBuyButton.Visible = !disabled && canBuy;
				break;
			case BuyButtonEnum.RoomUpgrade:
				RoomUpgradeBuyButton.Visible = !disabled;
				break;
		}
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
		
		SetupIncomeCards();
		if (selectedUpgrade != null) SetupSelectedCard();
		canAddSupply = parts < GameEvents.Instance.SupplyUpgradePrice;
    }

	private void SetupIncomeCards()
	{
		
		if(harvestManager.HarvestTimeLevel < 5)
		{
			var speedCard = IncomeSpeedScene.Instantiate() as SpeedIncomeButton;
			speedCard.UpgradesScreen = this;
			CardContainer.AddChild(speedCard);
		}
		
		if(harvestManager.BaseIncome < harvestManager.MaxIncome)
		{
			var amountCard = IncomeAmountScene.Instantiate() as IncomeUpgradeCard;
			amountCard.UpgradesScreen = this;
			CardContainer.AddChild(amountCard);
		}

		var maxSupply = GameEvents.Instance.MaxSupply;
		var maxSupplyUpgraded = GameEvents.Instance.MaxSupplyUpgraded;
		if (maxSupply < maxSupplyUpgraded){
			var supplyCard = SupplyScene.Instantiate() as RoomIncreaseCard;
			supplyCard.UpgradesScreen = this;
			CardContainer.AddChild(supplyCard);
		}
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
        BuyButtonSetup(BuyButtonEnum.NormalUpgrade, parts < selectedUpgrade.Price || currentSupply + selectedUpgrade.SupplyCost > maxSupply);
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
			BuyButtonSetup(BuyButtonEnum.NormalUpgrade, true);
			SetTurrets(CurrentTurrets.ToArray());
		}
	}

	private void CleanUpBuyCard()
	{
		selectedCard.Visible = false;
		BuyButtonSetup(BuyButtonEnum.NormalUpgrade, true);
		SetupCards();
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

	private void OnNextWaveStartSelected()
	{
		GetTree().Paused = false;
		QueueFree();
	}
}
