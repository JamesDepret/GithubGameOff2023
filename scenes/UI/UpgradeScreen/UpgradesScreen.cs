namespace UI;
public partial class UpgradesScreen : CanvasLayer
{
	[Signal] public delegate void UpgradeSelectedEventHandler(BaseUpgrade upgrade);
	public List<BaseUpgrade> CurrentTurrets { get; set; }
	public Control HelpPanel { get; set; }
	private HarvestManager harvestManager;
	private ShipUpgradeCard selectedCard;
	private BaseUpgrade selectedUpgrade;
	private HFlowContainer CardContainer;
	private VBoxContainer PermaCardsContainer;
	private Button StartWaveButton;
	private BaseUpgrade[] availableUpgrades;
	private HFlowContainer turretContainersLevel1;
	private HFlowContainer turretContainersLevel2;
	private bool canAddSupply = true;
	private BaseUpgrade? previousUpgradePointer = null;
	public override void _Ready()
	{
		HelpPanel = GetNode<Control>("HelpPanel");
		selectedCard = GetNode<ShipUpgradeCard>("ShipUpgradeCard");
		CardContainer = GetNode<HFlowContainer>("UpgradeContainer");
		PermaCardsContainer = GetNode<VBoxContainer>("PermanentUpgrades");
		StartWaveButton = GetNode<Button>("StartWaveButton");
		NormalUpgradeBuyButton = GetNode<Button>("BuyButton");

		IncomeSpeedBuyButton = GetNode<Button>("SpeedIncomeBuyButton");
		IncomeAmountBuyButton = GetNode<Button>("AddIncomeButton");
		RoomUpgradeBuyButton = GetNode<Button>("RoomUpgradeBuyButton");

		turretContainersLevel1 = GetNode<HFlowContainer>("VBoxContainer/TurretContainers");
		turretContainersLevel2 = GetNode<HFlowContainer>("VBoxContainer/TurretContainers2");
		turretContainersLevel1.GetChildren().ToList().ForEach(child => child.QueueFree());
		turretContainersLevel2.GetChildren().ToList().ForEach(child => child.QueueFree());

		harvestManager = GetNode<HarvestManager>("/root/Main/Managers/HarvestManager");

		NormalUpgradeBuyButton.Pressed += OnUpgradeCardBought;
		IncomeSpeedBuyButton.Pressed += CleanUpDetailsCard;
		IncomeAmountBuyButton.Pressed += CleanUpDetailsCard;
		RoomUpgradeBuyButton.Pressed += CleanUpDetailsCard;

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

	private void OnNextWaveStartSelected()
	{
		GetTree().Paused = false;
		QueueFree();
	}
}
